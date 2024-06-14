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

    private readonly string InvalidTechnology;

    public PaymentsController(AndreVehiclesPaymentAPIContext context)
    {
        _context = context;
        _service = new PaymentService();

        InvalidTechnology = "Invalid technology. Valid values are: entity, dapper, ado";
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
                return BadRequest(InvalidTechnology);
        }
    }


    [HttpGet("{technology}/{id}")]
    public async Task<ActionResult<Payment>> GetPayment(string technology, int id)
    {
        Payment? payment;

        switch (technology)
        {
            case "entity":
                if (_context.Payment == null) return NotFound();

                payment = await _context.Payment
                    .Include(p => p.Pix)
                    .Include(p => p.Pix.Type)
                    .Include(b => b.BankSlip)
                    .Include(c => c.Card)
                    .SingleOrDefaultAsync(p => p.Id == id);

                return payment != null ? payment : NotFound();

            case "dapper":
            case "ado":
                payment = _service.Get(technology, id);
                return payment != null ? payment : NotFound();

            default:
                return BadRequest(InvalidTechnology);
        }
    }





    [HttpPost("pix/{technology}")]
    public async Task<ActionResult<Payment>> PostPixPayment(string technology, PixPaymentDTO paymentDTO)
    {
        Payment payment = new()
        {
            PaymentDate = paymentDTO.PaymentDate,
            Pix = paymentDTO.Pix
        };

        return await ReturnByTechnology(technology, payment);
    }


    [HttpPost("bankSlip/{technology}")]
    public async Task<ActionResult<Payment>> PostBankSlipPayment(string technology, BankSlipPaymentDTO bankSlipPaymentDTO)
    {
        Payment payment = new()
        {
            PaymentDate = bankSlipPaymentDTO.PaymentDate,
            BankSlip = bankSlipPaymentDTO.BankSlip
        };

        return await ReturnByTechnology(technology, payment);
    }


    [HttpPost("card/{technology}")]
    public async Task<ActionResult<Payment>> PostCardPayment(string technology, CardPaymentDTO cardPaymentDTO)
    {
        Payment payment = new()
        {
            PaymentDate = cardPaymentDTO.PaymentDate,
            Card = cardPaymentDTO.Card
        };

        return await ReturnByTechnology(technology, payment);
    }







    private async Task<ActionResult<Payment>> ReturnByTechnology(string technology, Payment payment)
    {
        return technology switch
        {
            "entity" => await PostWithEntity(payment),
            "dapper" or "ado" => PostWithDapperOrAdo(technology, payment),
            _ => BadRequest(InvalidTechnology),
        };
    }


    private async Task<ActionResult<Payment>> PostWithEntity(Payment payment)
    {
        if (_context.Payment == null)
            return NotFound();

        _context.Payment.Add(payment);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetPayment", new { technology = "entity", id = payment.Id }, payment);
    }


    private ActionResult<Payment> PostWithDapperOrAdo(string technology, Payment payment)
    {
        bool success = _service.Post(technology, payment);

        if (success)
            return CreatedAtAction("GetPayment", new { technology, id = payment.Id }, payment);
        else
            return BadRequest($"Can't create Payment with technology ({technology})!");
    }
}
