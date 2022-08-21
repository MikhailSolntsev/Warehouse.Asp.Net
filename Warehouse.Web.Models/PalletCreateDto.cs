using System.Text.Json.Serialization;

namespace Warehouse.Web.Dto;

public class PalletCreateDto
{
    public int? Id { get; set; }
    public int Length { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IList<BoxCreateDto>? Boxes { get; set; }
}
