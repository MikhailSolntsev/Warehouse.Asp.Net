
namespace Warehouse.Data.Models;

public class ScalableModel
{
    public int? Id { get; protected set; }
    public int Length { get; }
    public int Height { get; }
    public int Width { get; }
    public virtual int Weight { get; }
    public virtual int Volume { get => Length * Width * Height; }
    public virtual DateTime ExpirationDate { get; protected set; }

    public ScalableModel(int length, int height, int width, int weight)
    {
        Length = length;
        Height = height;
        Width = width;
        Weight = weight;
    }
    public ScalableModel(int length, int height, int width)
    {
        Length = length;
        Height = height;
        Width = width;
    }
    public override int GetHashCode() => Id ?? 0;
    public override bool Equals(object? obj) => obj is ScalableModel && ((ScalableModel)obj).Id == Id;
}
