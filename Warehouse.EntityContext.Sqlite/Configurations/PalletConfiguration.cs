using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Warehouse.EntityContext.Models;

namespace Warehouse.EntityContext.Sqlite.Configurations
{
    internal class PalletConfiguration : IEntityTypeConfiguration<PalletEntity>
    {
        public void Configure(EntityTypeBuilder<PalletEntity> builder)
        {
            builder.ToTable("Pallets")
                .HasIndex("Id");

            builder.HasKey(pallet => pallet.Id);

            builder.Property(pallet => pallet.Length)
                .HasColumnType("INTEGER")
                .IsRequired();

            builder.Property(pallet => pallet.Width)
                .HasColumnType("INTEGER")
                .IsRequired();

            builder.Property(pallet => pallet.Height)
                .HasColumnType("INTEGER")
                .IsRequired();

            builder.HasMany(pallet => pallet.Boxes);
        }
    }
}
