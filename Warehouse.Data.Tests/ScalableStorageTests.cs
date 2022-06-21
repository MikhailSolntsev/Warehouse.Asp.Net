using Warehouse.Data.Models;
using Warehouse.EntityContext;
using Warehouse.EntityContext.Sqlite;
using FluentAssertions;

namespace Warehouse.Data
{
    public class ScalableStorageTests
    {
        private ScalableStorage storage;
        public ScalableStorageTests()
        {
            string fileName = Path.GetRandomFileName();
            WarehouseContext context = new WarehouseSqliteContext(fileName);
            storage = new ScalableStorage(context);
        }

        [Fact(DisplayName = "Storage can add pallet")]
        public async Task CanAddPallet()
        {
            // Assign
            Pallet pallet = new(3, 5, 7, 11);

            // Act
            await storage.AddPalletAsync(pallet);

            // Assert
            var pallets = await storage.GetAllPalletsAsync();
            pallets.Should().HaveCount(1);
        }

        [Fact(DisplayName = "Adding pallet should return new Pallet, not null")]
        public async Task AddingPalletShouldReturnNewPallet()
        {
            // Assign
            Pallet pallet = new(3, 5, 7);
            Box box = new Box(3, 3, 3, 3, DateTime.Now);
            pallet.AddBox(box);

            // Act
            var newPallet = await storage.AddPalletAsync(pallet);

            // Assert
            newPallet.Should().NotBeNull();
        }

        [Fact(DisplayName = "Storage can modify pallet")]
        public async Task CanModifyPallet()
        {
            // Assign
            Pallet pallet = new(3, 5, 7, 11);
            await storage.AddPalletAsync(pallet);

            // Act
            pallet = new(13, 5, 7, 11);
            await storage.UpdatePalletAsync(pallet);

            //Assert
            var storedPallet = await storage.GetPalletAsync(pallet.Id??0);
            storedPallet.Should().NotBeNull();
            storedPallet.Length.Should().Be(13);
        }

        [Fact(DisplayName = "Storage can delete pallet")]
        public async Task CanDeletePallet()
        {
            // Assign
            Pallet pallet = new(3, 5, 7, 11);
            await storage.AddPalletAsync(pallet);

            // Act
            await storage.DeletePalletAsync(pallet);

            //Assert
            var storedPallet = await storage.GetPalletAsync(pallet.Id ?? 0);
            storedPallet.Should().BeNull();
        }

        [Fact(DisplayName = "Can add box to pallet")]
        public async Task CanAddBoxToPallet()
        {
            // Assign
            Pallet pallet = new(3, 5, 7, 11);
            await storage.AddPalletAsync(pallet);

            Box box = new Box(3, 5, 7, 11, DateTime.Today);

            // Act
            await storage.AddBoxToPalletAsync(box, pallet);

            //Assert
            var storedPallet = await storage.GetPalletAsync(pallet.Id ?? 0);
            storedPallet.Boxes.Should().HaveCount(1);
        }

        [Fact(DisplayName = "Pallet without Id stores with Id")]
        public void PalletWithoutIdStoresWithId()
        {
            // Assign
            Pallet pallet = new(3, 5, 7);

            // Act
            var addedPallet = storage.AddPalletAsync(pallet).Result;

            // Assert
            addedPallet.Id.Should().NotBe(0);
        }
    }
}
