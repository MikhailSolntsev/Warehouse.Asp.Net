using AutoFixture.Xunit2;

namespace Warehouse.Data.Tests;

public class BoxTests
{
    private readonly IDateTimeProvider dateTimeProvider = new DateTimeProvider();

    [Fact(DisplayName = "Box should calculate ExpirationDate from production date")]
    public void ExpirationDateCalculatesFromProductionDate()
    {
        DateTime productionDate = dateTimeProvider.Today();
        DateTime expirationDate = productionDate.AddDays(100);

        BoxModel box = new BoxModel(3, 5, 7, 11, productionDate: productionDate);

        box.ExpirationDate.Should().Be(expirationDate, "Calculated expiration date should be 100 days after production");
    }
    
    [Fact(DisplayName = "Box should accept expiration date instead of production date")]
    public void ExpirationDateHasMorePriorityThanProduction()
    {
        DateTime productionDate = dateTimeProvider.Today();
        DateTime expirationDate = dateTimeProvider.Today();

        BoxModel box = new BoxModel(3, 5, 7, 11, expirationDate, productionDate);

        box.ExpirationDate.Should().Be(expirationDate, "Expiration date should has more priority");
    }

    [Fact(DisplayName = "Creating box without dates should throw exception")]
    public void CreatingWithoutDatesShoulThrowException()
    {
        Action action = () => new BoxModel(3, 5, 7, 11);
        action.Should().Throw<ArgumentNullException>("Creating box without dates is prohibited");
    }

    [Theory(DisplayName = "Box should calculate Volume from dimensions"), AutoData]
    public void VolumeCalculatesFromDimensions(int length, int width, int height)
    {
        BoxModel box = new BoxModel(length, height, width, 11, dateTimeProvider.Today());

        box.Volume.Should().Be(length * height * width, "Box Volume should be calculated from dimensions");
    }
}
