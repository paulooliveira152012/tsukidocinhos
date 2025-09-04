using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using BrigadeiroApp.Models;

namespace BrigadeiroApp.Data;

public class AppDbContext : IdentityDbContext<ApplicationUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

    public DbSet<BrigadeiroType> BrigadeiroTypes => Set<BrigadeiroType>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        base.OnModelCreating(b); // importante: cria as tabelas de Identity

        b.Entity<OrderItem>(e =>
        {
            e.HasOne(i => i.Order)
             .WithMany(o => o.Items)
             .HasForeignKey(i => i.OrderId)
             .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(i => i.BrigadeiroType)
             .WithMany()
             .HasForeignKey(i => i.BrigadeiroTypeId)
             .OnDelete(DeleteBehavior.Restrict);
        });

        // (opcional) seed de tipos, se você usa
        // b.Entity<BrigadeiroType>().HasData(...);
    }
}
