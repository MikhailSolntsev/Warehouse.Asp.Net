using Warehouse.Data;
using Warehouse.Data.Models;
using Warehouse.EntityContext;
using Warehouse.EntityContext.Models;
using Warehouse.EntityContext.Sqlite;
using FluentAssertions;
using AutoMapper;

namespace Warehouse.IntegrationTests
{
    public class ScalableStorageEntityContextTests
    {
        private readonly IPalletStorage palletStorage;
        private readonly IWarehouseContext context;
        private readonly CancellationToken token = CancellationToken.None;

        public ScalableStorageEntityContextTests()
        {
            string fileName = Path.GetRandomFileName();
            context = new WarehouseSqliteContext(fileName);
            context.Database.EnsureCreated();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(typeof(ModelMappingProfile));
            });

            IMapper mapper = config.CreateMapper();

            palletStorage = new PalletStorage(context, mapper);
        }

        [Fact(DisplayName = "Storage can add pallet")]
        public async Task CanAddPallet()
        {
            // Arrange
            PalletModel pallet = new(3, 5, 7, 11);

            // Act
            await palletStorage.AddPalletAsync(pallet, token);

            //Assert
            context.Pallets.Should().HaveCount(1);
        }

        [Fact(DisplayName = "Storage can modify pallet")]
        public async Task CanModifyPallet()
        {
            // Arrange
            PalletModel pallet = new(3, 5, 7, 11);
            await palletStorage.AddPalletAsync(pallet, token);

            // Act
            pallet = new(13, 5, 7, 11);
            await palletStorage.UpdatePalletAsync(pallet, token);

            //Assert
            context.Pallets.Find(pallet.Id)?.Length.Should().Be(13);
        }

        [Fact(DisplayName = "Storage can delete pallet")]
        public async Task CanDeletePallet()
        {
            // Arrange
            PalletModel pallet = new(3, 5, 7, 11);
            await palletStorage.AddPalletAsync(pallet, token);

            // Act
            await palletStorage.DeletePalletAsync(11, token);

            //Assert
            context.Pallets.Find(pallet.Id).Should().BeNull();
        }

        [Fact(DisplayName = "Can add box to pallet")]
        public async Task CanAddBoxToPallet()
        {
            // Arrange
            PalletModel pallet = new(3, 5, 7, 11);
            await palletStorage.AddPalletAsync(pallet, token);

            BoxModel box = new BoxModel(3, 5, 7, 11, DateTime.Today);

            // Act
            await palletStorage.AddBoxToPalletAsync(box, pallet, token);

            //Assert
            context.Boxes.Find(pallet.Id).Should().BeNull();
        }
    }
}
