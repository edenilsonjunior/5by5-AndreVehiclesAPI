using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AndreVehicles.InsuranceAPI.Data;
using Models.Insurances;
using Models.DTO.Insurance;
using Models.People;
using Repositories;
using Models.Cars;
using Repositories.Insurances;

namespace AndreVehicles.InsuranceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InsurancesController : ControllerBase
    {
        private readonly AndreVehiclesInsuranceAPIContext _context;

        public InsurancesController(AndreVehiclesInsuranceAPIContext context)
        {
            _context = context;
        }

        // GET: api/Insurances
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Insurance>>> GetInsurance()
        {
            var insurance = new InsuranceRepository().Get();

            if (insurance == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(insurance);
            }
        }

        // GET: api/Insurances/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Insurance>> GetInsurance(int id)
        {
            var insurance = new InsuranceRepository().Get(id);

            if(insurance == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(insurance);
            }
        }

        // PUT: api/Insurances/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutInsurance(int id, Insurance insurance)
        {
            if (id != insurance.Id)
            {
                return BadRequest();
            }

            _context.Entry(insurance).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InsuranceExists(id))
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

        // POST: api/Insurances
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Insurance>> PostInsurance(InsuranceDTO insuranceDTO)
        {
            Customer costumer = await ApiConsume<Customer>.Get("https://localhost:7063/api/Customers/entity/", $"{insuranceDTO.CustomerDocument}");

            if (costumer == null)
                return BadRequest("Customer not found.");

            Car car = await ApiConsume<Car>.Get("https://localhost:7274/api/Cars/entity/", $"{insuranceDTO.CarPlate}");

            if (car == null)
                return BadRequest("Car not found.");

            Driver driver = await ApiConsume<Driver>.Get("https://localhost:7194/api/Drivers/", $"{insuranceDTO.MainDriverDocument}");

            if (driver == null)
                return BadRequest("Driver not found.");

            Insurance insurance = new Insurance()
            {
                Customer = costumer,
                Deductible = insuranceDTO.Deductible,
                Car = car,
                MainDriver = driver
            };

            InsuranceRepository insuranceRepository = new InsuranceRepository();

            
            if(insuranceRepository.Post(insurance))
                return CreatedAtAction("GetInsurance", new { id = insurance.Id }, insurance);
            else
                return BadRequest();
        }

        // DELETE: api/Insurances/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInsurance(int id)
        {
            if (_context.Insurance == null)
            {
                return NotFound();
            }
            var insurance = await _context.Insurance.FindAsync(id);
            if (insurance == null)
            {
                return NotFound();
            }

            _context.Insurance.Remove(insurance);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool InsuranceExists(int id)
        {
            return (_context.Insurance?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
