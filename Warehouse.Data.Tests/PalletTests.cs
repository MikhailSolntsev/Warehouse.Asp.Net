
namespace Warehouse.Data.Tests;

public class PalletTests
{
    private readonly IDateTimeProvider dateTimeProvider = new DateTimeProvider();

    [Fact(DisplayName = "Box Length should not be greater than pallet")]
    public void CantAddBoxWithLargeLength()
    {
        PalletModel pallet = new PalletModel(3, 5, 7);
        BoxModel box = new(11, 3, 5, 0, dateTimeProvider.Today());

        Action action = () => pallet.AddBox(box);

        action.Should().Throw<Exception>("Adding box with greater Length should throw exception");
    }
    
    [Fact(DisplayName = "Box Height should not be greater than pallet")]
    public void CantAddBoxWithLargeHeight()
    {
        PalletModel pallet = new PalletModel(3, 5, 7);
        BoxModel box = new(3, 11, 5, 0, dateTimeProvider.Today());

        Action action = () => pallet.AddBox(box);

        action.Should().Throw<Exception>("Adding box with greater Height should throw exception");
    }
    
    [Fact(DisplayName = "Box Width should not be greater than pallet")]
    public void CantAddBoxWithLargeWidth()
    {
        PalletModel pallet = new PalletModel(3, 5, 7);
        BoxModel box = new(3, 5, 11, 0, dateTimeProvider.Today());

        Action action = () => pallet.AddBox(box);

        action.Should().Throw<Exception>("Adding box with greater Width should throw exception");
    }
    
    [Fact(DisplayName = "Pallet Weight should be calculated from own and boxes")]
    public void WeightMustBeCalculated()
    {
        PalletModel pallet = new(3, 5, 7);
        BoxModel box1 = new(3, 5, 7, 11, dateTimeProvider.Today());
        BoxModel box2 = new(3, 5, 7, 13, dateTimeProvider.Today());
        pallet.AddBox(box1);
        pallet.AddBox(box2);

        pallet.Weight.Should().Be(30 + 11 + 13, "Pallet Weight should be calculated from own(30) and boxes");
    }
    
    [Fact(DisplayName = "Pallet Volume should be calculated from own and boxes")]
    public void VolumeMustBeCalculated()
    {
        PalletModel pallet = new(3, 5, 7);
        BoxModel box1 = new(3, 5, 7, 11, dateTimeProvider.Today());
        BoxModel box2 = new(3, 5, 7, 13, dateTimeProvider.Today());
        pallet.AddBox(box1);
        pallet.AddBox(box2);

        pallet.Volume.Should().Be(3 * 5 * 7 * 3, "Pallet Volume should be calculated from own l*w*h and boxes");
    }


}
