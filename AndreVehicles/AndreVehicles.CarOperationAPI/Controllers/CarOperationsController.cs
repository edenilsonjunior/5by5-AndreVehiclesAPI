using AndreVehicles.CarOperationAPI.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.Cars;
using Models.DTO.Cars;
using Services.Cars;

namespace AndreVehicles.CarOperationAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CarOperationsController : ControllerBase
{
    private readonly AndreVehiclesCarOperationAPIContext _context;

    private readonly CarService _carService;
    private readonly OperationService _operationService;
    private readonly CarOperationService _service;

    public CarOperationsController(AndreVehiclesCarOperationAPIContext context)
    {
        _context = context;
        _carService = new CarService();
        _operationService = new OperationService();
        _service = new CarOperationService();
    }


    [HttpGet("{technology}")] // GET: api/CarOperations
    public async Task<ActionResult<IEnumerable<CarOperation>>> GetCarOperation(string technology)
    {
        switch (technology)
        {
            case "entity":
                return _context.CarOperation == null ? NotFound() : await _context.CarOperation.Include(c => c.Car).Include(o => o.Operation).ToListAsync();

            case "dapper":
            case "ado":
                var carOperationList = _service.Get(technology);
                return carOperationList == null || carOperationList.Count == 0 ? NotFound() : carOperationList;

            default:
                return BadRequest("Invalid technology. Valid values are: entity, dapper, ado");
        }
    }



    [HttpGet("{technology}/{id}")] // GET: api/CarOperations/5
    public async Task<ActionResult<CarOperation>> GetCarOperation(string technology, int id)
    {
        CarOperation? carOperation;

        switch (technology)
        {
            case "entity":
                if (_context.CarOperation == null)
                    return NotFound();

                carOperation = await _context.CarOperation
                    .Include(c => c.Car)
                    .Include(o => o.Operation)
                    .Where(cp => cp.Id == id)
                    .SingleOrDefaultAsync(c => c.Id == id);

                return carOperation != null ? carOperation : NotFound();

            case "dapper":
            case "ado":
                carOperation = _service.Get(technology, id);

                return carOperation != null ? carOperation : NotFound();

            default:
                return BadRequest("Invalid technology. Valid values are: entity, dapper, ado");
        }
    }


    [HttpPost("{technology}")] // POST: api/CarOperations
    public async Task<ActionResult<CarOperation>> PostCarOperation(string technology, CarOperationDTO carOperationDTO)
    {

        Car car;
        Operation operation;
        CarOperation co;

        switch (technology)
        {
            case "entity":

                if (_context.CarOperation == null)
                    return Problem("Entity set 'AndreVehiclesCarOperationAPIContext.CarOperation'  is null.");

                car = await _context.Car.FindAsync(carOperationDTO.CarPlate);
                if (car == null) return NotFound("Car not found.");

                operation = await _context.Operation.FindAsync(carOperationDTO.OperationId);
                if (operation == null) return NotFound("Operation not found.");

                co = new CarOperation
                {
                    Car = car,
                    Operation = operation,
                    Status = carOperationDTO.Status
                };

                _context.CarOperation.Add(co);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetCarOperation", new {technology, id = co.Id }, co);

            case "dapper":
            case "ado":

                car = _carService.Get(technology, carOperationDTO.CarPlate);
                if (car == null) return NotFound("Car not found.");

                operation = _operationService.Get(technology, carOperationDTO.OperationId);
                if (operation == null) return NotFound("Operation not found.");

                co = new CarOperation
                {
                    Car = car,
                    Operation = operation,
                    Status = carOperationDTO.Status
                };

                co.Id = _service.Post(technology, co);

                return co.Id != -1 ? CreatedAtAction("GetCarOperation", new {technology, id = co.Id }, co) : BadRequest();

            default:
                return BadRequest("Invalid technology. Valid values are: entity, dapper, ado");
        }

    }

    /*
    [HttpDelete("{id}")] // DELETE: api/CarOperations/5
    public async Task<IActionResult> DeleteCarOperation(int id)
    {
        if (_context.CarOperation == null)
        {
            return NotFound();
        }
        var carOperation = await _context.CarOperation.FindAsync(id);
        if (carOperation == null)
        {
            return NotFound();
        }

        _context.CarOperation.Remove(carOperation);
        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpPut("{id}")] // PUT: api/CarOperations/5
    public async Task<IActionResult> PutCarOperation(int id, CarOperation carOperation)
    {
        if (id != carOperation.Id)
        {
            return BadRequest();
        }

        _context.Entry(carOperation).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!CarOperationExists(id))
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
    */


    private bool CarOperationExists(int id)
    {
        return (_context.CarOperation?.Any(e => e.Id == id)).GetValueOrDefault();
    }
}
