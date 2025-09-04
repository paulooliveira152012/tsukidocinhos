using Microsoft.EntityFrameworkCore;
using BrigadeiroApp.Models;

namespace BrigadeiroApp.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<BrigadeiroType> BrigadeiroTypes => Set<BrigadeiroType>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        base.OnModelCreating(b);

        // Order 1..N OrderItem
        b.Entity<OrderItem>(e =>
        {
            e.HasOne(i => i.Order)
             .WithMany(o => o.Items)
             .HasForeignKey(i => i.OrderId)
             .IsRequired()
             .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(i => i.BrigadeiroType)
             .WithMany()
             .HasForeignKey(i => i.BrigadeiroTypeId)
             .IsRequired()
             .OnDelete(DeleteBehavior.Restrict);
        });

        // Seed
        b.Entity<BrigadeiroType>().HasData(
            new BrigadeiroType { Id=1, Name="Tradicional", UnitPrice=2.50m, UnitCost=1.00m },
            new BrigadeiroType { Id=2, Name="Beijinho",   UnitPrice=2.50m, UnitCost=1.10m },
            new BrigadeiroType { Id=3, Name="Ninho",      UnitPrice=3.00m, UnitCost=1.30m },
            new BrigadeiroType { Id=4, Name="Nutella",    UnitPrice=3.50m, UnitCost=1.80m }
        );
    }
}
