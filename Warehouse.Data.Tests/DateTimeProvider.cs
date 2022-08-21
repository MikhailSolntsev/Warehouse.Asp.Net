namespace Warehouse.Data.Tests;

public class DateTimeProvider : IDateTimeProvider
{
    private readonly DateTime today = DateTime.Today;
    public DateTime Today() => today;
}
