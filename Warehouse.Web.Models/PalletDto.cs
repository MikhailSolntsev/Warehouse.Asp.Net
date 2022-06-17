
namespace Warehouse.Web.Models;

public class PalletDto
{
    public int Id { get; set; }
    public int Length { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }

    public List<BoxDto> Boxes { get; set; }
}
