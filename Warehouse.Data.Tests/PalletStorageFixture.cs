using AutoMapper;
using Warehouse.Data.Infrastructure;

namespace Warehouse.Data.Tests;

public class PalletStorageFixture : IAsyncLifetime
{
    public IPalletStorage Storage { get; private set; }
    public static int GoodId => 41;
    public static int ModifyId => 43;
    public static int SpareId => 11;
    public static int WrongId => 13;

    public PalletStorageFixture()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(typeof(EntityMappingProfile));
        });

        IMapper mapper = config.CreateMapper();

        Storage = new PalletStorage(DataContextFactory.GetContext(), mapper);
    }
    public async Task InitializeAsync() => await FillStorageWithPalletAndBoxesAsync();
    public Task DisposeAsync() => Task.CompletedTask;

    private async Task FillStorageWithPalletAndBoxesAsync()
    {
        var token = CancellationToken.None;

        PalletModel pallet;

        pallet = new(3, 5, 7, GoodId);
        await Storage.AddPalletAsync(pallet, token);

        pallet = new(3, 5, 7, ModifyId);
        await Storage.AddPalletAsync(pallet, token);

        pallet = new(3, 5, 7, 47);
        await Storage.AddPalletAsync(pallet, token);

        pallet = new(3, 5, 7, 53);
        await Storage.AddPalletAsync(pallet, token);

        pallet = new(3, 5, 7, 59);
        await Storage.AddPalletAsync(pallet, token);
    }

}
