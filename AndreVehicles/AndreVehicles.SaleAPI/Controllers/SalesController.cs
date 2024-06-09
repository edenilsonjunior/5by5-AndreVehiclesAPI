using AndreVehicles.SaleAPI.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.Sales;
using Services.Sales;

namespace AndreVehicles.SaleAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SalesController : ControllerBase
{
    private readonly AndreVehiclesSaleAPIContext _context;
    private readonly SaleService _service;

    public SalesController(AndreVehiclesSaleAPIContext context)
    {
        _context = context;
        _service = new SaleService();
    }


    [HttpGet("{technology}")]
    public async Task<ActionResult<IEnumerable<Sale>>> GetSale(string technology)
    {
        switch (technology)
        {
            case "entity":
                return _context.Sale == null ? NotFound() : await _context.Sale.Include(c => c.Customer).Include(e => e.Employee).Include(p => p.Payment).ToListAsync();

            case "dapper":
            case "ado":
                var sales = _service.Get(technology);
                return sales == null || sales.Count == 0 ? NotFound() : sales;

            default:
                return BadRequest("Invalid technology. Valid values are: entity, dapper, ado");
        }
    }


    [HttpGet("{technology}/{id}")]
    public async Task<ActionResult<Sale>> GetSale(string technology, int id)
    {
        Sale? sale;
        switch (technology)
        {
            case "entity":
                if (_context.Sale == null)
                    return NotFound();

                sale = await _context.Sale.Include(c => c.Customer).Include(e => e.Employee).Include(p => p.Payment).SingleOrDefaultAsync(s => s.Id == id);
                return sale != null ? sale : NotFound();

            case "dapper":
            case "ado":
                sale = _service.Get(technology, id);
                return sale != null ? sale : NotFound();

            default:
                return BadRequest("Invalid technology. Valid values are: entity, dapper, ado");
        }
    }


    [HttpPost("{technology}")]
    public async Task<ActionResult<Sale>> PostSale(string technology, Sale sale)
    {
        switch (technology)
        {
            case "entity":
                _context.Sale.Add(sale);
                await _context.SaveChangesAsync();
                return CreatedAtAction("GetSale", new { id = sale.Id }, sale);

            case "dapper":
            case "ado":
                bool success = _service.Post(technology, sale);

                return success ? CreatedAtAction("GetSale", new { id = sale.Id }, sale) : BadRequest();

            default:
                return BadRequest("Invalid technology. Valid values are: entity, dapper, ado");
        }
    }


    /*
    [HttpPut("{id}")]
    public async Task<IActionResult> PutSale(int id, Sale sale)
    {
        if (id != sale.Id)
        {
            return BadRequest();
        }

        _context.Entry(sale).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!SaleExists(id))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }


    // DELETE: api/Sales/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteSale(int id)
    {
        if (_context.Sale == null)
        {
            return NotFound();
        }
        var sale = await _context.Sale.FindAsync(id);
        if (sale == null)
        {
            return NotFound();
        }

        _context.Sale.Remove(sale);
        await _context.SaveChangesAsync();

        return NoContent();
    }
    */


    private bool SaleExists(int id)
    {
        return (_context.Sale?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}
