using AutoMapper;
using Warehouse.Data.Models;
using Warehouse.Web.Models;

namespace Warehouse.Web.Api.Infrastructure.Mapping
{
    public class DtoMappingProfile : Profile
    {
        public DtoMappingProfile()
        {
            CreateMap<BoxModel, BoxResponseDto>();

            CreateMap<BoxCreateDto, BoxModel>()
                .ForMember(p => p.Id, opt => opt.Condition(p => p.Id is not null));

            CreateMap<BoxUpdateDto, BoxModel>();

            CreateMap<PalletModel, PalletResponseDto>()
                .ForMember(p => p.Boxes, opt => opt.Condition(p => p.Boxes.Count > 0));

            CreateMap<PalletCreateDto, PalletModel>()
                .ForMember(p => p.Id, opt => opt.Condition(p => p.Id is not null))
                .ForMember(p => p.ExpirationDate, opt => opt.Ignore());

            CreateMap<PalletUpdateDto, PalletModel>()
                .ForMember(p => p.ExpirationDate, opt => opt.Ignore());
        }
    }
}
