
namespace Warehouse.Data.Models;

public class Pallet : Scalable
{
    private const int OwnWeigth = 30;
    private List<Box> boxes = new();

    public IReadOnlyList<Box> Boxes { get => boxes; }

    public override int Weight { get => OwnWeigth + boxes.Sum(box => box.Weight);  }
    public override int Volume { get => base.Volume + boxes.Sum(box => box.Volume); }
    public override DateTime ExpirationDate { get => boxes.Count switch { 0 => DateTime.MinValue, _ => boxes.Min(box => box.ExpirationDate) }; }

    public Pallet(int length, int height, int width) : base(length, height, width)
    {
        
    }
    public Pallet(int length, int height, int width, int id) : base(length, height, width)
    {
        Id = id;
    }

    public void AddBox(Box box)
    {
        if (box.Length > Length || box.Height > Height || box.Width > Width)
        {
            throw new ArgumentOutOfRangeException("Box dimensions grater than pallet");
        }

        boxes.Add(box);
    }
}
