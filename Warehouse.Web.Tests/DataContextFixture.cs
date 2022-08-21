using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Warehouse.EntityContext.Sqlite;
using Warehouse.EntityContext;

namespace Warehouse.Web.Api;

public class DataContextFixture: IDisposable
{
    public IWarehouseContext Context { get; private set; }
    
    public DataContextFixture()
    {
        // in-memory SQLite
        // https://docs.microsoft.com/en-us/ef/core/testing/choosing-a-testing-strategy
        // https://stackoverflow.com/questions/56319638/entityframeworkcore-sqlite-in-memory-db-tables-are-not-created
        //
        var keepAliveConnection = new SqliteConnection("DataSource=:memory:");
        keepAliveConnection.Open();
        var options = new DbContextOptionsBuilder<WarehouseSqliteContext>().UseSqlite(keepAliveConnection).Options;

        Context = new WarehouseSqliteContext(options);
        Context.Database.EnsureCreated();
    }

    public void Dispose()
    {
        Context.Database.EnsureDeleted();
    }
}
