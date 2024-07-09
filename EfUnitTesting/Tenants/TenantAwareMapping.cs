using EfUnitTesting.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EfUnitTesting.QueryFilter.Tenants;

public abstract class TenantAwareMapping<TEntity> : IEntityTypeConfiguration<TEntity> 
    where TEntity : class
{
    private readonly MoviesContext _context;

    protected TenantAwareMapping(MoviesContext context)
    {
        _context = context;
    }
    
    public void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder
            .Property<string?>("TenantId")
            .HasColumnType("varchar(32)");
        builder.HasIndex("TenantId");
        builder
            .HasQueryFilter(entity =>
                EF.Property<string>(entity, "TenantId") 
                == _context.TenantId);
        
        ConfigureEntity(builder);
    }

    public abstract void ConfigureEntity(EntityTypeBuilder<TEntity> builder);
}