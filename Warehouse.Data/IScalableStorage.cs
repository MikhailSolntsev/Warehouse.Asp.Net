using Warehouse.Data.Models;

namespace Warehouse.Data
{
    public interface IScalableStorage
    {
        Task<bool> AddBoxToPalletAsync(BoxModel box, PalletModel pallet);
        Task<bool> RemoveBoxFromPallet(BoxModel box);
        
    }
}
