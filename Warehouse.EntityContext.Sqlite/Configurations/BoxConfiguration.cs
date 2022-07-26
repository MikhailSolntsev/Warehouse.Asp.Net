using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Warehouse.EntityContext.Models;

namespace Warehouse.EntityContext.Sqlite.Configurations
{
    internal class BoxConfiguration : IEntityTypeConfiguration<BoxEntity>
    {
        public void Configure(EntityTypeBuilder<BoxEntity> builder)
        {
            builder.ToTable("Boxes")
                .HasIndex("Id");

            builder.HasKey(box => box.Id);

            builder.Property(box => box.Length)
                .HasColumnType("INTEGER")
                .IsRequired();

            builder.Property(box => box.Width)
                .HasColumnType("INTEGER")
                .IsRequired();

            builder.Property(box => box.Height)
                .HasColumnType("INTEGER")
                .IsRequired();

            builder.Property(box => box.Weight)
                .HasColumnType("INTEGER")
                .IsRequired();

            builder.Property(box => box.ExpirationDate)
                .HasColumnType("datetime")
                .IsRequired();

            builder.Property(box => box.PalletModelId)
                .HasColumnName("PalletId")
                .HasColumnType("INTEGER");

            builder.HasOne(box => box.PalletModel)
                .WithMany(pallet => pallet.Boxes);
        }
    }
}
