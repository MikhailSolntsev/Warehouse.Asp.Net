using Warehouse.Data.Models;
using Warehouse.EntityContext;
using Warehouse.EntityContext.Models;
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
    public async Task<IReadOnlyList<PalletModel>> GetAllPalletsAsync(int take, int? skip)
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

    public async Task<bool> DeletePalletAsync(int id)
    {
        var pallets = db.Pallets;

        var storedPallet = await pallets
            .Include(p => p.Boxes)
            //.Where(b => b.Id == pallet.Id)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (storedPallet is null)
        {
            return false;
        }

        storedPallet.Boxes?.Select(box => { box.PalletModelId = 0; return true; });

        pallets.Remove(storedPallet);

        var affected = await db.SaveChangesAsync();
        return affected > 0;
    }

}
