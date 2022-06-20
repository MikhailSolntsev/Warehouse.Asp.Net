using System.Text.Json;
using System.Text.Json.Serialization;

namespace Warehouse.Web.Models;

public class PalletDto
{
    public int Id { get; set; }
    public int Length { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public List<BoxDto>? Boxes { get; set; }
}
