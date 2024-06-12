using AndreVehicles.EmployeeAPI.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.DTO.People;
using Models.People;
using Services.People;

namespace AndreVehicles.EmployeeAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class EmployeesController : ControllerBase
{
    private readonly AndreVehiclesEmployeeAPIContext _context;
    private readonly EmployeeService _service;


    public EmployeesController(AndreVehiclesEmployeeAPIContext context)
    {
        _context = context;
        _service = new EmployeeService();
    }


    [HttpGet("{technology}")]
    public async Task<ActionResult<IEnumerable<Employee>>> GetEmployee(string technology)
    {
        switch (technology)
        {
            case "entity":
                return _context.Employee == null ? NotFound() : await _context.Employee.Include(e => e.Role).Include(e => e.Address).ToListAsync();

            case "dapper":
            case "ado":
                var employees = _service.Get(technology);
                return employees == null || employees.Count == 0 ? NotFound() : employees;

            default:
                return BadRequest("Invalid technology. Valid values are: entity, dapper, ado");
        }
    }


    [HttpGet("{technology}/{id}")]
    public async Task<ActionResult<Employee>> GetEmployee(string technology, string id)
    {
        Employee? employee;

        switch (technology)
        {
            case "entity":
                if (_context.Employee == null)
                    return NotFound();

                employee = await _context.Employee.Include(e => e.Role).Include(e => e.Address).FirstOrDefaultAsync(e => e.Document == id);
                return employee != null ? employee : NotFound();

            case "dapper":
            case "ado":
                employee = _service.Get(technology, id);
                return employee != null ? employee : NotFound();

            default:
                return BadRequest("Invalid technology. Valid values are: entity, dapper, ado");
        }
    }


    [HttpPost("{technology}")]
    public async Task<ActionResult<Employee>> PostEmployee(string technology, EmployeeDTO employeeDTO)
    {

        Address? address;

        try
        {
            using HttpClient client = new();
            client.BaseAddress = new Uri("https://localhost:7020");

            string cep = employeeDTO.Address.PostalCode;
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

        address.PostalCode = employeeDTO.Address.PostalCode;
        address.AdditionalInfo = employeeDTO.Address.AdditionalInfo;
        address.Number = employeeDTO.Address.Number;
        address.StreetType = employeeDTO.Address.StreetType;







        Employee employee = new()
        {
            Document = employeeDTO.Document,
            Name = employeeDTO.Name,
            BirthDate = employeeDTO.BirthDate,
            Address = address,
            Phone = employeeDTO.Phone,
            Email = employeeDTO.Email,
            CommissionValue = employeeDTO.CommissionValue,
            Commission = employeeDTO.Commission,
            Role = new Role
            {
                Description = employeeDTO.Role.Description
            }
        };

        if (new AddressService().PostMongo(address) == null)
            return BadRequest("Failed to save address in MongoDB.");


        switch (technology)
        {
            case "entity":
                if (_context.Employee == null)
                    return Problem("Entity set 'AndreVehiclesEmployeeAPIContext.Employee' is null.");

                _context.Employee.Add(employee);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetEmployee", new { technology, id = employee.Document }, employee);

            case "dapper":
            case "ado":
                bool success = _service.Post(technology, employee);
                return success ? CreatedAtAction("GetEmployee", new { technology, id = employee.Document }, employee) : BadRequest();

            default:
                return BadRequest("Invalid technology. Valid values are: entity, dapper, ado");
        }
    }


    /*
    [HttpPut("{id}")]
    public async Task<IActionResult> PutEmployee(string id, Employee employee)
    {
        if (id != employee.Document)
        {
            return BadRequest();
        }

        _context.Entry(employee).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!EmployeeExists(id))
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
    public async Task<IActionResult> DeleteEmployee(string id)
    {
        if (_context.Employee == null)
        {
            return NotFound();
        }

        var employee = await _context.Employee.FindAsync(id);
        if (employee == null)
        {
            return NotFound();
        }

        _context.Employee.Remove(employee);
        await _context.SaveChangesAsync();

        return NoContent();
    }
    */


    private bool EmployeeExists(string id)
    {
        return (_context.Employee?.Any(e => e.Document == id)).GetValueOrDefault();
    }
}
