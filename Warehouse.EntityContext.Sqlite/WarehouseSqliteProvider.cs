using Microsoft.EntityFrameworkCore;

namespace Warehouse.EntityContext.Sqlite;

public class WarehouseSqliteProvider : IDbProvider
{
    private string fileName;

    public WarehouseSqliteProvider()
    {
        fileName = "../Warehouse.db";
    }
    public WarehouseSqliteProvider(string fileName)
    {
        this.fileName = fileName;
    }

    public async Task<DbContext> GetWarehouseContextAsync()
    {
        WarehouseContext db = new(fileName);

        bool created = await db.Database.EnsureCreatedAsync();

        if (created)
        {
            return db;
        }

        throw new DbUpdateException($"Can't create database with file {fileName}");
    }
}
