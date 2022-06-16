using Microsoft.EntityFrameworkCore;

namespace Warehouse.EntityContext;

public interface IDbProvider
{
    public Task<DbContext> GetWarehouseContextAsync();
}
