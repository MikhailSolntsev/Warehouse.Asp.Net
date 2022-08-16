using Warehouse.Data.Models;
using Warehouse.EntityContext;
using Warehouse.EntityContext.Models;
using Warehouse.EntityContext.Sqlite;
using FluentAssertions;
using AutoMapper;

namespace Warehouse.Data
{
    public class ScalableStorageTests
    {
        private readonly IPalletStorage palletStorage;

        public ScalableStorageTests()
        {
            var fileName = Path.GetRandomFileName();
            IWarehouseContext context = new WarehouseSqliteContext(fileName);
            context.Database.EnsureCreated();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(typeof(ModelMappingProfile));
            });

            IMapper mapper = config.CreateMapper();

            palletStorage = new PalletStorage(context, mapper);
        }

        [Fact(DisplayName = "Can add box to pallet")]
        public async Task CanAddBoxToPallet()
        {
            // Arrange
            PalletModel pallet = new(3, 5, 7, 11);
            await palletStorage.AddPalletAsync(pallet);

            var box = new BoxModel(3, 5, 7, 11, DateTime.Today);

            // Act
            await palletStorage.AddBoxToPalletAsync(box, pallet);

            //Assert
            var storedPallet = await palletStorage.GetPalletAsync(pallet.Id ?? 0);

            storedPallet.Should().NotBeNull();
            storedPallet?.Boxes?.Should().HaveCount(1);
        }

        [Fact(DisplayName = "Get all pallet retrieves pallet with boxes")]
        public async Task CanAddRetrievePalletWithBoxes()
        {
            // Arrange
            PalletModel pallet = new(3, 5, 7, 11);
            await palletStorage.AddPalletAsync(pallet);

            var box = new BoxModel(3, 5, 7, 11, DateTime.Today);

            // Act
            await palletStorage.AddBoxToPalletAsync(box, pallet);

            //Assert
            var pallets = await palletStorage.GetAllPalletsAsync(1, null);
            pallets[0].Boxes?.Should().HaveCount(1);
        }

    }
}
