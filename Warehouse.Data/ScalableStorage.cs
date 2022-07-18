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
    private readonly IWarehouseContext db;
    private readonly IMapper mapper;

    /// <summary>
    /// .ctor
    /// </summary>
    /// <param name="context">Database context</param>
    /// <param name="mapper">Automapper</param>
    public ScalableStorage(IWarehouseContext context, IMapper mapper)
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
    public async Task<List<PalletModel>> GetAllPalletsAsync(int take, int? skip)
    {
        var query = (IQueryable<PalletEntity>) db.Pallets;

        if (skip is not null)
        {
            query = query.Skip(skip ?? 0);
        }

        query = query.Take(take).Include(p => p.Boxes);
        
        return await
            query
            //.ProjectTo<Pallet>(mapper.ConfigurationProvider)
            .Select(palletModel => mapper.Map<PalletModel>(palletModel))
            .ToListAsync();
    }

    /// <summary>
    /// Gets one pallen async
    /// </summary>
    /// <param name="id">pallet Id</param>
    /// <returns>founded pallet</returns>
    public async Task<PalletModel?> GetPalletAsync(int id)
    {
        var storedPallet = await db.Pallets
            .Include(p => p.Boxes)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (storedPallet is null)
        {
            return null;
        }
        return mapper.Map<PalletModel>(storedPallet);
    }

    /// <summary>
    /// Adding one pallet with his boxes
    /// </summary>
    /// <param name="pallet">Pallet needs to add</param>
    /// <returns></returns>
    public async Task<PalletModel?> AddPalletAsync(PalletModel pallet)
    {
        PalletEntity model = mapper.Map<PalletEntity>(pallet);
        await db.Pallets.AddAsync(model);

        var affected = await db.SaveChangesAsync();
        if (affected > 0)
        {
            return mapper.Map<PalletModel>(model);
        }
        return null;
    }

    /// <summary>
    /// Updates pallet information
    /// </summary>
    /// <param name="pallet">Pallet with new information</param>
    /// <returns>New version of pallet</returns>
    public async Task<PalletModel?> UpdatePalletAsync(PalletModel pallet)
    {
        var pallets = db.Pallets;
        
        var storedPallet = await pallets.FindAsync(pallet.Id);
        if (storedPallet is null)
        {
            storedPallet = mapper.Map<PalletEntity>(pallet);
            await pallets.AddAsync(storedPallet);
        }
        else
        {
            mapper.Map<PalletModel, PalletEntity>(pallet, storedPallet);
        }

        var affected = await db.SaveChangesAsync();
        if (affected > 0)
        {
            return mapper.Map<PalletModel>(storedPallet);
        }
        return null;
    }

    public async Task<bool> DeletePalletAsync(PalletModel pallet)
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

    public async Task<List<BoxModel>> GetAllBoxesAsync(int take, int? skip)
    {
        var query = (IQueryable<BoxEntity>) db.Boxes;

        if (skip is not null)
        {
            query = query.Skip(skip ?? 0);
        }

        query = query.Take(take);
        
        return await 
            query
            .ProjectTo<BoxModel>(mapper.ConfigurationProvider)
            //.Select(boxModel => mapper.Map<Box>(boxModel))
            .ToListAsync();
    }

    public async Task<BoxModel?> GetBoxAsync(int id)
    {
        var storedBox = await db.Boxes.FindAsync(id);

        if (storedBox is null)
        {
            return null;
        }
        return mapper.Map<BoxModel>(storedBox);
    }

    public async Task<BoxModel?> AddBoxAsync(BoxModel box)
    {
        BoxEntity model = mapper.Map<BoxEntity>(box);

        await db.Boxes.AddAsync(model);

        var affected = await db.SaveChangesAsync();
        if (affected > 0)
        {
            return mapper.Map<BoxModel>(model);
        }
        return null;
    }

    public async Task<BoxModel?> UpdateBoxAsync(BoxModel box)
    {
        var boxes = db.Boxes;

        var storedBox = await boxes.FindAsync(box.Id);
        if (storedBox is null)
        {
            storedBox = mapper.Map<BoxEntity>(box);
            await boxes.AddAsync(storedBox);
        }
        else
        {
            mapper.Map<BoxModel, BoxEntity>(box, storedBox);
        }

        var affected = await db.SaveChangesAsync();
        if (affected > 0)
        {
            return mapper.Map<BoxModel>(storedBox);
        }
        return null;
    }

    public async Task<bool> DeleteBoxAsync(BoxModel box)
    {
        db.Boxes.Remove(mapper.Map<BoxEntity>(box));

        var affected = await db.SaveChangesAsync();
        return affected > 0;
    }
    public async Task<bool> DeleteBoxAsync(int id)
    {
        var boxes = db.Boxes;
        BoxEntity? boxModel = await boxes.FindAsync(id);

        if (boxModel is null)
        {
            return true;
        }
        boxes.Remove(boxModel);

        var affected = await db.SaveChangesAsync();
        return (affected > 0);
    }

    public async Task<bool> AddBoxToPalletAsync(BoxModel box, PalletModel pallet)
    {
        if (db.Boxes is null)
        {
            return false;
        }

        BoxEntity? boxModel = await db.Boxes.FindAsync(box.Id);
        if (boxModel is null)
        {
            boxModel = mapper.Map<BoxEntity>(box);
            await db.Boxes.AddAsync(boxModel);
        }

        boxModel.PalletModelId = pallet.Id;

        var affected = await db.SaveChangesAsync();
        return affected > 0;
    }

    public async Task<bool> RemoveBoxFromPallet(BoxModel box)
    {
        BoxEntity? boxModel = await db.Boxes.FindAsync(box.Id);

        if (boxModel == null)
        {
            return true;
        }

        boxModel.PalletModelId = 0;

        var affected = await db.SaveChangesAsync();
        return affected > 0;
    }
    
}
