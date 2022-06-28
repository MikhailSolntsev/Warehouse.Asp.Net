using Microsoft.EntityFrameworkCore;
using Warehouse.EntityContext.Models;
using Warehouse.EntityContext.Sqlite;
using FluentAssertions;

namespace Warehouse.EntityContext.Tests
{
    public class DataContextTests
    {
        [Fact(DisplayName = "Database must create itself")]
        public void DatabaseCanBeInitialized()
        {
            // Arrange
            string fileName = Path.GetRandomFileName();

            using (WarehouseSqliteContext db = new(fileName))
            {
                // Act
                bool created = db.Database.EnsureCreated();

                // Assert
                created.Should().BeTrue("Database must create intself");
            }
            DatabaseHelper.DeleteDatabase(fileName);
        }

        [Fact(DisplayName = "Can add Box in database")]
        public void CanAddBox()
        {
            string fileName = Path.GetRandomFileName();
            using (WarehouseSqliteContext db = new(fileName))
            {
                db.Database.EnsureCreated();
                BoxModel model = new()
                {
                    Id = 1,
                    Length = 3,
                    Width = 5,
                    Height = 7,
                    Weight = 11,
                    ExpirationDate = DateTime.Today
                };
                db.Boxes?.Add(model);

                // Act
                db.SaveChanges();

                // Assert
                db.Boxes?.ToArray().Length.Should().Be(1, "After adding new 1 item, count should be 1");
            }

            // Clear
            DatabaseHelper.DeleteDatabase(fileName);
        }

        [Fact(DisplayName = "Can't add box without Required property")]
        public void CantAddBoxWithoutProperty()
        {
            // Arrange
            string fileName = Path.GetRandomFileName();
            using (WarehouseSqliteContext db = new(fileName))
            {
                db.Database.EnsureCreated();
                BoxModel model = new()
                {
                    Id = 1,
                    Length = 3,
                    Width = 5,
                    Height = 7,
                    ExpirationDate = DateTime.Today
                };
                db.Boxes?.Add(model);

                // Act
                Action action = () => db.SaveChanges();

                // Assert
                action.Should().Throw<Exception>("Can't add box without Required property");
            }
            // Clear
            DatabaseHelper.DeleteDatabase(fileName);
        }

        [Fact(DisplayName = "Can read boxes with pallets")]
        public void CanReadBoxesWithPallets()
        {
            // Arrange
            string fileName = Path.GetRandomFileName();
            using (WarehouseSqliteContext db = new(fileName))
            {
                db.Database.EnsureCreated();

                PalletModel pallet = new()
                {
                    Id = 13,
                    Length = 3,
                    Width = 5,
                    Height = 7
                };
                db.Pallets?.Add(pallet);

                BoxModel box = new()
                {
                    Id = 17,
                    Length = 3,
                    Width = 5,
                    Height = 7,
                    Weight = 13,
                    ExpirationDate = DateTime.Today
                    ,PalletModelId = 13
                };
                db.Boxes?.Add(box);

                // Act
                db.SaveChanges();

                // Assert
                db.Pallets.Should().NotBeNullOrEmpty("Pallets set should not be empty");
                db.Boxes.Should().NotBeNullOrEmpty("Boxes set should not be empty");

                db.Pallets?
                    .Include(pallet => pallet.Boxes)?
                    .FirstOrDefault()?
                    .Boxes
                    .Should()
                    .NotBeNullOrEmpty("Boxes set in pallet should not be empty");
            }

            // Clear
            DatabaseHelper.DeleteDatabase(fileName);
        }
    }
}
