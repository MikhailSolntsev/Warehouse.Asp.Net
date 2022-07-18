using Warehouse.Data.Models;

namespace Warehouse.Data;

public interface IBoxStorage
{
    Task<IReadOnlyList<BoxModel>> GetAllBoxesAsync(int take, int? skip);
    Task<BoxModel?> GetBoxAsync(int id);
    Task<BoxModel?> AddBoxAsync(BoxModel box);
    Task<BoxModel?> UpdateBoxAsync(BoxModel box);
    Task<bool> DeleteBoxAsync(BoxModel box);
    Task<bool> DeleteBoxAsync(int id);
}
