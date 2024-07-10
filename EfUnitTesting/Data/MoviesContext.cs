using EfUnitTesting.Data.EntityMapping;
using EfUnitTesting.Data.Interceptors;
using EfUnitTesting.Models;
using EfUnitTesting.QueryFilter.Tenants;
using Microsoft.EntityFrameworkCore;

namespace EfUnitTesting.Data;

public class MoviesContext : DbContext
{
    private readonly TenantService _tenantService;
    public string? TenantId => _tenantService.GetTenantId();

    public MoviesContext() { }

    public MoviesContext(DbContextOptions<MoviesContext> options, TenantService tenant)
        :base(options)
    {
        _tenantService = tenant;
    }
    
    public DbSet<Genre> Genres => Set<Genre>();
    public DbSet<Movie> Movies => Set<Movie>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(new SaveChangesInterceptor());

        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new MovieMapping());
        modelBuilder.ApplyConfiguration(new GenreMapping(this));
    }
}