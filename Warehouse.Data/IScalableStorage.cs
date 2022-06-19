using Warehouse.Data.Models;

namespace Warehouse.Data
{
    public interface IScalableStorage
    {
        void AddBoxAsync(Box box);
        void AddBoxToPalletAsync(Box box, Pallet pallet);
        void AddPalletAsync(Pallet pallet);
        void DeleteBoxAsync(Box box);
        void DeleteBoxAsync(int id);
        Task<bool> DeletePalletAsync(Pallet pallet);
        Task<List<Pallet>> GetAllPalletsAsync();
        Task<Pallet?> GetPalletAsync(int id);
        void RemoveBoxFromPallet(Box box);
        void UpdatePalletAsync(Pallet pallet);
    }
}