using System.Text.Json.Serialization;

namespace Warehouse.Web.Models;

public class PalletUpdateDto
{
    public int Id { get; set; }
    public int Length { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IList<BoxUpdateDto>? Boxes { get; set; }
}
