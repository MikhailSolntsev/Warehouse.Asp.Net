using Warehouse.Data.Models;

namespace Warehouse.EntityContext.Models
{
    public static class ModelExtensions
    {
        public static BoxModel ToBoxModel(this Box box)
        {
            return new BoxModel()
            {
                Id = box.Id,
                Length = box.Length,
                Width = box.Width,
                Height = box.Height,
                Weight = box.Weight,
                ExpirationDate = box.ExpirationDate
            };
        }
        public static Box ToBox(this BoxModel boxModel)
        {
            return new Box(
                boxModel.Length ?? 0,
                boxModel.Height ?? 0,
                boxModel.Width ?? 0,
                boxModel.Weight ?? 0,
                boxModel.ExpirationDate,
                boxModel.Id ?? 0);
        }
        public static PalletModel ToPalletModel(this Pallet pallet)
        {
            return new PalletModel();
        }
        public static Pallet ToPallet(this PalletModel palletModel)
        {
            return new Pallet(
                palletModel.Length ?? 0,
                palletModel.Height ?? 0,
                palletModel.Width ?? 0,
                palletModel.Id ?? 0);
        }
    }
}
