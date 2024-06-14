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
using Microsoft.Data.SqlClient;

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
            if (_context.FinancialPending == null)
            {
                return NotFound();
            }
            return await _context.FinancialPending
                .Include(f => f.Customer)
                .Include(f => f.Customer.Address)
                .ToListAsync();
        }

        // GET: api/FinancialPendings/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FinancialPending>> GetFinancialPending(int id)
        {
            if (_context.FinancialPending == null)
            {
                return NotFound();
            }
            var financialPending = await _context.FinancialPending
                                .Include(f => f.Customer)
                                .Include(f => f.Customer.Address)
                                .FirstOrDefaultAsync(f => f.Id == id);

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

            Customer customer = await GetCustomer(financialPendingDTO.CustomerDocument);

            if (customer == null)
            {
                return NotFound();
            }

            FinancialPending financialPending = new()
            {
                Description = financialPendingDTO.Description,
                Customer = customer,
                Price = financialPendingDTO.Price,
                FinancialPendingDate = financialPendingDTO.FinancialPendingDate,
                PaymentDate = financialPendingDTO.PaymentDate,
                Status = financialPendingDTO.Status
            };

            if (_context.FinancialPending == null)
            {
                return Problem("Entity set 'AndreVehiclesFinancialPendingAPIContext.FinancialPending'  is null.");
            }

            var parameters = new[]
            {
                new SqlParameter("@Description", financialPending.Description),
                new SqlParameter("@CustomerDocument", financialPending.Customer.Document),
                new SqlParameter("@Price", financialPending.Price),
                new SqlParameter("@FinancialPendingDate", financialPending.FinancialPendingDate),
                new SqlParameter("@PaymentDate", financialPending.PaymentDate),
                new SqlParameter("@Status", financialPending.Status)
            };

            int sucess = await _context.Database.ExecuteSqlRawAsync(FinancialPending.POST, parameters);

            if (sucess > 0)
            {
                return CreatedAtAction("GetFinancialPending", new { id = financialPending.Id }, financialPending);
            }
            else
            {
                return BadRequest();
            }
        }

        private async Task<Customer> GetCustomer(string document)
        {
            Customer? customer;

            try
            {
                using HttpClient client = new();
                client.BaseAddress = new Uri("https://localhost:7063/api/Customers/");

                HttpResponseMessage response = await client.GetAsync($"entity/{document}");

                response.EnsureSuccessStatusCode();
                customer = await response.Content.ReadFromJsonAsync<Customer>();
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
