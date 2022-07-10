using Warehouse.Data.Models;
using Warehouse.EntityContext;
using Warehouse.EntityContext.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using AutoMapper;

namespace Warehouse.Data;

public class ScalableStorage : IScalableStorage
{
    private WarehouseContext db;
    private IMapper mapper;

    /// <summary>
    /// .ctor
    /// </summary>
    /// <param name="context">Database context</param>
    /// <param name="mapper">Automapper</param>
    public ScalableStorage(WarehouseContext context, IMapper mapper)
    {
        db = context;
        db.Database.EnsureCreated();

        this.mapper = mapper;
    }

    /// <summary>
    /// Get all pallets with pagination
    /// </summary>
    /// <param name="skip">Skip N elements</param>
    /// <param name="count">Get N elements</param>
    /// <returns>List of pallets</returns>
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
        
        return await
            query
            .Select(palletModel => mapper.Map<Pallet>(palletModel))
            .ToListAsync();
    }

    /// <summary>
    /// Gets one pallen async
    /// </summary>
    /// <param name="id">pallet Id</param>
    /// <returns>founded pallet</returns>
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
        return mapper.Map<Pallet>(storedPallet);
    }

    /// <summary>
    /// Adding one pallet with his boxes
    /// </summary>
    /// <param name="pallet">Pallet needs to add</param>
    /// <returns></returns>
    public async Task<Pallet?> AddPalletAsync(Pallet pallet)
    {
        if (db.Pallets is null)
        {
            return null;
        }

        PalletModel model = mapper.Map<PalletModel>(pallet);
        await db.Pallets.AddAsync(model);

        int affected = await db.SaveChangesAsync();

        if (affected > 0)
        {
            return mapper.Map<Pallet>(model);
        }

        return null;
    }

    /// <summary>
    /// Updates pallet information
    /// </summary>
    /// <param name="pallet">Pallet with new information</param>
    /// <returns>New version of pallet</returns>
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
            storedPallet = mapper.Map<PalletModel>(pallet);
            await pallets.AddAsync(storedPallet);
        }
        else
        {
            mapper.Map<Pallet, PalletModel>(pallet, storedPallet);
        }

        int affected = await db.SaveChangesAsync();

        if (affected > 0)
        {
            return mapper.Map<Pallet>(storedPallet);
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

    public async Task<List<Box>> GetAllBoxesAsync(int skip = 0, int count = 0)
    {
        var boxes = db.Boxes;
        if (boxes is null)
        {
            return new List<Box>();
        }

        var query = (IQueryable<BoxModel>)boxes;
        if (skip > 0)
        {
            query = query.Skip(skip);
        }
        if (count > 0)
        {
            query = query.Take(count);
        }

        return await 
            query
            .Select(boxModel => mapper.Map<Box>(boxModel))
            .ToListAsync();
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
        return mapper.Map<Box>(storedBox);
    }
    public async Task<Box?> AddBoxAsync(Box box)
    {
        if (db.Boxes is null)
        {
            return null;
        }

        BoxModel model = mapper.Map<BoxModel>(box);
        await db.Boxes.AddAsync(model);

        int affected = await db.SaveChangesAsync();
        if (affected > 0)
        {
            return mapper.Map<Box>(model);
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
            storedBox = mapper.Map<BoxModel>(box);
            await boxes.AddAsync(storedBox);
        }
        else
        {
            mapper.Map<Box, BoxModel>(box, storedBox);
        }

        int affected = await db.SaveChangesAsync();
        if (affected > 0)
        {
            return mapper.Map<Box>(storedBox);
        }
        return null;
    }
    public async Task<bool> DeleteBoxAsync(Box box)
    {
        db.Boxes?.Remove(mapper.Map<BoxModel>(box));
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
            boxModel = mapper.Map<BoxModel>(box);
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
