using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Day21_Inventory_System.Data;
using Day21_Inventory_System.Models;

namespace Day21_Inventory_System.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SalesController : ControllerBase
{
  private readonly AppDbContext _context;

  public SalesController(AppDbContext context)
  {
    _context = context;
  }

  // GET: api/Sales
  [HttpGet]
  public async Task<ActionResult<IEnumerable<Sale>>> GetSales()
  {
    var sales = await _context.Sales
        .Include(s => s.Product)
        .ToListAsync();

    return Ok(sales);
  }
  // GET: api/Sales/{id}
  [HttpGet("{id}")]
  public async Task<ActionResult<Sale>> GetSale(int id)
  {
    var sale = await _context.Sales
        .Include(s => s.Product)
        .FirstOrDefaultAsync(s => s.Id == id);

    if (sale == null)
    {
      return NotFound();
    }

    return Ok(sale);
  }
  // POST: api/Sales
  [HttpPost]
  public async Task<ActionResult<Sale>> CreateSale(Sale sale)
  {
    if (sale.Quantity <= 0)
    {
      return BadRequest("Quantity must be greater than 0.");
    }

    if (sale.SalePrice <= 0)
    {
      return BadRequest("Sale price must be greater than 0.");
    }

    if (string.IsNullOrWhiteSpace(sale.Customer))
    {
      return BadRequest("Customer is required.");
    }
    // Check product exists
    var product = await _context.Products
        .FindAsync(sale.ProductId);

    if (product == null)
    {
      return BadRequest("Invalid ProductId.");
    }

    // Find inventory
    var inventory = await _context.Inventory
        .FirstOrDefaultAsync(i => i.ProductId == sale.ProductId);

    if (inventory == null)
    {
      return BadRequest("Inventory record not found.");
    }

    // Check available stock
    if (inventory.Quantity < sale.Quantity)
    {
      return BadRequest("Insufficient stock.");
    }

    // Add sale
    _context.Sales.Add(sale);

    // Reduce stock
    inventory.Quantity -= sale.Quantity;

    // Save changes
    await _context.SaveChangesAsync();

    return CreatedAtAction(
        nameof(GetSale),
        new { id = sale.Id },
        sale
    );
  }
  // PUT: api/Sales/{id}
  [HttpPut("{id}")]
  public async Task<IActionResult> UpdateSale(
      int id,
      Sale sale)
  {
    if (sale.Quantity <= 0)
    {
      return BadRequest("Quantity must be greater than 0.");
    }

    if (sale.SalePrice <= 0)
    {
      return BadRequest("Sale price must be greater than 0.");
    }

    if (string.IsNullOrWhiteSpace(sale.Customer))
    {
      return BadRequest("Customer is required.");
    }
    // URL ID and body ID should match
    if (id != sale.Id)
    {
      return BadRequest();
    }

    // Find existing sale
    var existingSale = await _context.Sales
        .FindAsync(id);

    if (existingSale == null)
    {
      return NotFound();
    }

    // Check product exists
    var productExists = await _context.Products
        .AnyAsync(p => p.Id == sale.ProductId);

    if (!productExists)
    {
      return BadRequest("Invalid ProductId.");
    }

    // Find old product inventory
    var oldInventory = await _context.Inventory
        .FirstOrDefaultAsync(i =>
            i.ProductId == existingSale.ProductId);

    if (oldInventory == null)
    {
      return BadRequest("Inventory record not found.");
    }

    // Product changed
    if (existingSale.ProductId != sale.ProductId)
    {
      // Return old sale quantity to old product stock
      oldInventory.Quantity += existingSale.Quantity;

      // Find new product inventory
      var newInventory = await _context.Inventory
          .FirstOrDefaultAsync(i =>
              i.ProductId == sale.ProductId);

      if (newInventory == null)
      {
        return BadRequest("New product inventory not found.");
      }

      // Check new product has enough stock
      if (newInventory.Quantity < sale.Quantity)
      {
        return BadRequest("Insufficient stock.");
      }

      // Remove new sale quantity
      newInventory.Quantity -= sale.Quantity;
    }
    else
    {
      // Same product
      var difference =
          sale.Quantity - existingSale.Quantity;

      // Sale increased → stock decreases
      // Sale decreased → stock increases
      oldInventory.Quantity -= difference;

      // Make sure stock doesn't become negative
      if (oldInventory.Quantity < 0)
      {
        return BadRequest("Insufficient stock.");
      }
    }

    // Update sale
    existingSale.ProductId = sale.ProductId;
    existingSale.Quantity = sale.Quantity;
    existingSale.SalePrice = sale.SalePrice;
    existingSale.SaleDate = sale.SaleDate;
    existingSale.Customer = sale.Customer;

    await _context.SaveChangesAsync();

    return NoContent();
  }
}