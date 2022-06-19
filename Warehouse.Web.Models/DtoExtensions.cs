using Warehouse.Data.Models;
using AutoMapper;

namespace Warehouse.Web.Models
{
    public static class DtoExtensions
    {
        private static IMapper? mapper;

        private static IMapper InitMapper()
        {
            if (mapper is null)
            {
                var configuration = new MapperConfiguration(cfg =>
                {
                    cfg.CreateMap<Box, BoxDto>();
                    cfg.CreateMap<BoxDto, Box>();

                    cfg.CreateMap<ICollection<BoxDto>, IReadOnlyList<Box>>()
                        .ConvertUsing(collection => collection.Select(model => model.ToBox()).ToList());

                    cfg.CreateMap<IReadOnlyList<Box>, ICollection<BoxDto>>()
                        .ConvertUsing(list => list.Select(box => box.ToBoxDto()).ToList());

                    cfg.CreateMap<Pallet, PalletDto>();
                    cfg.CreateMap<PalletDto, Pallet>()
                        .AfterMap((src, dst) => src.Boxes.ToList().ForEach(model => dst.AddBox(model.ToBox())));
                });
                mapper = configuration.CreateMapper();
            }

            if (mapper is null)
            {
                throw new Exception("Can't create mappper for models");
            }

            return mapper;
        }
        public static Pallet ToPallet(this PalletDto palletDto) =>
            new Pallet(
                palletDto.Length,
                palletDto.Height,
                palletDto.Width,
                palletDto.Id);

        public static PalletDto ToPalletDto(this Pallet pallet) =>
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

        public static BoxDto ToBoxDto(this Box box) =>
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
