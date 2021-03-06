using System.Text.Json.Serialization;

namespace Warehouse.Web.Models;

public class PalletDto
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public int? Id { get; set; }
    public int Length { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IList<BoxDto>? Boxes { get; set; }
}
