using Warehouse.Common;
using FluentAssertions; 

namespace WarehouseTests
{
    public class BoxTests
    {
        [Fact(DisplayName = "Box should calculate ExpirationDate from production date")]
        public void ExpirationDateCalculatesFromProductionDate()
        {
            DateTime productionDate = DateTime.Today;
            DateTime expirationDate = productionDate.AddDays(100);

            Box box = new Box(3, 5, 7, 11, productionDate: productionDate);

            box.ExpirationDate.Should().Be(expirationDate, "Calculated expiration date should be 100 days after production");
        }
        
        [Fact(DisplayName = "Box should accept expiration date instead of production date")]
        public void ExpirationDateHasMorePriorityThanProduction()
        {
            DateTime productionDate = DateTime.Today;
            DateTime expirationDate = DateTime.Today;

            Box box = new Box(3, 5, 7, 11, expirationDate, productionDate);

            box.ExpirationDate.Should().Be(expirationDate, "Expiration date should has more priority");
        }

        [Fact(DisplayName = "Creating box without dates should throw exception")]
        public void CreatingWithoutDatesShoulThrowException()
        {
            Action action = () => new Box(3, 5, 7, 11);
            action.Should().Throw<ArgumentNullException>("Creating box without dates is prohibited");
        }

        [Fact(DisplayName = "Box should calculate Volume from dimensions")]
        public void VolumeCalculatesFromDimensions()
        {
            Box box = new Box(3, 5, 7, 11, DateTime.Today);

            box.Volume.Should().Be(3 * 5 * 7, "Box Volume should be calculated from dimensions");
        }
    }
}