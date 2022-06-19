using Warehouse.Data;
using Warehouse.Data.Models;
using Warehouse.EntityContext;
using Warehouse.EntityContext.Sqlite;
using FluentAssertions;

namespace Warehouse.IntegrationTests
{
    public class ScalableStorageEntityContextTests
    {
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

            //Assert
            context.Pallets.Should().HaveCount(1);
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
            context.Pallets?.Find(pallet.Id)?.Length.Should().Be(13);
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
            context.Pallets?.Find(pallet.Id).Should().BeNull();
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
            context.Boxes?.Find(pallet.Id).Should().BeNull();
        }
    }
}
