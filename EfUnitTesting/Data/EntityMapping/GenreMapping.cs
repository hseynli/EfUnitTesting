using EfUnitTesting.Data.ValueGenerators;
using EfUnitTesting.Models;
using EfUnitTesting.QueryFilter.Tenants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EfUnitTesting.Data.EntityMapping;

public class GenreMapping : TenantAwareMapping<Genre>
{
    public GenreMapping(MoviesContext context) : base(context)
    { }

    public override void ConfigureEntity(EntityTypeBuilder<Genre> builder)
    {
        builder.Property<DateTime>("CreatedDate")
            .HasColumnName("CreatedAt")
            .HasValueGenerator<CreatedDateGenerator>();
    }
}