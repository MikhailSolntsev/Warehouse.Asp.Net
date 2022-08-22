using AutoMapper;
using Warehouse.Data.Infrastructure;
using Warehouse.EntityContext;

namespace Warehouse.Data.Tests;

/// <summary>
/// Topic about fixture
/// https://xunit.net/docs/shared-context
///
/// Topic about IAsyncLifetime
/// https://www.danclarke.com/cleaner-tests-with-iasynclifetime
/// 
/// </summary>
public class BoxStorageFixture : IAsyncLifetime
{
    public IBoxStorage Storage { get; private set; }
    public static int GoodId => 41;
    public static int WrongId => 15;

    public BoxStorageFixture()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(typeof(EntityMappingProfile));
        });

        IMapper mapper = config.CreateMapper();

        Storage = new BoxStorage(DataContextFactory.GetContext(), mapper);
    }
    public async Task InitializeAsync() => await FillStorageWithBoxesAsync();
    public Task DisposeAsync() => Task.CompletedTask;

    private async Task FillStorageWithBoxesAsync()
    {
        CancellationToken token = CancellationToken.None;

        BoxModel box;

        box = new(3, 5, 7, 11, DateTime.Today, GoodId);
        await Storage.AddBoxAsync(box, token);

        box = new(3, 5, 7, 13, DateTime.Today, 43);
        await Storage.AddBoxAsync(box, token);

        box = new(3, 5, 7, 17, DateTime.Today, 47);
        await Storage.AddBoxAsync(box, token);

        box = new(3, 5, 7, 19, DateTime.Today, 53);
        await Storage.AddBoxAsync(box, token);

        box = new(3, 5, 7, 23, DateTime.Today, 59);
        await Storage.AddBoxAsync(box, token);
    }

}
