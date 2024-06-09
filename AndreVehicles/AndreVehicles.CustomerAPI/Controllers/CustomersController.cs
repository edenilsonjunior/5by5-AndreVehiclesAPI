using AndreVehicles.CustomerAPI.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.People;
using Services.People;

namespace AndreVehicles.CustomerAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CustomersController : ControllerBase
{
    private readonly AndreVehiclesCustomerAPIContext _context;
    private readonly CustomerService _service;

    public CustomersController(AndreVehiclesCustomerAPIContext context)
    {
        _context = context;
        _service = new CustomerService();
    }

    [HttpGet("{technology}")]
    public async Task<ActionResult<IEnumerable<Customer>>> GetCustomer(string technology)
    {
        switch (technology)
        {
            case "entity":
                return _context.Customer == null ? NotFound() : await _context.Customer.Include(a => a.Address).ToListAsync();

            case "dapper":
            case "ado":
                var customers = _service.Get(technology);
                return customers == null || customers.Count == 0 ? NotFound() : customers;

            default:
                return BadRequest("Invalid technology. Valid values are: entity, dapper, ado");
        }
    }

    [HttpGet("{technology}/{id}")]
    public async Task<ActionResult<Customer>> GetCustomer(string technology, string id)
    {
        Customer? customer;

        switch (technology)
        {
            case "entity":
                if (_context.Customer == null)
                    return NotFound();

                customer = await _context.Customer.Include(a => a.Address).Where(c => c.Document == id).FirstOrDefaultAsync();
                return customer != null ? customer : NotFound();

            case "dapper":
            case "ado":
                customer = _service.Get(technology, id);
                return customer != null ? customer : NotFound();

            default:
                return BadRequest("Invalid technology. Valid values are: entity, dapper, ado");
        }
    }

    [HttpPost("{technology}")]
    public async Task<ActionResult<Customer>> PostCustomer(string technology, Customer customer)
    {
        switch (technology)
        {
            case "entity":
                if (_context.Customer == null)
                    return Problem("Entity set 'AndreVehiclesCustomerAPIContext.Customer' is null.");

                _context.Customer.Add(customer);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetCustomer", new { id = customer.Document }, customer);

            case "dapper":
            case "ado":
                bool success = _service.Post(technology, customer);
                return success ? CreatedAtAction("GetCustomer", new { id = customer.Document }, customer) : BadRequest();

            default:
                return BadRequest("Invalid technology. Valid values are: entity, dapper, ado");
        }
    }

    /*
    [HttpPut("{id}")]
    public async Task<IActionResult> PutCustomer(string id, Customer customer)
    {
        if (id != customer.Document)
        {
            return BadRequest();
        }

        _context.Entry(customer).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!CustomerExists(id))
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
    public async Task<IActionResult> DeleteCustomer(string id)
    {
        if (_context.Customer == null)
        {
            return NotFound();
        }

        var customer = await _context.Customer.FindAsync(id);
        if (customer == null)
        {
            return NotFound();
        }

        _context.Customer.Remove(customer);
        await _context.SaveChangesAsync();

        return NoContent();
    } */

    private bool CustomerExists(string id)
    {
        return (_context.Customer?.Any(e => e.Document == id)).GetValueOrDefault();
    }
}
