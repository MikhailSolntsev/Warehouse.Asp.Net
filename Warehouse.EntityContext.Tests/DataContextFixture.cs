using Warehouse.EntityContext.Sqlite;

namespace Warehouse.EntityContext.Tests;

public class DataContextFixture: IDisposable
{
    public IWarehouseContext Context { get; private set; }
    private readonly string fileName;

    public DataContextFixture()
    {
        fileName = Path.GetRandomFileName();

        Context = new WarehouseSqliteContext(fileName);
        Context.Database.EnsureCreated();
    }

    public void Dispose()
    {
        var deleted = Context.Database.EnsureDeleted();
    }
}
