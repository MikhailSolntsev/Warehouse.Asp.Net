using Warehouse.Data.Models;
using AutoMapper;

namespace Warehouse.EntityContext.Models
{
    public static class ModelExtensions
    {
        private static IMapper? mapper;

        private static IMapper InitMapper()
        {
            if (mapper is null)
            {
                var configuration = new MapperConfiguration(cfg =>
                    {
                        cfg.CreateMap<Box, BoxModel>();
                        cfg.CreateMap<BoxModel, Box>();

                        cfg.CreateMap<ICollection<BoxModel>, IReadOnlyList<Box>>()
                            .ConvertUsing(collection => collection.Select(model => model.ToBox()).ToList());

                        cfg.CreateMap<IReadOnlyList<Box>, ICollection<BoxModel>>()
                            .ConvertUsing(list => list.Select(box => box.ToBoxModel()).ToList());

                        cfg.CreateMap<Pallet, PalletModel>();
                        cfg.CreateMap<PalletModel, Pallet>()
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

        public static BoxModel ToBoxModel(this Box box) =>
            InitMapper().Map<BoxModel>(box);

        public static Box ToBox(this BoxModel boxModel) =>
            InitMapper().Map<Box>(boxModel);

        public static PalletModel ToPalletModel(this Pallet pallet) =>
            InitMapper().Map<PalletModel>(pallet);

        public static Pallet ToPallet(this PalletModel palletModel) =>
            InitMapper().Map<Pallet>(palletModel);
    }
}
