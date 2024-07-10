using EfUnitTesting.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EfUnitTesting.Data.EntityMapping;

public class MovieMapping : IEntityTypeConfiguration<Movie>
{
    public void Configure(EntityTypeBuilder<Movie> builder)
    {
        builder
            .ToTable("Pictures")
            .UseTphMappingStrategy()
            .HasQueryFilter(movie => movie.ReleaseDate > new DateTime(1990,1,1))
            .HasKey(movie => movie.Identifier);

        builder
            .HasAlternateKey(movie => new { movie.Title, movie.ReleaseDate });

        builder.HasIndex(movie => movie.AgeRating)
            .IsDescending();

        builder.Property(movie => movie.Title)
            .HasColumnType("varchar")
            .HasMaxLength(128)
            .IsRequired();

        builder.Property(movie => movie.ReleaseDate);
    }
}