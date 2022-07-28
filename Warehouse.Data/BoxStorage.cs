using Warehouse.Data.Models;
using Warehouse.EntityContext;
using Warehouse.EntityContext.Models;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using AutoMapper.QueryableExtensions;

namespace Warehouse.Data;

public class BoxStorage : IBoxStorage
{
    private readonly IWarehouseContext db;
    private readonly IMapper mapper;

    public BoxStorage(IWarehouseContext context, IMapper mapper)
    {
        db = context;
        this.mapper = mapper;
    }

    public async Task<IReadOnlyList<BoxModel>> GetAllBoxesAsync(int take, int? skip)
    {
        var query = (IQueryable<BoxEntity>)db.Boxes;

        if (skip is not null)
        {
            query = query.Skip(skip ?? 0);
        }

        query = query.Take(take);

        return await
            query
            .ProjectTo<BoxModel>(mapper.ConfigurationProvider)
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

    public async Task<bool> DeleteBoxAsync(int id)
    {
        var boxes = db.Boxes;
        BoxEntity? box = await boxes.FirstAsync(b => b.Id == id);

        if (box is null)
        {
            return false;
        }

        boxes.Remove(box);

        var affected = await db.SaveChangesAsync();
        return affected > 0;
    }

}
