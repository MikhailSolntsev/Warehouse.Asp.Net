using Microsoft.EntityFrameworkCore;
using Warehouse.EntityContext.Sqlite.Configurations;
using Warehouse.EntityContext.Entities;

namespace Warehouse.EntityContext.Sqlite;

public class WarehouseSqliteContext : DbContext, IWarehouseContext
{
    public const string DefaultFileName = "../Warehouse.db";

    private readonly string fileName;

    public WarehouseSqliteContext() : base()
    {
        fileName = DefaultFileName;
    }
     public WarehouseSqliteContext(string fileName) : base()
    {
        this.fileName = fileName;
    }
    public WarehouseSqliteContext(string fileName, DbContextOptions<WarehouseSqliteContext> options) : base(options)
    {
        this.fileName = fileName;
    }
    public WarehouseSqliteContext(DbContextOptions<WarehouseSqliteContext> options) : base(options)
    {
        fileName = "";
    }

    public virtual DbSet<BoxEntity> Boxes => Set<BoxEntity>();
    public virtual DbSet<PalletEntity> Pallets => Set<PalletEntity>();

    public async Task<int> SaveChangesAsync()
    {
        return await base.SaveChangesAsync();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured && !string.IsNullOrEmpty(fileName))
        {
            optionsBuilder.UseSqlite($"Filename={fileName}");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(PalletConfiguration).Assembly);
    }

}
