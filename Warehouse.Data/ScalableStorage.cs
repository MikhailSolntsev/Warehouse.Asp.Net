﻿using Warehouse.Data.Models;
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

    public async Task<List<Pallet>> GetAllPalletsAsync()
    {
        var pallets = db.Pallets;
        if (pallets is null)
        {
            return new List<Pallet>();
        }
        return await Task.FromResult(pallets
            .Include(p => p.Boxes)
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

        await db.Pallets.AddAsync(pallet.ToPalletModel());

        int affected = await db.SaveChangesAsync();

        if (affected == 1)
        {
            return pallet;
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
            await pallets.AddAsync(pallet.ToPalletModel());
        }
        else
        {
            EntityContext.Models.ModelExtensions.GetMapperInstance().Map<Pallet, PalletModel>(pallet, storedPallet);
        }

        int affected = await db.SaveChangesAsync();
        if (affected == 1)
        {
            return pallet;
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

        await db.Boxes.AddAsync(box.ToBoxModel());

        int affected = await db.SaveChangesAsync();
        if (affected == 1)
        {
            return box;
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
            await boxes.AddAsync(box.ToBoxModel());
        }
        else
        {
            EntityContext.Models.ModelExtensions.GetMapperInstance().Map<Box, BoxModel>(box, storedBox);
        }

        int affected = await db.SaveChangesAsync();
        if (affected == 1)
        {
            return box;
        }
        return null;
    }
    public async Task<bool> DeleteBoxAsync(Box box)
    {
        db.Boxes?.Remove(box.ToBoxModel());
        int affected = await db.SaveChangesAsync();
        return affected == 1;
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
        return (affected == 1);
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
        return affected == 1;
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
        return affected == 1;
    }
    
}
