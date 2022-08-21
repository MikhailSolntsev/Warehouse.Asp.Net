using Warehouse.Data.Models;
using Warehouse.EntityContext;
using Warehouse.EntityContext.Entities;
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

    public async Task<IReadOnlyList<BoxModel>> GetAllBoxesAsync(int take, int skip, CancellationToken token)
    {
        var query = (IQueryable<BoxEntity>)db.Boxes;

        query = query.Skip(skip).Take(take);

        return await
            query
            .ProjectTo<BoxModel>(mapper.ConfigurationProvider)
            .ToListAsync(token);
    }

    public async Task<BoxModel?> GetBoxAsync(int id, CancellationToken token)
    {
        var storedBox = await db.Boxes.FirstOrDefaultAsync(b => b.Id == id, token);

        if (storedBox is null)
        {
            return null;
        }
        return mapper.Map<BoxModel>(storedBox);
    }

    public async Task<BoxModel?> AddBoxAsync(BoxModel box, CancellationToken token)
    {
        BoxEntity model = mapper.Map<BoxEntity>(box);

        await db.Boxes.AddAsync(model, token);

        var affected = await db.SaveChangesAsync();
        if (affected > 0)
        {
            return mapper.Map<BoxModel>(model);
        }
        return null;
    }

    public async Task<BoxModel?> UpdateBoxAsync(BoxModel box, CancellationToken token)
    {
        var boxes = db.Boxes;

        var storedBox = await boxes.FirstOrDefaultAsync(b => b.Id == box.Id, token);
        if (storedBox is null)
        {
            throw new ArgumentException($"No such box with id {box.Id}");
        }

        mapper.Map<BoxModel, BoxEntity>(box, storedBox);
        
        var affected = await db.SaveChangesAsync();
        if (affected > 0)
        {
            return mapper.Map<BoxModel>(storedBox);
        }
        return null;
    }

    public async Task<bool> DeleteBoxAsync(int id, CancellationToken token)
    {
        var boxes = db.Boxes;
        BoxEntity? box = await boxes.FirstAsync(b => b.Id == id, token);

        if (box is null)
        {
            return false;
        }

        boxes.Remove(box);

        var affected = await db.SaveChangesAsync();
        return affected > 0;
    }

}
