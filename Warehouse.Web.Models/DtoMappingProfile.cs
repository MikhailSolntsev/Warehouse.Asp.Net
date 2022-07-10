using AutoMapper;
using Warehouse.Data.Models;

namespace Warehouse.Web.Models
{
    public class DtoMappingProfile : Profile
    {
        public DtoMappingProfile()
        {
            CreateMap<Box, BoxDto>();
            CreateMap<BoxDto, Box>()
                .ForMember(b => b.Id, opt => opt.Condition(b => b.Id is not null));

            CreateMap<Pallet, PalletDto>()
                .ForMember(p => p.Boxes, opt => opt.Condition(p => p.Boxes.Count > 0));
            CreateMap<PalletDto, Pallet>()
                .ForMember(p => p.Id, opt => opt.Condition(p => p.Id is not null));
        }
    }
}
