using Warehouse.Data.Infrastructure;
using Warehouse.EntityContext;
using Warehouse.EntityContext.Sqlite;
using AutoMapper;

namespace Warehouse.Data.Tests
{
    public class PalletStorageTests
    {
        private readonly IPalletStorage storage;
        private readonly CancellationToken token = CancellationToken.None;
        private readonly IDateTimeProvider dateProvider = new DateTimeProvider();

        public PalletStorageTests()
        {
            string fileName = Path.GetRandomFileName();
            IWarehouseContext context = new WarehouseSqliteContext(fileName);
            context.Database.EnsureCreated();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(typeof(EntityMappingProfile));
            });

            IMapper mapper = config.CreateMapper();

            storage = new PalletStorage(context, mapper);
        }

        [Fact(DisplayName = "Storage can add pallet")]
        public async Task CanAddPallet()
        {
            // Arrange
            PalletModel pallet = new(3, 5, 7, 11);

            // Act
            await storage.AddPalletAsync(pallet, token);

            // Assert
            var pallets = await storage.GetAllPalletsAsync(100, 0, token);

            pallets.Should().HaveCount(1);
            pallets.First().Should().BeEquivalentTo(new { Length = 3, Height = 5, Width = 7, Id = 11 });
        }

        [Fact(DisplayName = "Storage can retrieve pallets with SKIP pagination")]
        public async Task CanRetrievePalletWithSkipPagination()
        {
            // Arrange
            await FillStorageWithPalletAndBoxesAsync();

            // Act
            var pallets = await storage.GetAllPalletsAsync(take: 100, skip: 2, token);

            // Assert
            pallets.Should().HaveCount(3);
        }

        [Fact(DisplayName = "Storage can retrieve pallets with TAKE pagination")]
        public async Task CanRetrievePalletWithTakePagination()
        {
            // Arrange
            await FillStorageWithPalletAndBoxesAsync();

            // Act
            var pallets = await storage.GetAllPalletsAsync(2, 0, token);

            // Assert
            pallets.Should().HaveCount(2);
        }

        [Fact(DisplayName = "Adding pallet should return new Pallet, not null")]
        public async Task AddingPalletShouldReturnNewPallet()
        {
            // Arrange
            PalletModel pallet = new(3, 5, 7);
            BoxModel box = new BoxModel(3, 3, 3, 3, dateProvider.Today());
            pallet.AddBox(box);

            // Act
            var newPallet = await storage.AddPalletAsync(pallet, token);

            // Assert
            newPallet.Should().NotBeNull();

            newPallet.Should().BeEquivalentTo(new { Length = 3, Height = 5, Width = 7});
            newPallet!.Boxes.Should().HaveCount(1);
            newPallet.Boxes[0].Should().BeEquivalentTo(new { Length = 3, Height = 3, Width = 3, Weight = 3, ExpirationDate = dateProvider.Today()});
        }

        [Fact(DisplayName = "Get pallet with wrong Id should return null")]
        public async Task GetPalletWithWrongIdShouldReturnNull()
        {
            // Arrange
            PalletModel pallet = new(3, 5, 7, 11);
            
            // Act
            await storage.AddPalletAsync(pallet, token);
            var newPallet = await storage.GetPalletAsync(100, token);
            // Assert

            newPallet.Should().BeNull();
        }

        [Fact(DisplayName = "Storage can modify pallet")]
        public async Task CanModifyPallet()
        {
            // Arrange
            PalletModel pallet = new(3, 5, 7, 11);
            await storage.AddPalletAsync(pallet, token);

            // Act
            pallet = new(13, 5, 7, 11);
            await storage.UpdatePalletAsync(pallet, token);

            //Assert
            var storedPallet = await storage.GetPalletAsync(pallet.Id??0, token);
            storedPallet.Should().NotBeNull();
            storedPallet?.Length.Should().Be(13);
        }

        [Fact(DisplayName = "Storage can delete pallet")]
        public async Task CanDeletePallet()
        {
            // Arrange
            PalletModel pallet = new(3, 5, 7, 11);
            await storage.AddPalletAsync(pallet, token);

            // Act
            await storage.DeletePalletAsync(11, token);

            //Assert
            var storedPallet = await storage.GetPalletAsync(pallet.Id ?? 0, token);
            storedPallet.Should().BeNull();
        }

        [Fact(DisplayName = "Pallet without Id stores with Id")]
        public async void PalletWithoutIdStoresWithId()
        {
            // Arrange
            PalletModel pallet = new(3, 5, 7);

            // Act
            var addedPallet = await storage.AddPalletAsync(pallet, token);

            // Assert
            addedPallet.Should().NotBeNull();
            addedPallet?.Id.Should().NotBe(0);
        }

        private async Task FillStorageWithPalletAndBoxesAsync()
        {
            PalletModel pallet = new(3, 5, 7, 11);
            await storage.AddPalletAsync(pallet, token);
            pallet = new(3, 5, 7, 13);
            await storage.AddPalletAsync(pallet, token);
            pallet = new(3, 5, 7, 17);
            await storage.AddPalletAsync(pallet, token);
            pallet = new(3, 5, 7, 19);
            await storage.AddPalletAsync(pallet, token);
            pallet = new(3, 5, 7, 23);
            await storage.AddPalletAsync(pallet, token);
        }

    }
}
