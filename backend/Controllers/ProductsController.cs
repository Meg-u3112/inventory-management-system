using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Day21_Inventory_System.Data;
using Day21_Inventory_System.Models;

namespace Day21_Inventory_System.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
  private readonly AppDbContext _context;

  public ProductsController(AppDbContext context)
  {
    _context = context;
  }

  // ==========================================
  // GET: api/Products
  // Get all products
  // ==========================================
  [HttpGet]
  public async Task<ActionResult<IEnumerable<Product>>> GetProducts(
    int page = 1,
    int pagesize = 5)
  {
    if (page < 1 || pagesize < 1)
    {
      return BadRequest("Page and Pagesize must be greater than 0.");
    }
    var totalProducts = await _context.Products.CountAsync();

    var products = await _context.Products
        .Include(p => p.Category)
        .Skip((page - 1) * pagesize)
        .Take(pagesize)
        .ToListAsync();
    var totalPages = (int)Math.Ceiling(totalProducts / (double)pagesize);

    return Ok(new
    {
      page,
      pagesize,
      totalProducts,
      totalPages,
      products
    });
  }


  // ==========================================
  // GET: api/Products/{id}
  // Get one product
  // ==========================================
  [HttpGet("{id}")]
  public async Task<ActionResult<Product>> GetProduct(int id)
  {
    var product = await _context.Products
        .Include(p => p.Category)
        .FirstOrDefaultAsync(p => p.Id == id);

    if (product == null)
    {
      return NotFound();
    }

    return Ok(product);
  }


  // ==========================================
  // POST: api/Products
  // Create product
  // ==========================================
  [HttpPost]
  public async Task<ActionResult<Product>> CreateProduct(Product product)
  {
    // Check category exists
    var categoryExists = await _context.Categories
        .AnyAsync(c => c.Id == product.CategoryId);

    if (!categoryExists)
    {
      return BadRequest("Invalid CategoryId.");
    }

    _context.Products.Add(product);

    await _context.SaveChangesAsync();

    return CreatedAtAction(
        nameof(GetProduct),
        new { id = product.Id },
        product
    );
  }


  // ==========================================
  // PUT: api/Products/{id}
  // Update product
  // ==========================================
  [HttpPut("{id}")]
  public async Task<IActionResult> UpdateProduct(
      int id,
      Product product)
  {
    // Check URL ID and body ID
    if (id != product.Id)
    {
      return BadRequest();
    }

    // Check product exists
    var existingProduct = await _context.Products
        .FindAsync(id);

    if (existingProduct == null)
    {
      return NotFound();
    }

    // Check category exists
    var categoryExists = await _context.Categories
        .AnyAsync(c => c.Id == product.CategoryId);

    if (!categoryExists)
    {
      return BadRequest("Invalid CategoryId.");
    }

    // Update fields
    existingProduct.Name = product.Name;
    existingProduct.Price = product.Price;
    existingProduct.CategoryId = product.CategoryId;

    await _context.SaveChangesAsync();

    return NoContent();
  }


  // ==========================================
  // DELETE: api/Products/{id}
  // Delete product
  // ==========================================
  [HttpDelete("{id}")]
  public async Task<IActionResult> DeleteProduct(int id)
  {
    var product = await _context.Products
        .FindAsync(id);

    if (product == null)
    {
      return NotFound();
    }

    _context.Products.Remove(product);

    await _context.SaveChangesAsync();

    return NoContent();
  }
}