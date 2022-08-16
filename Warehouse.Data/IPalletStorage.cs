using Warehouse.Data.Models;

namespace Warehouse.Data;

public interface IPalletStorage
{
    Task<IReadOnlyList<PalletModel>> GetAllPalletsAsync(int take, int? skip, CancellationToken token);
    Task<PalletModel?> GetPalletAsync(int id, CancellationToken token);
    Task<PalletModel?> AddPalletAsync(PalletModel pallet, CancellationToken token);
    Task<PalletModel?> UpdatePalletAsync(PalletModel pallet, CancellationToken token);
    Task<bool> DeletePalletAsync(int id, CancellationToken token);
    Task<bool> AddBoxToPalletAsync(BoxModel box, PalletModel pallet, CancellationToken token);
    Task<bool> RemoveBoxFromPallet(BoxModel box, CancellationToken token);
}
