using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Warehouse.EntityContext.Models;

namespace Warehouse.EntityContext
{
    public interface IWarehouseContext
    {
        DbSet<BoxEntity> Boxes { get; }
        DbSet<PalletEntity> Pallets { get; }

        DatabaseFacade Database { get; }
        Task<int> SaveChangesAsync();
    }
}
