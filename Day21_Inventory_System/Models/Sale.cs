namespace Day21_Inventory_System.Models;

public class Sale
{
  public int Id { get; set; }

  public int ProductId { get; set; }

  public Product? Product { get; set; }

  public int Quantity { get; set; }

  public decimal SalePrice { get; set; }

  public DateTime SaleDate { get; set; } = DateTime.UtcNow;
  public string? Customer { get; set; }

}