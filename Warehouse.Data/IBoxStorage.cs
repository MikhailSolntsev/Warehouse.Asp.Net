using Warehouse.Data.Models;

namespace Warehouse.Data;

public interface IBoxStorage
{
    Task<IReadOnlyList<BoxModel>> GetAllBoxesAsync(int take, int skip, CancellationToken token);
    Task<BoxModel?> GetBoxAsync(int id, CancellationToken token);
    Task<BoxModel?> AddBoxAsync(BoxModel box, CancellationToken token);
    Task<BoxModel?> UpdateBoxAsync(BoxModel box, CancellationToken token);
    Task<bool> DeleteBoxAsync(int id, CancellationToken token);
}
