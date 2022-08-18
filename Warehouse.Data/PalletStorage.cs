using Warehouse.Data.Models;
using Warehouse.EntityContext;
using Warehouse.EntityContext.Entities;
using Microsoft.EntityFrameworkCore;
using AutoMapper;

namespace Warehouse.Data;

public class PalletStorage : IPalletStorage
{
    private readonly IWarehouseContext db;
    private readonly IMapper mapper;

    /// <summary>
    /// .ctor
    /// </summary>
    /// <param name="context">Database context</param>
    /// <param name="mapper">Automapper</param>
    public PalletStorage(IWarehouseContext context, IMapper mapper)
    {
        db = context;

        this.mapper = mapper;
    }

    /// <summary>
    /// Get all pallets with pagination
    /// </summary>
    /// <param name="skip">Skip N elements</param>
    /// <param name="take">Get N elements</param>
    /// <returns>List of pallets</returns>
    public async Task<IReadOnlyList<PalletModel>> GetAllPalletsAsync(int take, int? skip, CancellationToken token)
    {
        var query = (IQueryable<PalletEntity>)db.Pallets;

        if (skip is not null)
        {
            query = query.Skip(skip ?? 0);
        }

        query = query.Take(take).Include(p => p.Boxes);

        return await
            query
            //.ProjectTo<Pallet>(mapper.ConfigurationProvider)
            .Select(palletModel => mapper.Map<PalletModel>(palletModel))
            .ToListAsync(token);
    }

    /// <summary>
    /// Gets one pallen async
    /// </summary>
    /// <param name="id">pallet Id</param>
    /// <returns>founded pallet</returns>
    public async Task<PalletModel?> GetPalletAsync(int id, CancellationToken token)
    {
        var storedPallet = await db.Pallets
            .Include(p => p.Boxes)
            .FirstOrDefaultAsync(p => p.Id == id, token);

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
    public async Task<PalletModel?> AddPalletAsync(PalletModel pallet, CancellationToken token)
    {
        PalletEntity model = mapper.Map<PalletEntity>(pallet);
        await db.Pallets.AddAsync(model, token);

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
    public async Task<PalletModel?> UpdatePalletAsync(PalletModel pallet, CancellationToken token)
    {
        var pallets = db.Pallets;

        var storedPallet = await pallets.FirstOrDefaultAsync(p => p.Id == pallet.Id, token);
        if (storedPallet is null)
        {
            throw new ArgumentException($"No such pallet wit id {pallet.Id}");
        }

        mapper.Map<PalletModel, PalletEntity>(pallet, storedPallet);
        
        var affected = await db.SaveChangesAsync();
        if (affected > 0)
        {
            return mapper.Map<PalletModel>(storedPallet);
        }
        return null;
    }

    /// <summary>
    /// Deletes pallet with clearing pallet in boxes
    /// </summary>
    /// <param name="id">Pallet Id</param>
    /// <returns></returns>
    public async Task<bool> DeletePalletAsync(int id, CancellationToken token)
    {
        var pallets = db.Pallets;

        var storedPallet = await pallets
            .Include(p => p.Boxes)
            //.Where(b => b.Id == pallet.Id)
            .FirstOrDefaultAsync(p => p.Id == id, token);

        if (storedPallet is null)
        {
            return false;
        }

        storedPallet.Boxes?.Select(box => { box.PalletModelId = 0; return true; });

        pallets.Remove(storedPallet);

        var affected = await db.SaveChangesAsync();
        return affected > 0;
    }

    /// <summary>
    /// Add box in pallet with size validation
    /// </summary>
    /// <param name="box"></param>
    /// <param name="pallet"></param>
    /// <returns></returns>
    public async Task<bool> AddBoxToPalletAsync(BoxModel box, PalletModel pallet, CancellationToken token)
    {
        BoxEntity? boxModel = await db.Boxes.FindAsync(box.Id, token);
        if (boxModel is null)
        {
            boxModel = mapper.Map<BoxEntity>(box);
            await db.Boxes.AddAsync(boxModel, token);
        }

        boxModel.PalletModelId = pallet.Id;

        var affected = await db.SaveChangesAsync();
        return affected > 0;
    }

    /// <summary>
    /// Removes box from pallet
    /// </summary>
    /// <param name="box"></param>
    /// <returns></returns>
    public async Task<bool> RemoveBoxFromPallet(BoxModel box, CancellationToken token)
    {
        BoxEntity? boxModel = await db.Boxes.FindAsync(box.Id, token);

        if (boxModel == null)
        {
            return true;
        }

        boxModel.PalletModelId = 0;

        var affected = await db.SaveChangesAsync();
        return affected > 0;
    }


}
