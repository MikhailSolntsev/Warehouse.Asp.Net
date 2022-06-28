using Warehouse.Data.Models;
using FluentAssertions;
using AutoMapper;

namespace Warehouse.Web.Models
{
    public class DtoExtensionsTests
    {
        IMapper mapper;

        public DtoExtensionsTests()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(typeof(DtoMappingProfile));
            });

            mapper = config.CreateMapper();
        }

        [Fact(DisplayName = "Pallet with Boxes convert to PalletModel with BoxModels")]
        public void PalletsConvertWithBoxes()
        {
            Pallet pallet = new Pallet(3, 5, 7);
            Box box = new Box(3, 5, 7, 11, DateTime.Today);

            pallet.AddBox(box);

            PalletDto model = mapper.Map<PalletDto>(pallet);

            model.Boxes.Should().HaveCount(1);
        }

        [Fact(DisplayName = "Pallet without Boxes convert to PalletModel with Boxes = null")]
        public void PalletsConvertToNullWithoutBoxes()
        {
            Pallet pallet = new Pallet(3, 5, 7);

            var palletDto = mapper.Map<PalletDto>(pallet);

            palletDto.Boxes.Should().BeNull();
        }


        [Fact(DisplayName = "PalletModel with BoxModels convert to Pallet with Boxes")]
        public void PalletModelsConvertWithBoxModels()
        {
            // Arrange
            PalletDto palletModel = new PalletDto();
            BoxDto boxModel = new BoxDto();

            palletModel.Boxes = new List<BoxDto>() { boxModel };

            Pallet pallet = mapper.Map<Pallet>(palletModel);

            pallet.Boxes.Should().HaveCount(1);
        }

        [Fact(DisplayName = "PalletModel with null Id converts to Pallet with null Id")]
        public void PalletModelsWithIdConvertsCorrect()
        {
            // Arrange
            PalletDto palletModel = new PalletDto();
            BoxDto boxModel = new BoxDto();

            // Act
            Pallet pallet = mapper.Map<Pallet>(palletModel);

            // Assert
            pallet.Id.Should().BeNull("PalletModel with null Id converts to Pallet with null Id");
        }
    }
}
