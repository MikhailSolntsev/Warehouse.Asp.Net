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

                    cfg.CreateMap<Pallet, PalletDto>()
                        .ForMember(p => p.Boxes, opt => opt.Condition(p => p.Boxes.Count > 0));
                    cfg.CreateMap<PalletDto, Pallet>()
                        .AfterMap((src, dst) => src.Boxes?.ToList().ForEach(model => dst.AddBox(model.ToBox())));
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
            InitMapper().Map<Pallet>(palletDto);

        public static PalletDto ToPalletDto(this Pallet pallet) =>
            InitMapper().Map<PalletDto>(pallet);

        public static Box ToBox(this BoxDto boxDto) =>
            InitMapper().Map<Box>(boxDto);

        public static BoxDto ToBoxDto(this Box box) =>
            InitMapper().Map<BoxDto>(box);
    }
}
