using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Day21_Inventory_System.Data;

namespace Day21_Inventory_System.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReportsController : ControllerBase
{
  private readonly AppDbContext _context;
  public ReportsController(AppDbContext context)
  {
    _context = context;
  }
  [HttpGet("purchasees-summary")]
  public async Task<IActionResult> GetPurchasesSummary()
  {
    var summary = await _context.Purchases
    .GroupBy(p => p.ProductId)
    .Select(g => new
    {
      ProductId = g.Key,
      TotalPurchased = g.Sum(p => p.Quantity)
    })
    .ToListAsync();
    return Ok(summary);
  }
  [HttpGet("sales-summary")]
  public async Task<IActionResult> GetSalesSummary()
  {
    var summary = await _context.Sales
        .GroupBy(s => s.ProductId)
        .Select(g => new
        {
          ProductId = g.Key,
          TotalSold = g.Sum(s => s.Quantity)
        })
        .ToListAsync();

    return Ok(summary);
  }
}