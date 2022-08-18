using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Warehouse.EntityContext.Entities;

namespace Warehouse.EntityContext.Sqlite.Configurations
{
    internal class PalletConfiguration : BaseConfiguration<PalletEntity>
    {
        public override void Configure(EntityTypeBuilder<PalletEntity> builder)
        {
            builder.ToTable("Pallets");

            base.Configure(builder);

            builder.HasMany(pallet => pallet.Boxes);
        }
    }
}
