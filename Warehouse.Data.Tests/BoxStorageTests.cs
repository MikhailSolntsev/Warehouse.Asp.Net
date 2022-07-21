using Warehouse.Data.Models;
using Warehouse.EntityContext;
using Warehouse.EntityContext.Models;
using Warehouse.EntityContext.Sqlite;
using FluentAssertions;
using AutoMapper;

namespace Warehouse.Data
{
    public class BoxStorageTests
    {
        private IBoxStorage storage;

        public BoxStorageTests()
        {
            var fileName = Path.GetRandomFileName();
            IWarehouseContext context = new WarehouseSqliteContext(fileName);
            context.Database.EnsureCreated();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(typeof(ModelMappingProfile));
            });

            IMapper mapper = config.CreateMapper();

            storage = new BoxStorage(context, mapper);
        }

        [Fact(DisplayName = "Storage can retrieve boxes with SKIP pagination")]
        public async Task CanRetrieveBoxWithSkipPagination()
        {
            // Arrange
            await FillStorageWithBoxesAsync();

            // Act
            var boxes = await storage.GetAllBoxesAsync(take: 100, skip: 2);

            // Assert
            boxes.Should().HaveCount(3);
        }

        [Fact(DisplayName = "Storage can retrieve boxes with TAKE pagination")]
        public async Task CanRetrieveBpxWithTakePagination()
        {
            // Arrange
            await FillStorageWithBoxesAsync();

            // Act
            var boxes = await storage.GetAllBoxesAsync(take: 2, null);

            // Assert
            boxes.Should().HaveCount(2);
        }

        [Fact(DisplayName = "Can't delete box with wrong Id")]
        public async Task CantDeleteBoxWithWrongId()
        {
            // Arrange
            BoxModel box = new(3, 5, 7, 11, DateTime.Today);
            await storage.AddBoxAsync(box);

            // Act
            Func<Task> action = async () => await storage.DeleteBoxAsync(15);

            // Assert
            await action.Should().ThrowAsync<Exception>();
        }

        [Fact(DisplayName = "Storage can delete boxes with good Id")]
        public async Task CanDeleteBoxWithGoodId()
        {
            // Arrange
            BoxModel box = new(3, 5, 7, 11, DateTime.Today);
            await storage.AddBoxAsync(box);

            // Act
            Func<Task> action = async () => await storage.DeleteBoxAsync(1);

            // Assert
            await action.Should().NotThrowAsync<Exception>();
        }

        [Fact(DisplayName = "Find box should not drop exception with wrong Id")]
        public async Task FindBoxShouldNotDropExceptionWithWrongId()
        {
            // Arrange
            BoxModel box = new(3, 5, 7, 11, DateTime.Today);
            await storage.AddBoxAsync(box);

            // Act
            Func<Task> action = async () => await storage.GetBoxAsync(15);

            // Assert
            await action.Should().NotThrowAsync<Exception>();
        }

        private async Task FillStorageWithBoxesAsync()
        {
            BoxModel box = new(3, 5, 7, 11, DateTime.Today);
            await storage.AddBoxAsync(box);
            box = new(3, 5, 7, 13, DateTime.Today);
            await storage.AddBoxAsync(box);
            box = new(3, 5, 7, 17, DateTime.Today);
            await storage.AddBoxAsync(box);
            box = new(3, 5, 7, 19, DateTime.Today);
            await storage.AddBoxAsync(box);
            box = new(3, 5, 7, 23, DateTime.Today);
            await storage.AddBoxAsync(box);
        }
    }
}
