using AutoMapper;
using Warehouse.Data.Models;

namespace Warehouse.Web.Models
{
    public class DtoMappingProfile : Profile
    {
        public DtoMappingProfile()
        {
            CreateMap<BoxModel, BoxDto>();
            CreateMap<BoxDto, BoxModel>()
                .ForMember(b => b.Id, opt => opt.Condition(b => b.Id is not null));

            CreateMap<PalletModel, PalletDto>()
                .ForMember(p => p.Boxes, opt => opt.Condition(p => p.Boxes.Count > 0));
            CreateMap<PalletDto, PalletModel>()
                .ForMember(p => p.Id, opt => opt.Condition(p => p.Id is not null))
                .ForMember(p => p.ExpirationDate, opt => opt.Ignore());
        }
    }
}
