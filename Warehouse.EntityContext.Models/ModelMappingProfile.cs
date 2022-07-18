using AutoMapper;
using Warehouse.Data.Models;

namespace Warehouse.EntityContext.Models
{
    public class ModelMappingProfile : Profile
    {
        public ModelMappingProfile()
        {
            CreateMap<Box, BoxModel>()
                .ForMember(b => b.PalletModelId, opt => opt.Ignore())
                .ForMember(b => b.PalletModel, opt => opt.Ignore());
            CreateMap<BoxModel, Box>();

            CreateMap<Pallet, PalletModel>();
            CreateMap<PalletModel, Pallet>()
                .ForMember(p => p.ExpirationDate, opt => opt.Ignore());
        }  
    }
}
