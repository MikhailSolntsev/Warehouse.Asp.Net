using Warehouse.Data.Models;
using Warehouse.EntityContext;
using Warehouse.EntityContext.Models;
using Warehouse.EntityContext.Sqlite;
using FluentAssertions;
using AutoMapper;

namespace Warehouse.Data
{
    public class PalletStorageTests
    {
        private IPalletStorage storage;

        public PalletStorageTests()
        {
            string fileName = Path.GetRandomFileName();
            IWarehouseContext context = new WarehouseSqliteContext(fileName);
            context.Database.EnsureCreated();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(typeof(ModelMappingProfile));
            });

            IMapper mapper = config.CreateMapper();

            storage = new PalletStorage(context, mapper);
        }

        [Fact(DisplayName = "Storage can add box")]
        public async Task CanAddPallet()
        {
            // Arrange
            PalletModel pallet = new(3, 5, 7, 11);

            // Act
            await storage.AddPalletAsync(pallet);

            // Assert
            var pallets = await storage.GetAllPalletsAsync(100, null);
            pallets.Should().HaveCount(1);
        }

        [Fact(DisplayName = "Storage can retrieve pallets with SKIP pagination")]
        public async Task CanRetrievePalletWithSkipPagination()
        {
            // Arrange
            await FillStorageWithPalletAndBoxesAsync();

            // Act
            var pallets = await storage.GetAllPalletsAsync(take: 100, skip: 2);

            // Assert
            pallets.Should().HaveCount(3);
        }

        [Fact(DisplayName = "Storage can retrieve pallets with TAKE pagination")]
        public async Task CanRetrievePalletWithTakePagination()
        {
            // Arrange
            await FillStorageWithPalletAndBoxesAsync();

            // Act
            var pallets = await storage.GetAllPalletsAsync(2, null);

            // Assert
            pallets.Should().HaveCount(2);
        }

        [Fact(DisplayName = "Adding pallet should return new Pallet, not null")]
        public async Task AddingPalletShouldReturnNewPallet()
        {
            // Arrange
            PalletModel pallet = new(3, 5, 7);
            BoxModel box = new BoxModel(3, 3, 3, 3, DateTime.Now);
            pallet.AddBox(box);

            // Act
            var newPallet = await storage.AddPalletAsync(pallet);

            // Assert
            newPallet.Should().NotBeNull();
        }

        [Fact(DisplayName = "Get pallet with wrong Id should return null")]
        public async Task GetPalletWithWrongIdShouldReturnNull()
        {
            // Arrange
            PalletModel pallet = new(3, 5, 7);
            
            // Act
            await storage.AddPalletAsync(pallet);
            var newPallet = await storage.GetPalletAsync(100);
            // Assert

            newPallet.Should().BeNull();
        }

        [Fact(DisplayName = "Storage can modify pallet")]
        public async Task CanModifyPallet()
        {
            // Arrange
            PalletModel pallet = new(3, 5, 7, 11);
            await storage.AddPalletAsync(pallet);

            // Act
            pallet = new(13, 5, 7, 11);
            await storage.UpdatePalletAsync(pallet);

            //Assert
            var storedPallet = await storage.GetPalletAsync(pallet.Id??0);
            storedPallet.Should().NotBeNull();
            storedPallet?.Length.Should().Be(13);
        }

        [Fact(DisplayName = "Storage can delete pallet")]
        public async Task CanDeletePallet()
        {
            // Arrange
            PalletModel pallet = new(3, 5, 7, 11);
            await storage.AddPalletAsync(pallet);

            // Act
            await storage.DeletePalletAsync(11);

            //Assert
            var storedPallet = await storage.GetPalletAsync(pallet.Id ?? 0);
            storedPallet.Should().BeNull();
        }

        [Fact(DisplayName = "Pallet without Id stores with Id")]
        public void PalletWithoutIdStoresWithId()
        {
            // Arrange
            PalletModel pallet = new(3, 5, 7);

            // Act
            var addedPallet = storage.AddPalletAsync(pallet).Result;

            // Assert
            addedPallet.Should().NotBeNull();
            addedPallet.Id.Should().NotBe(0);
        }

        private async Task FillStorageWithPalletAndBoxesAsync()
        {
            PalletModel pallet = new(3, 5, 7, 11);
            await storage.AddPalletAsync(pallet);
            pallet = new(3, 5, 7, 13);
            await storage.AddPalletAsync(pallet);
            pallet = new(3, 5, 7, 17);
            await storage.AddPalletAsync(pallet);
            pallet = new(3, 5, 7, 19);
            await storage.AddPalletAsync(pallet);
            pallet = new(3, 5, 7, 23);
            await storage.AddPalletAsync(pallet);
        }

    }
}
