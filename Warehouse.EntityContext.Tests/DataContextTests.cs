using Microsoft.EntityFrameworkCore;
using Warehouse.EntityContext.Entities;
using FluentAssertions;
using Xunit.Abstractions;

namespace Warehouse.EntityContext.Tests;

[Collection("Data context collection")]
public class DataContextTests
{
    private readonly IWarehouseContext db;

    public DataContextTests(DataContextFixture fixture)
    {
        db = fixture.Context;
    }

    [Fact(DisplayName = "Can add Box in database")]
    public async void CanAddBox()
    {
        BoxEntity entity = new()
        {
            Length = 3,
            Width = 5,
            Height = 7,
            Weight = 11,
            ExpirationDate = DateTime.Today
        };
        await db.Boxes.AddAsync(entity);

        // Act
        await db.SaveChangesAsync();

        // Assert
        entity.Id.Should().BeGreaterThan(0);
        BoxEntity found = await db.Boxes.FirstAsync(b => b.Id == entity.Id);
        found.Should().NotBeNull().And.BeEquivalentTo(new { Length = 3, Width = 5, Height = 7, Weight = 11 });
    }

    [Fact(DisplayName = "Can't add box without Required property")]
    public async void CantAddBoxWithoutProperty()
    {
        // Arrange
        BoxEntity model = new()
        {
            Length = 3,
            Width = 5,
            Height = 7,
            Weight= 11
        };
        await db.Boxes.AddAsync(model);

        // Act
        Action action = () => db.SaveChangesAsync();

        // Assert
        action.Should().NotThrow<Exception>("Can't add box without Required property");
    }

    [Fact(DisplayName = "Can read boxes with pallets")]
    public async void CanReadBoxesWithPallets()
    {
        // Arrange

        PalletEntity pallet = new()
        {
            Id = 13,
            Length = 3,
            Width = 5,
            Height = 7
        };
        db.Pallets.Add(pallet);

        BoxEntity box = new()
        {
            Id = 17,
            Length = 3,
            Width = 5,
            Height = 7,
            Weight = 13,
            ExpirationDate = DateTime.Today
            ,PalletModelId = 13
        };
        db.Boxes.Add(box);

        // Act
        await db.SaveChangesAsync();

        // Assert
        db.Pallets.Should().NotBeNullOrEmpty("Pallets set should not be empty");
        db.Boxes.Should().NotBeNullOrEmpty("Boxes set should not be empty");

        db.Pallets
            .Include(pallet => pallet.Boxes)?
            .FirstOrDefault()
            .Boxes
            .Should()
            .NotBeNullOrEmpty("Boxes set in pallet should not be empty");
    }

}
