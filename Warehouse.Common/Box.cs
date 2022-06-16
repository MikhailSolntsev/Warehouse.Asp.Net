namespace Warehouse.Data.Models;

public class Box : Scalable
{
    private const int ExpirationPeriod = 100;
    static private int nextId;

    public Box(int length, int height, int width, int weight, DateTime expirationDate, int id) : base(length, height, width, weight)
    {
        ExpirationDate = expirationDate;
        Id = id;
        nextId = Math.Max(id, nextId) + 1;
    }

    public Box(int length, int height, int width, int weight, DateTime? expirationDate = null, DateTime? productionDate = null) : base(length, height, width, weight)
    {
        if (expirationDate is null && productionDate is null)
        {
            throw new ArgumentNullException("Expiration date or production date should not be null");
        }

        if (expirationDate is null)
        {
            ExpirationDate = productionDate.GetValueOrDefault() + TimeSpan.FromDays(ExpirationPeriod);
        }
        else
        {
            ExpirationDate = expirationDate ?? DateTime.MinValue;
        };

        Id = Interlocked.Increment(ref nextId);
    }
    
}
