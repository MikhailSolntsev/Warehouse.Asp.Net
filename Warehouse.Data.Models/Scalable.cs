
namespace Warehouse.Data.Models;

public class Scalable
{
    public int Id { get; protected set; }
    public int Length { get; }
    public int Height { get; }
    public int Width { get; }
    public virtual int Weight { get; }
    public virtual int Volume { get => Length * Width * Height; }
    public virtual DateTime ExpirationDate { get; protected set; }

    public Scalable(int length, int height, int width, int weight)
    {
        Length = length;
        Height = height;
        Width = width;
        Weight = weight;
    }
    public override int GetHashCode() => Id;
    public override bool Equals(object? obj) => obj is Scalable && ((Scalable)obj).Id == Id;
}
