using Warehouse.Data.Models;

namespace Warehouse.Web.Models
{
    public static class DtoExtensions
    {
        public static Pallet ToPallet(this PalletDto palletDto) =>
            new Pallet(
                palletDto.Length,
                palletDto.Height,
                palletDto.Width,
                palletDto.Id);

        public static PalletDto toPalletDto(this Pallet pallet) =>
            new PalletDto()
            {
                Id = pallet.Id,
                Length = pallet.Length,
                Width = pallet.Width,
                Height = pallet.Height
            };

        public static Box ToBox(this BoxDto boxDto) =>
            new Box(
                boxDto.Length,
                boxDto.Height,
                boxDto.Width,
                boxDto.Weight,
                boxDto.ExpirationDate,
                boxDto.Id);

        public static BoxDto toBoxDto(this Box box) =>
            new BoxDto()
            {
                Id = box.Id,
                Length = box.Length,
                Width = box.Width,
                Height = box.Height,
                Weight = box.Weight,
                ExpirationDate = box.ExpirationDate
            };
    }
}
