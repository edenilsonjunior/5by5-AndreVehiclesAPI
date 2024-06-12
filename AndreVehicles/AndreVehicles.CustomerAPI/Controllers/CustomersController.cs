using AndreVehicles.CustomerAPI.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.DTO.People;
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
                return customer.Document != null ? customer : NotFound();

            case "dapper":
            case "ado":
                customer = _service.Get(technology, id);
                return customer != null ? customer : NotFound();

            default:
                return BadRequest("Invalid technology. Valid values are: entity, dapper, ado");
        }
    }

    [HttpPost("{technology}")]
    public async Task<ActionResult<Customer>> PostCustomer(string technology, CustomerDTO customerDTO)
    {

        Address? address;

        try
        {
            using HttpClient client = new();
            client.BaseAddress = new Uri("https://localhost:7020");

            string cep = customerDTO.Address.PostalCode;
            HttpResponseMessage response = await client.GetAsync($"/GetAddressByCep/{cep}");

            response.EnsureSuccessStatusCode();
            address = await response.Content.ReadFromJsonAsync<Address>();
        }
        catch (Exception)
        {
            return BadRequest($"Failed to retrieve address.");
        }

        if (address == null)
            return BadRequest("Address not found.");

        address.PostalCode = customerDTO.Address.PostalCode;
        address.AdditionalInfo = customerDTO.Address.AdditionalInfo;
        address.Number = customerDTO.Address.Number;
        address.StreetType = customerDTO.Address.StreetType;


        Customer customer = new()
        {
            Document = customerDTO.Document,
            Name = customerDTO.Name,
            BirthDate = customerDTO.BirthDate,
            Address = address,
            Phone = customerDTO.Phone,
            Email = customerDTO.Email,
            Income = customerDTO.Income
        };

        if (new AddressService().PostMongo(address) == null)
            return BadRequest("Failed to save address in MongoDB.");


        switch (technology)
        {
            case "entity":
                if (_context.Customer == null)
                    return Problem("Entity set 'AndreVehiclesCustomerAPIContext.Customer' is null.");

                _context.Customer.Add(customer);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetCustomer", new { technology, id = customer.Document }, customer);

            case "dapper":
            case "ado":
                bool success = _service.Post(technology, customer);
                return success ? CreatedAtAction("GetCustomer", new { technology, id = customer.Document }, customer) : BadRequest("Failed to insert customer.");

            default:
                return BadRequest("Invalid technology. Valid values are: entity, dapper, ado");
        }
    }

}
