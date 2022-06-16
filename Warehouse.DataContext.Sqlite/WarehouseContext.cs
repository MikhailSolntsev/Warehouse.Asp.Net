using Microsoft.EntityFrameworkCore;
using Warehouse.EntityContext.Models;

namespace Warehouse.EntityContext.Sqlite;

public partial class WarehouseContext : DbContext
{
    private string fileName;

    public WarehouseContext(string fileName)
        : base()
    {
        this.fileName = fileName;
    }

    public WarehouseContext(string fileName, DbContextOptions<WarehouseContext> options)
        : base(options)
    {
        this.fileName = fileName;
    }

    public virtual DbSet<BoxModel>? Boxes { get; set; }
    public virtual DbSet<PalletModel>? Pallets { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlite($"Filename={fileName}");
        }
    }

}
