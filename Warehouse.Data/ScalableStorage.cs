using Warehouse.Data.Models;
using Warehouse.EntityContext;
using Warehouse.EntityContext.Models;
using Microsoft.EntityFrameworkCore;
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

        this.mapper = mapper;
    }

    public async Task<bool> AddBoxToPalletAsync(BoxModel box, PalletModel pallet)
    {
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
