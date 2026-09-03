using System.Text.Json.Serialization;

namespace Day21_Inventory_System.Models;

public class Product
{
  public int Id { get; set; }

  public string Name { get; set; } = string.Empty;

  public decimal Price { get; set; }

  public int CategoryId { get; set; }

  public Category? Category { get; set; }

  [JsonIgnore]
  public ICollection<Purchase> Purchases { get; set; } = new List<Purchase>();

  [JsonIgnore]
  public ICollection<Sale> Sales { get; set; } = new List<Sale>();

  [JsonIgnore]
  public Inventory? Inventory { get; set; }
}