namespace Warehouse.Data.Models;

public class Box : Scalable
{
    private const int ExpirationPeriod = 100;

    public Box(int length, int height, int width, int weight, DateTime expirationDate, int? id) : base(length, height, width, weight)
    {
        ExpirationDate = expirationDate;
        Id = id;
    }
    public Box(int length, int height, int width, int weight, DateTime expirationDate) : base(length, height, width, weight)
    {
        ExpirationDate = expirationDate;
    }

    public Box(int length, int height, int width, int weight, DateTime? expirationDate = null, DateTime? productionDate = null) : base(length, height, width)
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
    }
    
}
