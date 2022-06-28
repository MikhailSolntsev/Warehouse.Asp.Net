using Warehouse.Data.Models;

namespace Warehouse.Data
{
    public interface IScalableStorage
    {
        Task<List<Pallet>> GetAllPalletsAsync(int skip = 0, int cout = 0);
        Task<Pallet?> GetPalletAsync(int id);
        Task<Pallet?> AddPalletAsync(Pallet pallet);
        Task<Pallet?> UpdatePalletAsync(Pallet pallet);
        Task<bool> DeletePalletAsync(Pallet pallet);
        Task<bool> DeletePalletAsync(int id);

        Task<List<Box>> GetAllBoxesAsync(int skip = 0, int count = 0);
        Task<Box?> GetBoxAsync(int id);
        Task<Box?> AddBoxAsync(Box box);
        Task<Box?> UpdateBoxAsync(Box box);
        Task<bool> DeleteBoxAsync(Box box);
        Task<bool> DeleteBoxAsync(int id);

        Task<bool> AddBoxToPalletAsync(Box box, Pallet pallet);
        Task<bool> RemoveBoxFromPallet(Box box);
        
    }
}
