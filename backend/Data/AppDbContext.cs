using Microsoft.EntityFrameworkCore;
using Day21_Inventory_System.Models;

namespace Day21_Inventory_System.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Category> Categories { get; set; }

    public DbSet<Product> Products { get; set; }

    public DbSet<Purchase> Purchases { get; set; }

    public DbSet<Sale> Sales { get; set; }

    public DbSet<Inventory> Inventory { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Category → Products (1 : Many)
        modelBuilder.Entity<Product>()
            .HasOne(p => p.Category)
            .WithMany(c => c.Products)
            .HasForeignKey(p => p.CategoryId)
            .OnDelete(DeleteBehavior.Restrict);

        // Product → Purchases (1 : Many)
        modelBuilder.Entity<Purchase>()
            .HasOne(p => p.Product)
            .WithMany(p => p.Purchases)
            .HasForeignKey(p => p.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        // Product → Sales (1 : Many)
        modelBuilder.Entity<Sale>()
            .HasOne(s => s.Product)
            .WithMany(p => p.Sales)
            .HasForeignKey(s => s.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        // Product → Inventory (1 : 1)
        modelBuilder.Entity<Inventory>()
            .HasOne(i => i.Product)
            .WithOne(p => p.Inventory)
            .HasForeignKey<Inventory>(i => i.ProductId)
            .OnDelete(DeleteBehavior.Cascade);

        // Product price
        modelBuilder.Entity<Product>()
            .Property(p => p.Price)
            .HasPrecision(18, 2);

        // Purchase price
        modelBuilder.Entity<Purchase>()
            .Property(p => p.PurchasePrice)
            .HasPrecision(18, 2);

        // Sale price
        modelBuilder.Entity<Sale>()
            .Property(s => s.SalePrice)
            .HasPrecision(18, 2);
    }
}