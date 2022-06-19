using Microsoft.EntityFrameworkCore;
using Warehouse.EntityContext.Models;

namespace Warehouse.EntityContext
{
    public abstract class WarehouseContext : DbContext
    {
        protected WarehouseContext() : base()
        {
        }
        public WarehouseContext(DbContextOptions<WarehouseContext> options) : base(options)
        {
        }
        public abstract DbSet<BoxModel>? Boxes { get; set; }
        public abstract DbSet<PalletModel>? Pallets { get; set; }
    }
}
