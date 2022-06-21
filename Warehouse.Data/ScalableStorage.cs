using Warehouse.Data.Models;
using Warehouse.EntityContext;
using Warehouse.EntityContext.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Warehouse.Data;

public class ScalableStorage : IScalableStorage
{
    private WarehouseContext db;

    public ScalableStorage(WarehouseContext injectedContext)
    {
        db = injectedContext;
        db.Database.EnsureCreated();
    }

    public async Task<List<Pallet>> GetAllPalletsAsync(int skip = 0, int count = 0)
    {
        var pallets = db.Pallets;
        if (pallets is null)
        {
            return new List<Pallet>();
        }

        var query = (IQueryable<PalletModel>) pallets.Include(p => p.Boxes);
        if (skip > 0)
        {
            query = query.Skip(skip);
        }
        if (count > 0)
        {
            query = query.Take(count);
        }

        return await Task.FromResult(
            query
            .Select(palletModel => palletModel.ToPallet())
            .ToList());
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
    public async Task<Pallet?> AddPalletAsync(Pallet pallet)
    {
        if (db.Pallets is null)
        {
            return null;
        }

        PalletModel model = pallet.ToPalletModel();
        await db.Pallets.AddAsync(model);

        int affected = await db.SaveChangesAsync();

        if (affected > 0)
        {
            return model.ToPallet();
        }

        return null;
    }
    public async Task<Pallet?> UpdatePalletAsync(Pallet pallet)
    {
        var pallets = db.Pallets;
        if (pallets is null)
        {
            return null;
        }

        var storedPallet = await pallets.FindAsync(pallet.Id);
        if (storedPallet is null)
        {
            storedPallet = pallet.ToPalletModel();
            await pallets.AddAsync(storedPallet);
        }
        else
        {
            EntityContext.Models.ModelExtensions.GetMapperInstance().Map<Pallet, PalletModel>(pallet, storedPallet);
        }

        int affected = await db.SaveChangesAsync();

        if (affected > 0)
        {
            return storedPallet.ToPallet();
        }
        return null;
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

        int affetcted = await db.SaveChangesAsync();
        return affetcted == 1;
    }
    public async Task<bool> DeletePalletAsync(int id)
    {
        var pallets = db.Pallets;
        if (pallets is null)
        {
            return false;
        }
        var pallet = await pallets.FindAsync(id);
        if (pallet is null)
        {
            return false;
        }

        var storedPallet = await pallets
            .Include(p => p.Boxes)
            .Where(b => b.Id == pallet.Id)
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

    public async Task<List<Box>> GetAllBoxesAsync()
    {
        var boxes = db.Boxes;
        if (boxes is null)
        {
            return new List<Box>();
        }
        return await Task.FromResult(boxes
            .Select(boxModel => boxModel.ToBox())
            .ToList());
    }
    public async Task<Box?> GetBoxAsync(int id)
    {
        var boxes = db.Boxes;
        if (boxes is null)
        {
            return null;
        }

        var storedBox = await boxes.FindAsync(id);

        if (storedBox is null)
        {
            return null;
        }
        return storedBox.ToBox();
    }
    public async Task<Box?> AddBoxAsync(Box box)
    {
        if (db.Boxes is null)
        {
            return null;
        }

        BoxModel model = box.ToBoxModel();
        await db.Boxes.AddAsync(model);

        int affected = await db.SaveChangesAsync();
        if (affected > 0)
        {
            return model.ToBox();
        }
        return null;
    }
    public async Task<Box?> UpdateBoxAsync(Box box)
    {
        var boxes = db.Boxes;
        if (boxes is null)
        {
            return null;
        }

        var storedBox = await boxes.FindAsync(box.Id);
        if (storedBox is null)
        {
            storedBox = box.ToBoxModel();
            await boxes.AddAsync(storedBox);
        }
        else
        {
            EntityContext.Models.ModelExtensions.GetMapperInstance().Map<Box, BoxModel>(box, storedBox);
        }

        int affected = await db.SaveChangesAsync();
        if (affected > 0)
        {
            return storedBox.ToBox();
        }
        return null;
    }
    public async Task<bool> DeleteBoxAsync(Box box)
    {
        db.Boxes?.Remove(box.ToBoxModel());
        int affected = await db.SaveChangesAsync();
        return affected > 0;
    }
    public async Task<bool> DeleteBoxAsync(int id)
    {
        var boxes = db.Boxes;
        if (boxes is null)
        {
            return true;
        }
        BoxModel boxModel = await boxes.FindAsync(id);

        if (boxModel is null)
        {
            return true;
        }
        boxes.Remove(boxModel);

        int affected = await db.SaveChangesAsync();
        return (affected > 0);
    }

    public async Task<bool> AddBoxToPalletAsync(Box box, Pallet pallet)
    {
        if (db.Boxes is null)
        {
            return false;
        }

        BoxModel? boxModel = await db.Boxes.FindAsync(box.Id);
        if (boxModel is null)
        {
            boxModel = box.ToBoxModel();
            await db.Boxes.AddAsync(boxModel);
        }

        boxModel.PalletModelId = pallet.Id;

        int affected = await db.SaveChangesAsync();

        return affected > 0;
    }
    public async Task<bool> RemoveBoxFromPallet(Box box)
    {
        if (db.Boxes is null)
        {
            return false;
        }
        BoxModel? boxModel = await db.Boxes.FindAsync(box.Id);

        if (boxModel == null)
        {
            return true;
        }
        boxModel.PalletModelId = 0;

        int affected = await db.SaveChangesAsync();
        return affected > 0;
    }
    
}
