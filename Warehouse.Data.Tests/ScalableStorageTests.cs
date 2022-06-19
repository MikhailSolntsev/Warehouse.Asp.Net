using Warehouse.Data.Models;
using Warehouse.EntityContext;
using Warehouse.EntityContext.Sqlite;
using FluentAssertions;

namespace Warehouse.Data
{
    public class ScalableStorageTests
    {
        public ScalableStorageTests()
        {

        }

        [Fact(DisplayName = "Storage can add pallet")]
        public void CanAddPallet()
        {
            // Assign
            string fileName = Path.GetRandomFileName();
            // TODO: make injection
            WarehouseContext context = new WarehouseSqliteContext(fileName);
            ScalableStorage storage = new ScalableStorage(context);
            Pallet pallet = new(3, 5, 7, 11);

            // Act
            storage.AddPalletAsync(pallet);

            storage.GetAllPalletsAsync().Result.Count.Should().Be(1);
        }

        [Fact(DisplayName = "Storage can modify pallet")]
        public void CanModifyPallet()
        {
            // Assign
            string fileName = Path.GetRandomFileName();
            // TODO: make injection
            WarehouseContext context = new WarehouseSqliteContext(fileName);
            ScalableStorage storage = new ScalableStorage(context);
            Pallet pallet = new(3, 5, 7, 11);
            storage.AddPalletAsync(pallet);

            // Act
            pallet = new(13, 5, 7, 11);
            storage.UpdatePalletAsync(pallet);

            //Assert
            var storedPallet = storage.GetPalletAsync(pallet.Id).Result;
            storedPallet.Should().NotBeNull();
            storedPallet.Length.Should().Be(13);
        }

        [Fact(DisplayName = "Storage can delete pallet")]
        public void CanDeletePallet()
        {
            // Assign
            string fileName = Path.GetRandomFileName();
            // TODO: make injection
            WarehouseContext context = new WarehouseSqliteContext(fileName);
            ScalableStorage storage = new ScalableStorage(context);
            Pallet pallet = new(3, 5, 7, 11);
            storage.AddPalletAsync(pallet);

            // Act
            storage.DeletePalletAsync(pallet);

            //Assert
            storage.GetPalletAsync(pallet.Id).Result.Should().BeNull();
        }

        [Fact(DisplayName = "Can add box to pallet")]
        public void CanAddBoxToPallet()
        {
            // Assign
            string fileName = Path.GetRandomFileName();
            // TODO: make injection
            WarehouseContext context = new WarehouseSqliteContext(fileName);
            ScalableStorage storage = new ScalableStorage(context);
            Pallet pallet = new(3, 5, 7, 11);
            storage.AddPalletAsync(pallet);

            Box box = new Box(3, 5, 7, 11, DateTime.Today);

            // Act
            storage.AddBoxToPalletAsync(box, pallet);

            //Assert
            storage.GetPalletAsync(pallet.Id).Result.Boxes.Should().HaveCount(1);
        }
    }
}
