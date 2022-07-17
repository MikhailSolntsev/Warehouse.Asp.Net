using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Warehouse.EntityContext.Models;

namespace Warehouse.EntityContext
{
    public interface IWarehouseContext
    {
        DbSet<BoxModel> Boxes { get; }
        DbSet<PalletModel> Pallets { get; }

        DatabaseFacade Database { get; }
        Task<int> SaveChangesAsync();
    }
}
