using AndreVehicles.OperationAPI.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.Cars;
using Services.Cars;

namespace AndreVehicles.OperationAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OperationsController : ControllerBase
{
    private readonly AndreVehiclesOperationAPIContext _context;
    private readonly OperationService _service;

    public OperationsController(AndreVehiclesOperationAPIContext context)
    {
        _context = context;
        _service = new OperationService();
    }


    [HttpGet("{technology}")]
    public async Task<ActionResult<IEnumerable<Operation>>> GetOperation(string technology)
    {
        switch (technology)
        {
            case "entity":
                return _context.Operation == null ? NotFound() : await _context.Operation.ToListAsync();

            case "dapper":
            case "ado":
                var operations = _service.Get(technology);
                return operations == null || operations.Count == 0 ? NotFound() : operations;

            default:
                return BadRequest("Invalid technology. Valid values are: entity, dapper, ado");
        }
    }

    [HttpGet("{technology}/{id}")]
    public async Task<ActionResult<Operation>> GetOperation(string technology, int id)
    {
        Operation? operation;

        switch (technology)
        {
            case "entity":
                if (_context.Operation == null)
                    return NotFound();

                operation = await _context.Operation.FindAsync(id);
                return operation?.Description != null ? operation : NotFound();

            case "dapper":
            case "ado":
                operation = _service.Get(technology, id);
                return operation != null ? operation : NotFound();

            default:
                return BadRequest("Invalid technology. Valid values are: entity, dapper, ado");
        }
    }

    [HttpPost("{technology}/{description}")]
    public async Task<ActionResult<Operation>> PostOperation(string technology, string description)
    {
        Operation operation = new(description);

        switch (technology)
        {
            case "entity":
                if (_context.Operation == null)
                    return Problem("Entity set 'AndreVehiclesOperationAPIContext.Operation' is null.");

                _context.Operation.Add(operation);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetOperation", new { technology = technology, id = operation.Id }, operation);

            case "dapper":
            case "ado":
                int success = _service.Post(technology, operation);
                return success != -1 ? CreatedAtAction("GetOperation", new { technology = technology, id = operation.Id }, operation) : BadRequest();

            default:
                return BadRequest("Invalid technology. Valid values are: entity, dapper, ado");
        }
    }

    /*
    [HttpPut("{id}")]
    public async Task<IActionResult> PutOperation(int id, Operation operation)
    {
        if (id != operation.Id)
        {
            return BadRequest();
        }

        _context.Entry(operation).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!OperationExists(id))
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
    public async Task<IActionResult> DeleteOperation(int id)
    {
        if (_context.Operation == null)
        {
            return NotFound();
        }

        var operation = await _context.Operation.FindAsync(id);
        if (operation == null)
        {
            return NotFound();
        }

        _context.Operation.Remove(operation);
        await _context.SaveChangesAsync();

        return NoContent();
    }*/

    private bool OperationExists(int id)
    {
        return (_context.Operation?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}
