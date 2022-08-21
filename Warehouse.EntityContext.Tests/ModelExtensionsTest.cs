using Warehouse.Data.Infrastructure;
using Warehouse.Data.Models;
using Warehouse.EntityContext.Entities;
using FluentAssertions;
using AutoMapper;

namespace Warehouse.EntityContext.Tests
{
    public class ModelExtensionsTest
    {
        private IMapper mapper;

        public ModelExtensionsTest()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(typeof(EntityMappingProfile));
            });

            mapper = config.CreateMapper();
        }

        [Fact(DisplayName = "Pallets with Boxes convert to PalletModel with BoxModels")]
        public void PalletsConvertWithBoxes()
        {
            PalletModel pallet = new PalletModel(3, 5, 7);
            BoxModel box = new BoxModel(3, 5, 7, 11, DateTime.Today);

            pallet.AddBox(box);

            PalletEntity model = mapper.Map<PalletEntity>(pallet);

            model.Boxes.Should().HaveCount(1);
        }

        [Fact(DisplayName = "PalletModels with BoxModels convert to Pallets with Boxes")]
        public void PalletModelsConvertWithBoxModels()
        {
            // Arrange
            PalletEntity palletModel = new PalletEntity();
            BoxEntity boxModel = new BoxEntity();

            palletModel.Boxes = new List<BoxEntity>() { boxModel};

            PalletModel pallet = mapper.Map<PalletModel>(palletModel);

            pallet.Boxes.Should().HaveCount(1);
        }
    }
}
