using Warehouse.Data.Models;
using Warehouse.EntityContext.Models;
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
                cfg.AddProfile(typeof(ModelMappingProfile));
            });

            mapper = config.CreateMapper();
        }

        [Fact(DisplayName = "Pallets with Boxes convert to PalletModel with BoxModels")]
        public void PalletsConvertWithBoxes()
        {
            Pallet pallet = new Pallet(3, 5, 7);
            Box box = new Box(3, 5, 7, 11, DateTime.Today);

            pallet.AddBox(box);

            PalletModel model = mapper.Map<PalletModel>(pallet);

            model.Boxes.Should().HaveCount(1);
        }

        [Fact(DisplayName = "PalletModels with BoxModels convert to Pallets with Boxes")]
        public void PalletModelsConvertWithBoxModels()
        {
            // Assign
            PalletModel palletModel = new PalletModel();
            BoxModel boxModel = new BoxModel();

            palletModel.Boxes = new List<BoxModel>() { boxModel};

            Pallet pallet = mapper.Map<Pallet>(palletModel);

            pallet.Boxes.Should().HaveCount(1);
        }
    }
}
