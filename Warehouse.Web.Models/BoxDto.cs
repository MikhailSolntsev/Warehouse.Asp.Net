
namespace Warehouse.Web.Models;

public class BoxDto
{
    public int Id { get; set; }
    public int Length { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public int Weight { get; set; }
    public DateTime ExpirationDate { get; set; }
}
