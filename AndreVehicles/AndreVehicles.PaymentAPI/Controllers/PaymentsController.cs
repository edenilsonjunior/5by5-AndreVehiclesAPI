using AndreVehicles.PaymentAPI.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.DTO.Sales;
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




    [HttpPost("pix/{technology}")]
    public async Task<ActionResult<Payment>> PostPixPayment(string technology, PixPaymentDTO paymentDTO)
    {
        Payment payment = new()
        {
            Id = paymentDTO.Id,
            PaymentDate = paymentDTO.PaymentDate,
            Pix = new()
            {
                Id = paymentDTO.Id,
                PixKey = paymentDTO.PixKey,
                Type = new()
                {
                    Name = paymentDTO.PixTypeName
                }
            }
        };

        switch (technology)
        {
            case "entity":
                if (_context.Payment == null)
                    return NotFound();

                _context.Payment.Add(payment);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetPayment", new { technology, id = payment.Id }, payment);

            case "dapper":
            case "ado":
                bool success = _service.Post(technology, payment);

                return success ? CreatedAtAction("GetPayment", new { technology, id = payment.Id }, payment) : BadRequest();

            default:
                return BadRequest();
        }
    }


    [HttpPost("bankSlip/{technology}")]
    public async Task<ActionResult<Payment>> PostBankSlipPayment(string technology, BankSlipPaymentDTO bankSlipPaymentDTO)
    {
        Payment payment = new()
        {
            Id = bankSlipPaymentDTO.Id,
            PaymentDate = bankSlipPaymentDTO.DueDate,
            BankSlip = new()
            {
                Number = bankSlipPaymentDTO.Number,
                DueDate = bankSlipPaymentDTO.DueDate
            }
        };

        switch (technology)
        {
            case "entity":
                if (_context.Payment == null)
                    return NotFound();

                _context.Payment.Add(payment);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetPayment", new { technology, id = payment.Id }, payment);

            case "dapper":
            case "ado":
                bool success = _service.Post(technology, payment);

                return success ? CreatedAtAction("GetPayment", new { technology, id = payment.Id }, payment) : BadRequest();
            default:
                return BadRequest("Invalid technology. Valid values are: entity, dapper, ado");
        }
    }



    [HttpPost("card/{technology}")]
    public async Task<ActionResult<Payment>> PostCardPayment(string technology, CardPaymentDTO cardPaymentDTO)
    {
        Payment payment = new()
        {
            Id = cardPaymentDTO.Id,
            PaymentDate = cardPaymentDTO.PaymentDate,
            Card = new()
            {
                CardNumber = cardPaymentDTO.CardNumber,
                ExpirationDate = cardPaymentDTO.ExpirationDate,
                SecurityCode = cardPaymentDTO.SecurityCode,
                CardHolderName = cardPaymentDTO.CardHolderName
            }
        };

        switch (technology)
        {
            case "entity":
                if (_context.Payment == null)
                    return NotFound();

                _context.Payment.Add(payment);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetPayment", new { technology, id = payment.Id }, payment);

            case "dapper":
            case "ado":
                bool success = _service.Post(technology, payment);

                return success ? CreatedAtAction("GetPayment", new { technology, id = payment.Id }, payment) : BadRequest();

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
