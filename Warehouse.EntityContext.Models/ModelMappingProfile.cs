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

            //CreateMap<ICollection<BoxModel>, IReadOnlyList<Box>>()
            //    .ConvertUsing(collection => collection.Select(model => model.ToBox()).ToList());

            //CreateMap<IList<Box>, ICollection<BoxModel>>()
            //    .ConvertUsing(list => list.Select(box => box.Map Map<BoxModel>(box)).ToList());
            ;

            CreateMap<Pallet, PalletModel>();
            CreateMap<PalletModel, Pallet>()
                //.AfterMap((src, dst) => src.Boxes.ToList().ForEach(model => dst.AddBox(model.ToBox())))
                ;
        }  
    }
}
