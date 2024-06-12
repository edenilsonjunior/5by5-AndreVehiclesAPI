using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AndreVehicles.FinancialPendingAPI.Data;
using Models.Financials;
using Models.DTO.Financials;
using Models.People;
using System.Runtime.ConstrainedExecution;
using Models.DTO.People;
using Services.People;

namespace AndreVehicles.FinancialPendingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FinancialPendingsController : ControllerBase
    {
        private readonly AndreVehiclesFinancialPendingAPIContext _context;

        public FinancialPendingsController(AndreVehiclesFinancialPendingAPIContext context)
        {
            _context = context;
        }

        // GET: api/FinancialPendings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FinancialPending>>> GetFinancialPending()
        {
          if (_context.FinancialPendings == null)
          {
              return NotFound();
          }
            return await _context.FinancialPendings.ToListAsync();
        }

        // GET: api/FinancialPendings/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FinancialPending>> GetFinancialPending(int id)
        {
          if (_context.FinancialPendings == null)
          {
              return NotFound();
          }
            var financialPending = await _context.FinancialPendings.FindAsync(id);

            if (financialPending == null)
            {
                return NotFound();
            }

            return financialPending;
        }

        // POST: api/FinancialPendings
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<FinancialPending>> PostFinancialPending(FinancialPendingDTO financialPendingDTO)
        {

            FinancialPending financialPending = new()
            {
                Description = financialPendingDTO.Description,
                Customer =  await GetCustomer(financialPendingDTO.CustomerDocument),
                Price = financialPendingDTO.Price,
                FinancialPendingDate = financialPendingDTO.FinancialPendingDate,
                PaymentDate = financialPendingDTO.PaymentDate,
                Status = financialPendingDTO.Status
            };

            if (_context.FinancialPendings == null)
              {
                  return Problem("Entity set 'AndreVehiclesFinancialPendingAPIContext.FinancialPending'  is null.");
              }
            _context.FinancialPendings.Add(financialPending);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFinancialPending", new { id = financialPending.Id }, financialPending);
        }

        private async Task<Customer> GetCustomer(string document)
        {
            Customer? customer;

            try
            {
                using HttpClient client = new();
                client.BaseAddress = new Uri("https://localhost:7063/api/Customers/entity");

                HttpResponseMessage response = await client.GetAsync($"entity/{document}");

                response.EnsureSuccessStatusCode();
                customer =  await response.Content.ReadFromJsonAsync<Customer>();
            }
            catch (Exception)
            {
                return null;
            }

            if (customer == null)
                return null;

            return customer;
        }
    }
}
