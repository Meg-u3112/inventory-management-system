using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Day21_Inventory_System.Data;
using Day21_Inventory_System.Models;

namespace Day21_Inventory_System.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController : ControllerBase
{
  private readonly AppDbContext _context;

  public CategoriesController(AppDbContext context)
  {
    _context = context;
  }


  // GET: api/Categories
  [HttpGet]
  public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
  {
    var categories = await _context.Categories
        .ToListAsync();

    return Ok(categories);
  }
  // GET: api/Categories/1
  [HttpGet("{id}")]
  public async Task<ActionResult<Category>> GetCategory(int id)
  {
    var category = await _context.Categories
        .FirstOrDefaultAsync(c => c.Id == id);

    if (category == null)
    {
      return NotFound();
    }

    return Ok(category);
  }
  // POST: api/Categories
  [HttpPost]
  public async Task<ActionResult<Category>> CreateCategory(Category category)
  {
    _context.Categories.Add(category);

    await _context.SaveChangesAsync();

    return CreatedAtAction(
        nameof(GetCategory),
        new { id = category.Id },
        category
    );
  }
  // PUT: api/Categories/6
  [HttpPut("{id}")]
  public async Task<IActionResult> UpdateCategory(int id, Category category)
  {
    // Check URL id and body id are same
    if (id != category.Id)
    {
      return BadRequest();
    }

    // Check whether category exists
    var existingCategory = await _context.Categories
        .FindAsync(id);

    if (existingCategory == null)
    {
      return NotFound();
    }

    // Update the existing record
    existingCategory.Name = category.Name;

    await _context.SaveChangesAsync();

    return NoContent();
  }
  // DELETE: api/Categories/6
  [HttpDelete("{id}")]
  public async Task<IActionResult> DeleteCategory(int id)
  {
    // Find category
    var category = await _context.Categories
        .FindAsync(id);

    // Category doesn't exist
    if (category == null)
    {
      return NotFound();
    }

    // Remove category
    _context.Categories.Remove(category);

    // Save changes
    await _context.SaveChangesAsync();

    return NoContent();
  }
}