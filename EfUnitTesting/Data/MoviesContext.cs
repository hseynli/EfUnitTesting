using EfUnitTesting.Data.EntityMapping;
using EfUnitTesting.Models;
using Microsoft.EntityFrameworkCore;

namespace EfUnitTesting.Data;

public class MoviesContext : DbContext
{
    public MoviesContext() { }

    public MoviesContext(DbContextOptions<MoviesContext> options)
        :base(options)
    { }
    
    public DbSet<Genre> Genres => Set<Genre>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new GenreMapping());
    }
}