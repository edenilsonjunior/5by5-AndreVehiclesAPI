using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Models.Financials;

namespace AndreVehicles.BankSql.Data
{
    public class AndreVehiclesBankSqlContext : DbContext
    {
        public AndreVehiclesBankSqlContext (DbContextOptions<AndreVehiclesBankSqlContext> options)
            : base(options)
        {
        }

        public DbSet<Models.Financials.Bank> Bank { get; set; } = default!;
    }
}
