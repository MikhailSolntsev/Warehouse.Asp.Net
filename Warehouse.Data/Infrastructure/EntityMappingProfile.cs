using AutoMapper;
using Warehouse.Data.Models;
using Warehouse.EntityContext.Entities;

namespace Warehouse.Data.Infrastructure
{
    public class EntityMappingProfile : Profile
    {
        public EntityMappingProfile()
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
