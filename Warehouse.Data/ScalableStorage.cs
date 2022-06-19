using Warehouse.Data.Models;
using Warehouse.EntityContext;
using Warehouse.EntityContext.Models;
using Microsoft.EntityFrameworkCore;

namespace Warehouse.Data;

public class ScalableStorage
{
    private WarehouseContext db;

    public ScalableStorage(WarehouseContext injectedContext)
    {
        db = injectedContext;
        db.Database.EnsureCreated();
    }

    public async void AddPalletAsync(Pallet pallet)
    {
        if (db.Pallets is null)
        {
            return;
        }

        await db.Pallets.AddAsync(pallet.ToPalletModel());

        await db.SaveChangesAsync();
    }

    public async void UpdatePalletAsync(Pallet pallet)
    {
        var pallets = db.Pallets;
        if (pallets is null)
        {
            return;
        }
        var storedPallet = await pallets.FindAsync(pallet.Id);
        if (storedPallet is null)
        {
            await pallets.AddAsync(pallet.ToPalletModel());
        }
        else
        {
            // TODO: use automapper
            storedPallet.Length = pallet.Length;
            storedPallet.Width= pallet.Width;
            storedPallet.Height = pallet.Height;
        }

        await db.SaveChangesAsync();
    }

    public async Task<bool> DeletePalletAsync(Pallet pallet)
    {
        var pallets = db.Pallets;
        if (pallets is null)
        {
            return false;
        }
        var storedPallet = await pallets
            .Include(p => p.Boxes)
            .Where(p => p.Id == pallet.Id)
            .FirstOrDefaultAsync();

        if (storedPallet is null)
        {
            return false;
        }

        storedPallet.Boxes?.Select(box => { box.PalletModelId = 0; return true; });

        pallets.Remove(storedPallet);

        await db.SaveChangesAsync();

        return true;
    }

    public async Task<List<Pallet>> GetAllPalletsAsync()
    {
        var pallets = db.Pallets;
        if (pallets is null)
        {
            return new List<Pallet>();
        }
        return await Task.FromResult(pallets.Include(p => p.Boxes).Select(palletModel => palletModel.ToPallet()).ToList());
    }

    public async Task<Pallet?> GetPalletAsync(int id)
    {
        var pallets = db.Pallets;
        if (pallets is null)
        {
            return null;
        }

        var storedPallet = await pallets.Include(p => p.Boxes).FirstOrDefaultAsync(p => p.Id == id);
        if (storedPallet is null)
        {
            return null;
        }
        return storedPallet.ToPallet();
    }

    public async void AddBoxToPalletAsync(Box box, Pallet pallet)
    {
        if (db.Boxes is null)
        {
            return;
        }

        BoxModel? boxModel = await db.Boxes.FindAsync(box.Id);
        if (boxModel is null)
        {
            boxModel = box.ToBoxModel();
            await db.Boxes.AddAsync(boxModel);
        }

        boxModel.PalletModelId = pallet.Id;

        await db.SaveChangesAsync();
    }

    public async void RemoveBoxFromPallet(Box box)
    {
        if (db.Boxes is null)
        {
            return;
        }
        BoxModel? boxModel = await db.Boxes.FindAsync(box.Id);

        if (boxModel == null)
        {
            return;
        }
        boxModel.PalletModelId = 0;

        await db.SaveChangesAsync();
            
    }

    public async void AddBoxAsync(Box box)
    {
        if (db.Boxes is null)
        {
            return;
        }

        await db.Boxes.AddAsync(box.ToBoxModel());

        await db.SaveChangesAsync();
    }

    
    public async void DeleteBoxAsync(Box box)
    {
        db.Boxes?.Remove(box.ToBoxModel());
        await db.SaveChangesAsync();
    }
    public async void DeleteBoxAsync(int id)
    {
        var boxModel = db.Boxes?.Find(id);
        if (boxModel is null)
        {
            return;
        }
        db.Boxes?.Remove(boxModel);
        await db.SaveChangesAsync();
    }
 
}
