using AutoMapper;
using Warehouse.Data.Models;

namespace Warehouse.EntityContext.Models
{
    public class ModelMappingProfile : Profile
    {
        public ModelMappingProfile()
        {
            CreateMap<Box, BoxModel>();
            CreateMap<BoxModel, Box>();

            CreateMap<Pallet, PalletModel>();
            CreateMap<PalletModel, Pallet>();
        }  
    }
}
