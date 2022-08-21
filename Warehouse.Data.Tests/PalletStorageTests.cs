using Warehouse.Data.Infrastructure;
using Warehouse.EntityContext;
using Warehouse.EntityContext.Sqlite;
using AutoMapper;

namespace Warehouse.Data.Tests
{
    public class PalletStorageTests : IClassFixture<PalletStorageFixture>
    {
        private readonly CancellationToken token = CancellationToken.None;
        private readonly IDateTimeProvider dateProvider = new DateTimeProvider();
        private readonly PalletStorageFixture fixture;
        private readonly IPalletStorage storage;

        public PalletStorageTests(PalletStorageFixture fixture)
        {
            this.fixture = fixture;
            storage = fixture.Storage;
        }

        [Fact(DisplayName = "Storage can add pallet")]
        public async Task CanAddPallet()
        {
            // Arrange
            PalletModel pallet = new(3, 5, 7, PalletStorageFixture.SpareId);

            // Act
            await storage.AddPalletAsync(pallet, token);

            // Assert
            var pallets = await storage.GetPalletAsync(PalletStorageFixture.SpareId, token);
            pallets.Should().BeEquivalentTo(new { Length = 3, Height = 5, Width = 7, Id = 11 });
        }

        [Fact(DisplayName = "Storage can retrieve pallets with SKIP pagination")]
        public async Task CanRetrievePalletWithSkipPagination()
        {
            // Arrange
            
            // Act
            var pallets = await storage.GetAllPalletsAsync(take: 100, skip: 2, token);

            // Assert
            pallets.Should().HaveCountLessThan(5);
        }

        [Fact(DisplayName = "Storage can retrieve pallets with TAKE pagination")]
        public async Task CanRetrievePalletWithTakePagination()
        {
            // Arrange
            
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
            
            // Act
            var newPallet = await storage.GetPalletAsync(PalletStorageFixture.WrongId, token);
            // Assert

            newPallet.Should().BeNull();
        }

        [Fact(DisplayName = "Storage can modify pallet")]
        public async Task CanModifyPallet()
        {
            // Arrange

            // Act
            var pallet = new PalletModel(13, 15, 17, PalletStorageFixture.ModifyId);
            await storage.UpdatePalletAsync(pallet, token);

            //Assert
            var storedPallet = await storage.GetPalletAsync(PalletStorageFixture.ModifyId, token);
            storedPallet.Should().NotBeNull();
            storedPallet.Should().BeEquivalentTo(new {Length = 13, Width = 17, Height = 15});
        }

        [Fact(DisplayName = "Storage can delete pallet")]
        public async Task CanDeletePallet()
        {
            // Arrange
            
            // Act
            await storage.DeletePalletAsync(PalletStorageFixture.GoodId, token);

            //Assert
            var storedPallet = await storage.GetPalletAsync(PalletStorageFixture.GoodId, token);
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
            addedPallet.Should().NotBeEquivalentTo(new {Id = 0});
        }
        
    }
}
