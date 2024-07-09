using Dometrain.EFCore.Tenants.QueryFilter.Tenants;
using EfUnitTesting.Data.EntityMapping;
using EfUnitTesting.Models;
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

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new GenreMapping(this));
    }
}