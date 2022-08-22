using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

using Warehouse.EntityContext.Sqlite;
using Warehouse.EntityContext;

namespace Warehouse.Data.Tests;

public static class DataContextFactory
{
    public static IWarehouseContext GetContext()
    {
        // in-memory SQLite
        // https://docs.microsoft.com/en-us/ef/core/testing/choosing-a-testing-strategy
        // https://stackoverflow.com/questions/56319638/entityframeworkcore-sqlite-in-memory-db-tables-are-not-created
        //

        var keepAliveConnection = new SqliteConnection("DataSource=:memory:");
        keepAliveConnection.Open();
        var options = new DbContextOptionsBuilder<WarehouseSqliteContext>()
            .UseSqlite(keepAliveConnection)
            .Options;

        IWarehouseContext context = new WarehouseSqliteContext(options);

        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        return context;
    }
}
