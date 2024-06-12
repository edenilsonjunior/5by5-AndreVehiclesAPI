using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Models.Financials;

namespace AndreVehicles.FinancialPendingAPI.Data
{
    public class AndreVehiclesFinancialPendingAPIContext : DbContext
    {
        public AndreVehiclesFinancialPendingAPIContext (DbContextOptions<AndreVehiclesFinancialPendingAPIContext> options)
            : base(options)
        {
        }

        public DbSet<Models.Financials.FinancialPending> FinancialPendings { get; set; } = default!;
        public DbSet<Models.People.Customer> Customer { get; set; } = default!;
    }
}
