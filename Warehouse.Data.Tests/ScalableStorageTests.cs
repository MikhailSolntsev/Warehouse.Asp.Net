using Warehouse.Data.Models;
using Warehouse.EntityContext;
using Warehouse.Data.Infrastructure;
using Warehouse.EntityContext.Sqlite;
using FluentAssertions;
using AutoMapper;

namespace Warehouse.Data
{
    public class ScalableStorageTests
    {
        private readonly IPalletStorage palletStorage;
        private readonly CancellationToken token = CancellationToken.None;

        public ScalableStorageTests()
        {
            var fileName = Path.GetRandomFileName();
            IWarehouseContext context = new WarehouseSqliteContext(fileName);
            context.Database.EnsureCreated();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(typeof(EntityMappingProfile));
            });

            IMapper mapper = config.CreateMapper();

            palletStorage = new PalletStorage(context, mapper);
        }

        [Fact(DisplayName = "Can add box to pallet")]
        public async Task CanAddBoxToPallet()
        {
            // Arrange
            PalletModel pallet = new(3, 5, 7, 11);
            await palletStorage.AddPalletAsync(pallet, token);

            var box = new BoxModel(3, 5, 7, 11, DateTime.Today);

            // Act
            await palletStorage.AddBoxToPalletAsync(box, pallet, token);

            //Assert
            var storedPallet = await palletStorage.GetPalletAsync(pallet.Id ?? 0, token);

            storedPallet.Should().NotBeNull();
            storedPallet?.Boxes?.Should().HaveCount(1);
        }

        [Fact(DisplayName = "Get all pallet retrieves pallet with boxes")]
        public async Task CanAddRetrievePalletWithBoxes()
        {
            // Arrange
            PalletModel pallet = new(3, 5, 7, 11);
            await palletStorage.AddPalletAsync(pallet, token);

            var box = new BoxModel(3, 5, 7, 11, DateTime.Today);

            // Act
            await palletStorage.AddBoxToPalletAsync(box, pallet, token);

            //Assert
            var pallets = await palletStorage.GetAllPalletsAsync(1, 0, token);
            pallets[0].Boxes?.Should().HaveCount(1);
        }

    }
}
