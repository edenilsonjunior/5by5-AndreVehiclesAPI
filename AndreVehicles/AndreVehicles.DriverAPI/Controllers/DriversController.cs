using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AndreVehicles.DriverAPI.Data;
using Models.Insurances;
using Models.DTO.Insurance;
using Models.DTO.People;
using Models.People;
using Repositories;
using Services.People;

namespace AndreVehicles.DriverAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DriversController : ControllerBase
    {
        private readonly AndreVehiclesDriverAPIContext _context;

        public DriversController(AndreVehiclesDriverAPIContext context)
        {
            _context = context;
        }

        // GET: api/Drivers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Driver>>> GetDriver()
        {
          if (_context.Driver == null)
          {
              return NotFound();
          }
            return await _context.Driver.Include(l => l.License).Include(l => l.License.Category).Include(a => a.Address).ToListAsync();
        }

        // GET: api/Drivers/5
        [HttpGet("{document}")]
        public async Task<ActionResult<Driver>> GetDriver(string document)
        {
          if (_context.Driver == null)
          {
              return NotFound();
          }
            var driver = await _context.Driver.Include(l => l.License).Include(l => l.License.Category).Include(a => a.Address).FirstOrDefaultAsync(d => d.Document == document);

            if (driver == null)
            {
                return NotFound();
            }

            return driver;
        }

        // PUT: api/Drivers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDriver(string id, Driver driver)
        {
            if (id != driver.Document)
            {
                return BadRequest();
            }

            _context.Entry(driver).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DriverExists(id))
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

        // POST: api/Drivers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Driver>> PostDriver(DriverDTO driverDTO)
        {
            string cep = driverDTO.Address.PostalCode;
            Address address = await ApiConsume<Address>.Get("https://localhost:7020", $"/GetAddressByCep/{cep}");

            if (address == null)
                return BadRequest("Address not found.");


            address.PostalCode = driverDTO.Address.PostalCode;
            address.AdditionalInfo = driverDTO.Address.AdditionalInfo;
            address.Number = driverDTO.Address.Number;
            address.StreetType = driverDTO.Address.StreetType;

            Driver driver = new Driver()
            {
                Document = driverDTO.Document,
                Name = driverDTO.Name,
                BirthDate = driverDTO.BirthDate,
                Address = address,
                Phone = driverDTO.Phone,
                Email = driverDTO.Email,
                License = driverDTO.DriversLicense
            };

            address = new AddressService().PostMongo(address);

            if (address == null)
                return BadRequest("Failed to save address in MongoDB.");

            if (_context.Driver == null)
            {
                return Problem("Entity set 'AndreVehiclesDriverAPIContext.Driver'  is null.");
            }

            _context.Driver.Add(driver);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (DriverExists(driver.Document))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetDriver", new { id = driver.Document }, driver);
        }

        // DELETE: api/Drivers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDriver(string id)
        {
            if (_context.Driver == null)
            {
                return NotFound();
            }
            var driver = await _context.Driver.FindAsync(id);
            if (driver == null)
            {
                return NotFound();
            }

            _context.Driver.Remove(driver);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DriverExists(string id)
        {
            return (_context.Driver?.Any(e => e.Document == id)).GetValueOrDefault();
        }
    }
}
