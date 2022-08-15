
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Warehouse.EntityContext.Sqlite.Configurations;

internal abstract class BaseConfiguration<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : class
{
    public virtual void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder.HasIndex("id");

        builder.HasKey("Id");

        builder.Property<int>("Length")
            .HasColumnType("INTEGER")
            .IsRequired();

        builder.Property<int>("Width")
            .HasColumnType("INTEGER")
            .IsRequired();

        builder.Property<int>("Height")
            .HasColumnType("INTEGER")
            .IsRequired();
    }
}
