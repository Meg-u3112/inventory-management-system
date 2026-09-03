using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Day21_Inventory_System.Data;
using Day21_Inventory_System.Models;

namespace Day21_Inventory_System.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InventoryController : ControllerBase
{
  private readonly AppDbContext _context;

  public InventoryController(AppDbContext context)
  {
    _context = context;
  }
  // GET api/Inventory
  [HttpGet]
  public async Task<ActionResult<IEnumerable<Inventory>>> GetInventory()
  {
    var inventory = await _context.Inventory.Include(i => i.Product).ToListAsync();

    return Ok(inventory);
  }

  [HttpGet("{id}")]
  public async Task<ActionResult<Inventory>> GetInventory(int id)
  {
    var inventory = await _context.Inventory
    .Include(i => i.Product)
    .FirstOrDefaultAsync(i => i.ProductId == id);

    if (inventory == null)
    {
      return NotFound();
    }
    return Ok(inventory);
  }
  [HttpGet("low-stock")]
  public async Task<ActionResult<IEnumerable<Inventory>>> GetLowStock(
    int threshold = 10)
  {
    var inventory = await _context.Inventory
        .Include(i => i.Product)
        .Where(i => i.Quantity < threshold)
        .ToListAsync();

    return Ok(inventory);
  }
}