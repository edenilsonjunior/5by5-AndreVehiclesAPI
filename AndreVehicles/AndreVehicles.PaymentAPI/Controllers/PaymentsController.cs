using AndreVehicles.PaymentAPI.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.Sales;
using Services.Sales;

namespace AndreVehicles.PaymentAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PaymentsController : ControllerBase
{
    private readonly AndreVehiclesPaymentAPIContext _context;
    private readonly PaymentService _service;

    public PaymentsController(AndreVehiclesPaymentAPIContext context)
    {
        _context = context;
        _service = new PaymentService();
    }


    [HttpGet("{technology}")] 
    public async Task<ActionResult<IEnumerable<Payment>>> GetPayment(string technology)
    {
        switch (technology)
        {
            case "entity":
                if (_context.Payment == null)
                    return NotFound();

                return await _context.Payment
                    .Include(p => p.Pix)
                    .ThenInclude(p => p.Type)
                    .Include(b => b.BankSlip)
                    .Include(c => c.Card)
                    .ToListAsync();

            case "dapper":
            case "ado":
                var payments = _service.Get(technology);
                return payments == null || payments.Count == 0 ? NotFound() : payments;

            default:
                return BadRequest("Invalid technology. Valid values are: entity, dapper, ado");
        }
    }

    
    [HttpGet("{technology}/{id}")] 
    public async Task<ActionResult<Payment>> GetPayment(string technology, int id)
    {
        Payment? payment;
        switch (technology)
        {
            case "entity":
                if (_context.Payment == null)
                    return NotFound();

                payment = await _context.Payment.Include(p => p.Pix).Include(p => p.Pix.Type).Include(b => b.BankSlip).Include(c => c.Card).SingleOrDefaultAsync(p => p.Id == id);
                return payment != null ? payment : NotFound();

            case "dapper":
            case "ado":
                payment = _service.Get(technology, id);
                return payment != null ? payment : NotFound();

            default:
                return BadRequest("Invalid technology. Valid values are: entity, dapper, ado");
        }
    }


    [HttpPost("technology")]
    public async Task<ActionResult<Payment>> PostPayment(string technology, Payment payment)
    {
        switch (technology)
        {
            case "entity":
                if (_context.Payment == null)
                    return NotFound();

                _context.Payment.Add(payment);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetPayment", new { id = payment.Id }, payment);

            case "dapper":
            case "ado":
                bool success = _service.Post(technology, payment);

                return success ? CreatedAtAction("GetPayment", new { id = payment.Id }, payment) : BadRequest();

            default:
                return BadRequest("Invalid technology. Valid values are: entity, dapper, ado");
        }
    }


    /*
    [HttpPut("{id}")]  // PUT: api/Payments/5
    public async Task<IActionResult> PutPayment(int id, Payment payment)
    {
        if (id != payment.Id)
        {
            return BadRequest();
        }

        _context.Entry(payment).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!PaymentExists(id))
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


    // DELETE: api/Payments/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePayment(int id)
    {
        if (_context.Payment == null)
        {
            return NotFound();
        }
        var payment = await _context.Payment.FindAsync(id);
        if (payment == null)
        {
            return NotFound();
        }

        _context.Payment.Remove(payment);
        await _context.SaveChangesAsync();

        return NoContent();
    }
    */

    private bool PaymentExists(int id)
    {
        return (_context.Payment?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}
