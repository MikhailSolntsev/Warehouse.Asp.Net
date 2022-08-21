using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Warehouse.EntityContext.Entities;

namespace Warehouse.EntityContext.Sqlite.Configurations
{
    internal class BoxConfiguration : BaseConfiguration<BoxEntity>
    {
        public override void Configure(EntityTypeBuilder<BoxEntity> builder)
        {
            builder.ToTable("Boxes");

            base.Configure(builder);

            builder.Property<int?>("Weight")
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
