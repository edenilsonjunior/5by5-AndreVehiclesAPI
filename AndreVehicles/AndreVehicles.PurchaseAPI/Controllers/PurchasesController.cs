using AndreVehicles.PurchaseAPI.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.Cars;
using Services.Cars;

namespace AndreVehicles.PurchaseAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PurchasesController : ControllerBase
{
    private readonly AndreVehiclesPurchaseAPIContext _context;
    private readonly PurchaseService _service;

    public PurchasesController(AndreVehiclesPurchaseAPIContext context)
    {
        _context = context;
        _service = new PurchaseService();
    }


    [HttpGet("{technology}")]
    public async Task<ActionResult<IEnumerable<Purchase>>> GetPurchase(string technology)
    {
        switch (technology)
        {
            case "entity":
                return _context.Purchase == null ? NotFound() : await _context.Purchase.Include(c => c.Car).ToListAsync();

            case "dapper":
            case "ado":
                var purchases = _service.Get(technology);
                return purchases == null || purchases.Count == 0 ? NotFound() : purchases;

            default:
                return BadRequest("Invalid technology. Valid values are: entity, dapper, ado");
        }
    }


    [HttpGet("{technology}/{id}")]
    public async Task<ActionResult<Purchase>> GetPurchase(string technology, int id)
    {
        Purchase? purchase;
        switch (technology)
        {
            case "entity":
                if (_context.Purchase == null)
                    return NotFound();

                purchase = await _context.Purchase.Include(c => c.Car).FirstOrDefaultAsync();
                return purchase != null ? purchase : NotFound();

            case "dapper":
            case "ado":
                purchase = _service.Get(technology, id);
                return purchase != null ? purchase : NotFound();

            default:
                return BadRequest("Invalid technology. Valid values are: entity, dapper, ado");
        }
    }


    [HttpPost("{technology}")]
    public async Task<ActionResult<Purchase>> PostPurchase(string technology, Purchase purchase)
    {
        switch (technology)
        {
            case "entity":
                _context.Purchase.Add(purchase);
                await _context.SaveChangesAsync();
                return CreatedAtAction("GetPurchase", new { id = purchase.Id }, purchase);

            case "dapper":
            case "ado":
                bool success = _service.Post(technology, purchase);
                return success ? CreatedAtAction("GetPurchase", new { id = purchase.Id }, purchase) : BadRequest();

            default:
                return BadRequest("Invalid technology. Valid values are: entity, dapper, ado");
        }
    }


    /*
    [HttpPut("{id}")]
    public async Task<IActionResult> PutPurchase(int id, Purchase purchase)
    {
        if (id != purchase.Id)
        {
            return BadRequest();
        }

        _context.Entry(purchase).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!PurchaseExists(id))
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


    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePurchase(int id)
    {
        if (_context.Purchase == null)
        {
            return NotFound();
        }
        var purchase = await _context.Purchase.FindAsync(id);
        if (purchase == null)
        {
            return NotFound();
        }

        _context.Purchase.Remove(purchase);
        await _context.SaveChangesAsync();

        return NoContent();
    }
    */


    private bool PurchaseExists(int id)
    {
        return (_context.Purchase?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}
