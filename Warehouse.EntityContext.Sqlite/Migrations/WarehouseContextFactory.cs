using Microsoft.Extensions.Configuration;

namespace Warehouse.EntityContext.Sqlite.Migrations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

public class WarehouseContextFactory : IDesignTimeDbContextFactory<WarehouseSqliteContext>
{
    public WarehouseSqliteContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<WarehouseSqliteContext>();

        string fileName = WarehouseSqliteContext.DefaultFileName;

        optionsBuilder.UseSqlite($"Data Source ={fileName}");

        return new WarehouseSqliteContext(fileName, optionsBuilder.Options);
    }
}
