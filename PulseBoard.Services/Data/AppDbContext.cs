using Microsoft.EntityFrameworkCore;
using PulseBoard.Services.Entities;

namespace PulseBoard.Services.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    // DbSets
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<Deal> Deals => Set<Deal>();
    public DbSet<Target> Targets => Set<Target>();


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ---------- Customer ----------
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.ToTable("Customers");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(200);

            entity.Property(x => x.Segment)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(x => x.ChurnRiskScore)
                .IsRequired();

            entity.Property(x => x.CreatedAt)
                .IsRequired();

            // Enums default to int in EF Core (fine for SQLite).
            // If you want string storage instead, uncomment:
            // entity.Property(x => x.Region).HasConversion<string>();
        });

        // ---------- Order ----------
        modelBuilder.Entity<Order>(entity =>
        {
            entity.ToTable("Orders");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Amount)
                .IsRequired()
                .HasPrecision(18, 2);

            entity.Property(x => x.OrderDate)
                .IsRequired();

            entity.Property(x => x.ProductLine)
                .IsRequired()
                .HasMaxLength(100);

            entity.HasOne(o => o.Customer)
            .WithMany(c => c.Orders)
            .HasForeignKey(o => o.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);


            entity.HasIndex(x => x.OrderDate);
            entity.HasIndex(x => x.CustomerId);
        });

        // ---------- Deal ----------
        modelBuilder.Entity<Deal>(entity =>
        {
            entity.ToTable("Deals");

            entity.HasKey(x => x.Id);

            entity.Property(x => x.Amount)
                .IsRequired()
                .HasPrecision(18, 2);

            entity.Property(x => x.Stage)
                .IsRequired()
                .HasMaxLength(50);

            entity.Property(x => x.ExpectedCloseDate)
                .IsRequired();

            entity.Property(x => x.WinProbability)
                .IsRequired()
                .HasPrecision(5, 4); // e.g. 0.7500

            entity.Property(x => x.RiskScore)
                .IsRequired();

            entity.HasOne(d => d.Customer)
            .WithMany(c => c.Deals)
            .HasForeignKey(d => d.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);


            entity.HasIndex(x => x.ExpectedCloseDate);
            entity.HasIndex(x => x.CustomerId);
        });

        modelBuilder.Entity<Target>(entity =>
 {
     entity.ToTable("Targets");

     entity.HasKey(t => t.Id);

     entity.Property(t => t.TargetRevenue)
         .HasPrecision(18, 2)
         .IsRequired();

     entity.Property(t => t.ProductLine)
         .HasMaxLength(100);
 });

    }
}
