using Warehouse.Data.Models;
using Warehouse.EntityContext;
using Warehouse.EntityContext.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using AutoMapper;
using AutoMapper.QueryableExtensions;

namespace Warehouse.Data;

public class ScalableStorage : IScalableStorage
{
    private readonly WarehouseContext db;
    private readonly IMapper mapper;

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
    /// <param name="take">Get N elements</param>
    /// <returns>List of pallets</returns>
    public async Task<List<Pallet>> GetAllPalletsAsync(int take, int? skip)
    {
        var query = (IQueryable<PalletModel>) db.Pallets.Include(p => p.Boxes);

        if (skip is not null)
        {
            query = query.Skip(skip ?? 0);
        }

        query = query.Take(take);
        
        return await
            query
            //.ProjectTo<Pallet>()
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
        var storedPallet = await db.Pallets
            .Include(p => p.Boxes)
            .FirstOrDefaultAsync(p => p.Id == id);

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
        PalletModel model = mapper.Map<PalletModel>(pallet);
        await db.Pallets.AddAsync(model);

        var affected = await db.SaveChangesAsync();
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

        var affected = await db.SaveChangesAsync();
        if (affected > 0)
        {
            return mapper.Map<Pallet>(storedPallet);
        }
        return null;
    }

    public async Task<bool> DeletePalletAsync(Pallet pallet)
    {
        var pallets = db.Pallets;

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

        var affetcted = await db.SaveChangesAsync();
        return affetcted == 1;
    }

    public async Task<bool> DeletePalletAsync(int id)
    {
        var pallets = db.Pallets;

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

    public async Task<List<Box>> GetAllBoxesAsync(int take, int? skip)
    {
        var query = (IQueryable<BoxModel>) db.Boxes;

        if (skip is not null)
        {
            query = query.Skip(skip ?? 0);
        }

        query = query.Take(take);
        
        return await 
            query
            .ProjectTo<Box>(mapper.ConfigurationProvider)
            //.Select(boxModel => mapper.Map<Box>(boxModel))
            .ToListAsync();
    }

    public async Task<Box?> GetBoxAsync(int id)
    {
        var storedBox = await db.Boxes.FindAsync(id);

        if (storedBox is null)
        {
            return null;
        }
        return mapper.Map<Box>(storedBox);
    }

    public async Task<Box?> AddBoxAsync(Box box)
    {
        BoxModel model = mapper.Map<BoxModel>(box);

        await db.Boxes.AddAsync(model);

        var affected = await db.SaveChangesAsync();
        if (affected > 0)
        {
            return mapper.Map<Box>(model);
        }
        return null;
    }

    public async Task<Box?> UpdateBoxAsync(Box box)
    {
        var boxes = db.Boxes;

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

        var affected = await db.SaveChangesAsync();
        if (affected > 0)
        {
            return mapper.Map<Box>(storedBox);
        }
        return null;
    }

    public async Task<bool> DeleteBoxAsync(Box box)
    {
        db.Boxes.Remove(mapper.Map<BoxModel>(box));

        var affected = await db.SaveChangesAsync();
        return affected > 0;
    }
    public async Task<bool> DeleteBoxAsync(int id)
    {
        var boxes = db.Boxes;
        BoxModel? boxModel = await boxes.FindAsync(id);

        if (boxModel is null)
        {
            return true;
        }
        boxes.Remove(boxModel);

        var affected = await db.SaveChangesAsync();
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

        var affected = await db.SaveChangesAsync();
        return affected > 0;
    }

    public async Task<bool> RemoveBoxFromPallet(Box box)
    {
        BoxModel? boxModel = await db.Boxes.FindAsync(box.Id);

        if (boxModel == null)
        {
            return true;
        }

        boxModel.PalletModelId = 0;

        var affected = await db.SaveChangesAsync();
        return affected > 0;
    }
    
}
