using Warehouse.Data.Models;
using Warehouse.EntityContext.Sqlite;
using FluentAssertions;

namespace Warehouse.Data.Tests
{
    public class ScalableStorageTests
    {
        [Fact]
        public void CanAddBox()
        {
            // Assign
            string fileName = Path.GetRandomFileName();
            ScalableStorage storage = new(new WarehouseSqliteProvider(fileName));

            Box box = new(3, 5, 7, 11, DateTime.Today);

            storage.AddBox(box);
        }
    }
}
