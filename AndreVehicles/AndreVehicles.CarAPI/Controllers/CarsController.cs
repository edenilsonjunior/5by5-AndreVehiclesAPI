using AndreVehicles.CarAPI.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.Cars;
using Services.Cars;

namespace AndreVehicles.CarAPI.Controllers;


[Route("api/[controller]")]
[ApiController]
public class CarsController : ControllerBase
{
    private readonly AndreVehiclesCarAPIContext _context;
    private readonly CarService _service;

    public CarsController(AndreVehiclesCarAPIContext context)
    {
        _context = context;
        _service = new CarService();
    }


    [HttpGet("{technology}")] // GET: api/Cars
    public async Task<ActionResult<IEnumerable<Car>>> GetCar(string technology)
    {
        switch (technology)
        {
            case "entity":
                return _context.Car == null ? NotFound() : await _context.Car.ToListAsync();

            case "dapper":
            case "ado":
                var cars = _service.Get(technology);
                return cars == null || cars.Count == 0 ? NotFound() : cars;

            default:
                return BadRequest("Invalid technology. Valid values are: entity, dapper, ado");
        }
    }


    [HttpGet("{technology}/{plate}")] // GET: api/Cars/5
    public async Task<ActionResult<Car>> GetCar(string technology, string plate)
    {
        Car? car;

        switch (technology)
        {
            case "entity":
                if (_context.Car == null)
                    return NotFound();

                car = await _context.Car.FindAsync(plate);
                return car != null ? car : NotFound();

            case "dapper":
            case "ado":
                car = _service.Get(technology, plate);

                return car.Plate != null ? car : NotFound();

            default:
                return BadRequest("Invalid technology. Valid values are: entity, dapper, ado");
        }
    }


    [HttpPost("{technology}")] // POST: api/Cars
    public async Task<ActionResult<Car>> PostCar(string technology, Car car)
    {

        switch (technology)
        {
            case "entity":
                if (_context.Car == null)
                    return Problem("Entity set 'AndreVehiclesCarAPIContext.Car'  is null.");

                _context.Car.Add(car);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetCar", new { id = car.Plate }, car);

            case "dapper":
            case "ado":

                bool success = _service.Post(technology, car);

                return success ? CreatedAtAction("GetCar", new { technology = technology, plate = car.Plate }, car) : BadRequest();

            default:
                return BadRequest("Invalid technology. Valid values are: entity, dapper, ado");
        }
    }


    private bool CarExists(string id)
    {
        return (_context.Car?.Any(e => e.Plate == id)).GetValueOrDefault();
    }


    /*
    [HttpPut("{id}")]  // PUT: api/Cars/5
    public async Task<IActionResult> PutCar(string id, Car car)
    {
        if (id != car.Plate)
        {
            return BadRequest();
        }

        _context.Entry(car).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!CarExists(id))
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

}
