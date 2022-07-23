using Microsoft.EntityFrameworkCore;
using Warehouse.EntityContext.Sqlite.Configurations;
using Warehouse.EntityContext.Models;

namespace Warehouse.EntityContext.Sqlite;

public class WarehouseSqliteContext : DbContext, IWarehouseContext
{
    private const string DefaultFileName = "../Warehouse.db";
    private readonly string fileName;

    public WarehouseSqliteContext()
        : base()
    {
        fileName = DefaultFileName;
    }
    public WarehouseSqliteContext(string fileName)
        : base()
    {
        this.fileName = fileName;
    }
    public WarehouseSqliteContext(DbContextOptions<WarehouseSqliteContext> options) : base(options)
    {
        fileName = DefaultFileName;
    }
    public WarehouseSqliteContext(string fileName, DbContextOptions<WarehouseSqliteContext> options) : base(options)
    {
        this.fileName = fileName;
    }

    public virtual DbSet<BoxEntity> Boxes => Set<BoxEntity>();
    public virtual DbSet<PalletEntity> Pallets => Set<PalletEntity>();

    public async Task<int> SaveChangesAsync()
    {
        return await base.SaveChangesAsync();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseSqlite($"Filename={fileName}");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new PalletConfiguration());
        modelBuilder.ApplyConfiguration(new BoxConfiguration());
    }

}
