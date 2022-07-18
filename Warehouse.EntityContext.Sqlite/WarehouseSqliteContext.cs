using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
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
        DiscribePallet(modelBuilder);

        DiscribeBox(modelBuilder);
    }

    private void DiscribeBox(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<BoxEntity>()
            .ToTable("Boxes")
            .HasIndex("Id");

        modelBuilder.Entity<BoxEntity>()
            .HasKey(box => box.Id);

        modelBuilder.Entity<BoxEntity>()
            .Property(box => box.Length)
            .HasColumnType("INTEGER")
            .IsRequired();

        modelBuilder.Entity<BoxEntity>()
            .Property(box => box.Width)
            .HasColumnType("INTEGER")
            .IsRequired();

        modelBuilder.Entity<BoxEntity>()
            .Property(box => box.Height)
            .HasColumnType("INTEGER")
            .IsRequired();

        modelBuilder.Entity<BoxEntity>()
            .Property(box => box.Weight)
            .HasColumnType("INTEGER")
            .IsRequired();

        modelBuilder.Entity<BoxEntity>()
            .Property(box => box.ExpirationDate)
            .HasColumnType("datetime")
            .IsRequired();

        modelBuilder.Entity<BoxEntity>()
            .Property(box => box.PalletModelId)
            .HasColumnName("PalletId")
            .HasColumnType("INTEGER");

        modelBuilder.Entity<BoxEntity>()
            .HasOne(box => box.PalletModel)
            .WithMany(pallet => pallet.Boxes);
    }

    private void DiscribePallet(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PalletEntity>()
            .ToTable("Pallets")
            .HasIndex("Id");

        modelBuilder.Entity<PalletEntity>()
            .HasKey(pallet => pallet.Id);

        modelBuilder.Entity<PalletEntity>()
            .Property(pallet => pallet.Length)
            .HasColumnType("INTEGER")
            .IsRequired();

        modelBuilder.Entity<PalletEntity>()
            .Property(pallet => pallet.Width)
            .HasColumnType("INTEGER")
            .IsRequired();

        modelBuilder.Entity<PalletEntity>()
            .Property(pallet => pallet.Height)
            .HasColumnType("INTEGER")
            .IsRequired();

        modelBuilder.Entity<PalletEntity>()
            .HasMany(pallet => pallet.Boxes);
    }
}
