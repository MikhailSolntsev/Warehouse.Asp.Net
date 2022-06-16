using Warehouse.Data.Models;
using Warehouse.EntityContext;
using Warehouse.EntityContext.Models;
using Warehouse.EntityContext.Sqlite;

namespace Warehouse.Data;

public class ScalableStorage
{
    public IDbProvider DbProvider;

    public ScalableStorage(IDbProvider dbProvider)
    {
        DbProvider = dbProvider;
    }

    public async void AddPallet(Pallet pallet)
    {
        WarehouseContext? db = await DbProvider.GetWarehouseContextAsync() as WarehouseContext;
        if (db is null)
        {
            return;
        }

        if (db.Pallets is null)
        {
            return;
        }

        await db.Pallets.AddAsync(pallet.ToPalletModel());

        await db.SaveChangesAsync();
    }

    public async void AddBox(Box box)
    {
        WarehouseContext? db = await DbProvider.GetWarehouseContextAsync() as WarehouseContext;
        if (db is null)
        {
            return;
        }

        if (db.Boxes is null)
        {
            return;
        }

        await db.Boxes.AddAsync(box.ToBoxModel());

        await db.SaveChangesAsync();
    }

    public async void AddBoxToPallet(Box box, Pallet pallet)
    {
        WarehouseContext? db = await DbProvider.GetWarehouseContextAsync() as WarehouseContext;
        if (db is null)
        {
            return;
        }
        if (db.Boxes is null)
        {
            return;
        }

        BoxModel? boxModel = await db.Boxes.FindAsync(box.Id);
        if (boxModel is null)
        {
            throw new ArgumentException($"Can't find Box with Id = {box.Id}");
        }

        //boxModel.PalletId = pallet.Id;

        await db.SaveChangesAsync();
    }

}
