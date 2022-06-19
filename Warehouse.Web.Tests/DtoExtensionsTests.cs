using Warehouse.Data.Models;
using FluentAssertions;

namespace Warehouse.Web.Models
{
    public class DtoExtensionsTests
    {
        [Fact(DisplayName = "Pallet with Boxes convert to PalletModel with BoxModels")]
        public void PalletsConvertWithBoxes()
        {
            Pallet pallet = new Pallet(3, 5, 7);
            Box box = new Box(3, 5, 7, 11, DateTime.Today);

            pallet.AddBox(box);

            PalletDto model = pallet.ToPalletDto();

            model.Boxes.Should().HaveCount(1);
        }

        [Fact(DisplayName = "Pallet without Boxes convert to PalletModel with Boxes = null")]
        public void PalletsConvertToNullWithoutBoxes()
        {
            Pallet pallet = new Pallet(3, 5, 7);

            var palletDto = pallet.ToPalletDto();

            palletDto.Boxes.Should().BeNull();
        }


        [Fact(DisplayName = "PalletModel with BoxModels convert to Pallet with Boxes")]
        public void PalletModelsConvertWithBoxModels()
        {
            // Assign
            PalletDto palletModel = new PalletDto();
            BoxDto boxModel = new BoxDto();

            palletModel.Boxes = new List<BoxDto>() { boxModel };

            Pallet pallet = palletModel.ToPallet();

            pallet.Boxes.Should().HaveCount(1);
        }
    }
}
