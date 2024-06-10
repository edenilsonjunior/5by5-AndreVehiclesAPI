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
                return _context.Employee == null ? NotFound() : await _context.Employee.ToListAsync();

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

                employee = await _context.Employee.FindAsync(id);
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
        Address address = await new AddressService().GetAddressByPostalCode(employeeDTO.Address);

        Employee employee = new()
        {
            Document = employeeDTO.Document,
            Name = employeeDTO.Name,
            BirthDate = employeeDTO.BirthDate,
            Address = address
        };

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
