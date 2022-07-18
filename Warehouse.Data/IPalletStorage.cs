using Warehouse.Data.Models;

namespace Warehouse.Data;

public interface IPalletStorage
{
    Task<IReadOnlyList<PalletModel>> GetAllPalletsAsync(int take, int? skip);
    Task<PalletModel?> GetPalletAsync(int id);
    Task<PalletModel?> AddPalletAsync(PalletModel pallet);
    Task<PalletModel?> UpdatePalletAsync(PalletModel pallet);
    Task<bool> DeletePalletAsync(PalletModel pallet);
    Task<bool> DeletePalletAsync(int id);
}
