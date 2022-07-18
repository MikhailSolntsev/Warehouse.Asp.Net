using Warehouse.Data.Models;

namespace Warehouse.Data
{
    public interface IScalableStorage
    {
        Task<List<PalletModel>> GetAllPalletsAsync(int take, int? skip);
        Task<PalletModel?> GetPalletAsync(int id);
        Task<PalletModel?> AddPalletAsync(PalletModel pallet);
        Task<PalletModel?> UpdatePalletAsync(PalletModel pallet);
        Task<bool> DeletePalletAsync(PalletModel pallet);
        Task<bool> DeletePalletAsync(int id);

        Task<List<BoxModel>> GetAllBoxesAsync(int take, int? skip);
        Task<BoxModel?> GetBoxAsync(int id);
        Task<BoxModel?> AddBoxAsync(BoxModel box);
        Task<BoxModel?> UpdateBoxAsync(BoxModel box);
        Task<bool> DeleteBoxAsync(BoxModel box);
        Task<bool> DeleteBoxAsync(int id);

        Task<bool> AddBoxToPalletAsync(BoxModel box, PalletModel pallet);
        Task<bool> RemoveBoxFromPallet(BoxModel box);
        
    }
}
