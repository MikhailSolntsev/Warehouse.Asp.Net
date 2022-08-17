using Microsoft.EntityFrameworkCore;

namespace Warehouse.EntityContext.Sqlite.Migrations;

public static class WarehouseMigrationManager
{
    public static async Task MigrateAsync()
    {
        Console.WriteLine("Database migration begin");

        var factory = new WarehouseContextFactory();

        using(var dbContext = factory.CreateDbContext(Array.Empty<string>()))
        {
            await dbContext.Database.MigrateAsync();
        }

        Console.WriteLine("Database migration finished");
    }
}
