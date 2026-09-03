using Day21_Inventory_System.Models;

namespace Day21_Inventory_System.Data;

public static class DbSeeder
{
  public static void Seed(AppDbContext context)
  {
    // Seed Categories
    if (!context.Categories.Any())
    {
      var categories = new List<Category>
            {
                new Category { Name = "Electronics" },
                new Category { Name = "Accessories" },
                new Category { Name = "Furniture" }
            };

      context.Categories.AddRange(categories);
      context.SaveChanges();
    }

    // Seed Products
    if (!context.Products.Any())
    {
      var categories = context.Categories.ToList();

      var electronics = categories.First(c => c.Name == "Electronics");
      var accessories = categories.First(c => c.Name == "Accessories");
      var furniture = categories.First(c => c.Name == "Furniture");

      var products = new List<Product>
            {
                new Product
                {
                    Name = "Laptop",
                    Price = 75000,
                    CategoryId = electronics.Id
                },

                new Product
                {
                    Name = "Smartphone",
                    Price = 35000,
                    CategoryId = electronics.Id
                },

                new Product
                {
                    Name = "Keyboard",
                    Price = 2500,
                    CategoryId = accessories.Id
                },

                new Product
                {
                    Name = "Mouse",
                    Price = 1200,
                    CategoryId = accessories.Id
                },

                new Product
                {
                    Name = "Office Chair",
                    Price = 8500,
                    CategoryId = furniture.Id
                }
            };

      context.Products.AddRange(products);
      context.SaveChanges();
    }

    // Seed Inventory
    if (!context.Inventory.Any())
    {
      var products = context.Products.ToList();

      var inventory = products.Select(product => new Inventory
      {
        ProductId = product.Id,
        Quantity = 10
      }).ToList();

      context.Inventory.AddRange(inventory);
      context.SaveChanges();
    }
  }
}