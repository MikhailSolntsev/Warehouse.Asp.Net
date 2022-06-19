using Warehouse.Data.Models;
using Warehouse.EntityContext.Models;
using FluentAssertions;

namespace Warehouse.EntityContext.Tests
{
    public class ModelExtensionsTest
    {
        [Fact(DisplayName = "Pallets with Boxes convert to PalletModel with BoxModels")]
        public void PalletsConvertWithBoxes()
        {
            Pallet pallet = new Pallet(3, 5, 7);
            Box box = new Box(3, 5, 7, 11, DateTime.Today);

            pallet.AddBox(box);

            PalletModel model = pallet.ToPalletModel();

            model.Boxes.Should().HaveCount(1);
        }

        [Fact(DisplayName = "PalletModels with BoxModels convert to Pallets with Boxes")]
        public void PalletModelsConvertWithBoxModels()
        {
            // Assign
            PalletModel palletModel = new PalletModel();
            BoxModel boxModel = new BoxModel();

            palletModel.Boxes = new List<BoxModel>() { boxModel};

            Pallet pallet = palletModel.ToPallet();

            pallet.Boxes.Should().HaveCount(1);
        }
    }
}
