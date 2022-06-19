using Microsoft.EntityFrameworkCore;
using Warehouse.EntityContext.Models;

namespace Warehouse.EntityContext.Sqlite;

public partial class WarehouseSqliteContext : WarehouseContext
{
    private const string DefaultFileName = "../Warehouse.db";
    private string fileName;

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
    public WarehouseSqliteContext(DbContextOptions<WarehouseContext> options) : base(options)
    {
        fileName = DefaultFileName;
    }
    public WarehouseSqliteContext(string fileName, DbContextOptions<WarehouseContext> options) : base(options)
    {
        this.fileName = fileName;
    }

    public override DbSet<BoxModel>? Boxes { get; set; }
    public override DbSet<PalletModel>? Pallets { get; set; }

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
        modelBuilder.Entity<BoxModel>()
            .ToTable("Box")
            .HasIndex("Id");

        modelBuilder.Entity<BoxModel>()
            .HasKey(box => box.Id);

        modelBuilder.Entity<BoxModel>()
            .Property(box => box.Length)
            .HasColumnType("INTEGER")
            .IsRequired();

        modelBuilder.Entity<BoxModel>()
            .Property(box => box.Width)
            .HasColumnType("INTEGER")
            .IsRequired();

        modelBuilder.Entity<BoxModel>()
            .Property(box => box.Height)
            .HasColumnType("INTEGER")
            .IsRequired();

        modelBuilder.Entity<BoxModel>()
            .Property(box => box.Weight)
            .HasColumnType("INTEGER")
            .IsRequired();

        modelBuilder.Entity<BoxModel>()
            .Property(box => box.ExpirationDate)
            .HasColumnType("datetime")
            .IsRequired();

        modelBuilder.Entity<BoxModel>()
            .Property(box => box.PalletModelId)
            .HasColumnName("PalletId")
            .HasColumnType("INTEGER");

        modelBuilder.Entity<BoxModel>()
            .HasOne(box => box.PalletModel)
            .WithMany(pallet => pallet.Boxes);
    }

    private void DiscribePallet(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PalletModel>()
            .ToTable("Pallet")
            .HasIndex("Id");

        modelBuilder.Entity<PalletModel>()
            .HasKey(pallet => pallet.Id);

        modelBuilder.Entity<PalletModel>()
            .Property(pallet => pallet.Length)
            .HasColumnType("INTEGER")
            .IsRequired();

        modelBuilder.Entity<PalletModel>()
            .Property(pallet => pallet.Width)
            .HasColumnType("INTEGER")
            .IsRequired();

        modelBuilder.Entity<PalletModel>()
            .Property(pallet => pallet.Height)
            .HasColumnType("INTEGER")
            .IsRequired();

        modelBuilder.Entity<PalletModel>()
            .HasMany(pallet => pallet.Boxes);
    }
}
