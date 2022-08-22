using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Warehouse.EntityContext.Sqlite;

namespace Warehouse.EntityContext.Tests;

public class DataContextFixture: IDisposable
{
    private static readonly object lockObject = new();
    private static bool contextInitialized = false;
    public IWarehouseContext Context { get; private set; }
    
    public DataContextFixture()
    {
        // in-memory SQLite
        // https://docs.microsoft.com/en-us/ef/core/testing/choosing-a-testing-strategy
        // https://stackoverflow.com/questions/56319638/entityframeworkcore-sqlite-in-memory-db-tables-are-not-created
        //

        lock(lockObject)
        {
            if (!contextInitialized)
            {
                var keepAliveConnection = new SqliteConnection("DataSource=:memory:");
                keepAliveConnection.Open();
                var options = new DbContextOptionsBuilder<WarehouseSqliteContext>()
                    .UseSqlite(keepAliveConnection)
                    .Options;

                Context = new WarehouseSqliteContext(options);

                Context.Database.EnsureDeleted();
                Context.Database.EnsureCreated();

                contextInitialized = true;
            }
        }
        
    }

    public void Dispose()
    {
        Context.Database.EnsureCreated();
    }
}
