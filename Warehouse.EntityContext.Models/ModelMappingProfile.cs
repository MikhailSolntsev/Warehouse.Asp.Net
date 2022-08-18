using AutoMapper;
using Warehouse.Data.Models;

namespace Warehouse.EntityContext.Entities
{
    public class ModelMappingProfile : Profile
    {
        public ModelMappingProfile()
        {
            CreateMap<BoxModel, BoxEntity>()
                .ForMember(b => b.PalletModelId, opt => opt.Ignore())
                .ForMember(b => b.PalletModel, opt => opt.Ignore());
            CreateMap<BoxEntity, BoxModel>();

            CreateMap<PalletModel, PalletEntity>();
            CreateMap<PalletEntity, PalletModel>()
                .ForMember(p => p.ExpirationDate, opt => opt.Ignore());
        }  
    }
}
