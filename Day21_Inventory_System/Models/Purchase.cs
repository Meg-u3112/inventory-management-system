using System.Text.Json.Serialization;

namespace Day21_Inventory_System.Models;

public class Purchase
{
  public int Id { get; set; }

  public int ProductId { get; set; }

  [JsonIgnore]
  public Product? Product { get; set; }

  public int Quantity { get; set; }

  public decimal PurchasePrice { get; set; }

  public DateTime PurchaseDate { get; set; }

  public string? Supplier { get; set; }
}