
namespace Warehouse.Data.Tests
{
    public class BoxStorageTests : IClassFixture<BoxStorageFixture>
    {
        private readonly IBoxStorage storage;
        private readonly BoxStorageFixture fixture;
        private readonly CancellationToken token = CancellationToken.None;

        public BoxStorageTests(BoxStorageFixture fixture)
        {
            this.fixture = fixture;
            storage = fixture.Storage;
        }

        [Fact(DisplayName = "Storage can retrieve boxes with SKIP pagination")]
        public async Task CanRetrieveBoxWithSkipPagination()
        {
            // Arrange
            
            // Act
            var boxes = await storage.GetAllBoxesAsync(take: 100, skip: 2, token);

            // Assert
            boxes.Should().HaveCountLessThan(4);
        }

        [Fact(DisplayName = "Storage can retrieve boxes with TAKE pagination")]
        public async Task CanRetrieveBpxWithTakePagination()
        {
            // Arrange
            
            // Act
            var boxes = await storage.GetAllBoxesAsync(take: 3, 0, token);

            // Assert
            boxes.Should().HaveCount(3);
        }

        [Fact(DisplayName = "Can't delete box with wrong Id")]
        public async Task CantDeleteBoxWithWrongId()
        {
            // Arrange
            
            // Act
            Func<Task> action = async () => await storage.DeleteBoxAsync(BoxStorageFixture.WrongId, token);

            // Assert
            await action.Should().ThrowAsync<Exception>();
        }

        [Fact(DisplayName = "Storage can delete boxes with good Id")]
        public async Task CanDeleteBoxWithGoodId()
        {
            // Arrange

            // Act
            Func<Task> action = async () => await storage.DeleteBoxAsync(BoxStorageFixture.GoodId, token);

            // Assert
            await action.Should().NotThrowAsync<Exception>();
        }

        [Fact(DisplayName = "Find box should not drop exception with wrong Id")]
        public async Task FindBoxShouldNotDropExceptionWithWrongId()
        {
            // Arrange

            // Act
            Func<Task> action = async () => await storage.GetBoxAsync(BoxStorageFixture.WrongId, token);

            // Assert
            await action.Should().NotThrowAsync<Exception>();
        }

    }
}
