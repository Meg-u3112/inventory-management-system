using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Day21_Inventory_System.Data;
using Day21_Inventory_System.Models;

namespace Day21_Inventory_System.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PurchasesController : ControllerBase
{
  private readonly AppDbContext _context;

  public PurchasesController(AppDbContext context)
  {
    _context = context;
  }

  // GET: api/Purchases
  [HttpGet]
  public async Task<ActionResult<IEnumerable<Purchase>>> GetPurchases()
  {
    var purchases = await _context.Purchases
        .Include(p => p.Product)
        .ToListAsync();

    return Ok(purchases);
  }
  // GET: api/Purchases/{id}
  [HttpGet("{id}")]
  public async Task<ActionResult<Purchase>> GetPurchase(int id)
  {
    var purchase = await _context.Purchases
        .Include(p => p.Product)
        .FirstOrDefaultAsync(p => p.Id == id);

    if (purchase == null)
    {
      return NotFound();
    }

    return Ok(purchase);
  }
  // POST: api/Purchases
  [HttpPost]
  public async Task<ActionResult<Purchase>> CreatePurchase(Purchase purchase)
  {
    if (purchase.Quantity <= 0)
    {
      return BadRequest("Quantity must be greater than 0.");
    }

    if (purchase.PurchasePrice <= 0)
    {
      return BadRequest("Purchase price must be greater than 0.");
    }

    if (string.IsNullOrWhiteSpace(purchase.Supplier))
    {
      return BadRequest("Supplier is required.");
    }

    // Check product exists
    var product = await _context.Products
        .FindAsync(purchase.ProductId);

    if (product == null)
    {
      return BadRequest("Invalid ProductId.");
    }

    // Find inventory
    var inventory = await _context.Inventory
        .FirstOrDefaultAsync(i => i.ProductId == purchase.ProductId);

    if (inventory == null)
    {
      return BadRequest("Inventory record not found.");
    }

    _context.Purchases.Add(purchase);

    inventory.Quantity += purchase.Quantity;

    await _context.SaveChangesAsync();

    return CreatedAtAction(
        nameof(GetPurchase),
        new { id = purchase.Id },
        purchase
    );
  }
  // PUT: api/Purchases/{id}
  [HttpPut("{id}")]
  public async Task<IActionResult> UpdatePurchase(
      int id,
      Purchase purchase)
  {
    if (purchase.Quantity <= 0)
    {
      return BadRequest("Quantity must be greater than 0.");
    }

    if (purchase.PurchasePrice <= 0)
    {
      return BadRequest("Purchase price must be greater than 0.");
    }

    if (string.IsNullOrWhiteSpace(purchase.Supplier))
    {
      return BadRequest("Supplier is required.");
    }
    // URL id and body id should match
    if (id != purchase.Id)
    {
      return BadRequest();
    }

    // Find existing purchase
    var existingPurchase = await _context.Purchases
        .FindAsync(id);

    if (existingPurchase == null)
    {
      return NotFound();
    }

    // Check product exists
    var productExists = await _context.Products
        .AnyAsync(p => p.Id == purchase.ProductId);

    if (!productExists)
    {
      return BadRequest("Invalid ProductId.");
    }

    // Find old inventory
    var oldInventory = await _context.Inventory
        .FirstOrDefaultAsync(i =>
            i.ProductId == existingPurchase.ProductId);

    if (oldInventory == null)
    {
      return BadRequest("Inventory record not found.");
    }

    // If product is changed
    if (existingPurchase.ProductId != purchase.ProductId)
    {
      // Remove old purchase quantity
      oldInventory.Quantity -= existingPurchase.Quantity;

      // Find new product inventory
      var newInventory = await _context.Inventory
          .FirstOrDefaultAsync(i =>
              i.ProductId == purchase.ProductId);

      if (newInventory == null)
      {
        return BadRequest("New product inventory not found.");
      }

      // Add new purchase quantity
      newInventory.Quantity += purchase.Quantity;
    }
    else
    {
      // Same product → adjust only the difference
      var difference =
          purchase.Quantity - existingPurchase.Quantity;

      oldInventory.Quantity += difference;
    }

    // Update purchase fields
    existingPurchase.ProductId = purchase.ProductId;
    existingPurchase.Quantity = purchase.Quantity;
    existingPurchase.PurchasePrice = purchase.PurchasePrice;
    existingPurchase.PurchaseDate = purchase.PurchaseDate;
    existingPurchase.Supplier =
    purchase.Supplier;

    await _context.SaveChangesAsync();

    return NoContent();
  }
  // DELETE: api/Purchases/{id}
  [HttpDelete("{id}")]
  public async Task<IActionResult> DeletePurchase(int id)
  {
    // Find purchase
    var purchase = await _context.Purchases
        .FindAsync(id);

    // Purchase doesn't exist
    if (purchase == null)
    {
      return NotFound();
    }

    // Find inventory
    var inventory = await _context.Inventory
        .FirstOrDefaultAsync(i => i.ProductId == purchase.ProductId);

    if (inventory == null)
    {
      return BadRequest("Inventory record not found.");
    }

    // Remove purchased quantity from stock
    inventory.Quantity -= purchase.Quantity;

    // Delete purchase
    _context.Purchases.Remove(purchase);

    // Save both changes
    await _context.SaveChangesAsync();

    return NoContent();
  }
}