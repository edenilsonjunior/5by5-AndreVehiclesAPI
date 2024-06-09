using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Models.Sales;

namespace AndreVehicles.SaleAPI.Data
{
    public class AndreVehiclesSaleAPIContext : DbContext
    {
        public AndreVehiclesSaleAPIContext (DbContextOptions<AndreVehiclesSaleAPIContext> options)
            : base(options)
        {
        }

        public DbSet<Models.Sales.Sale> Sale { get; set; } = default!;
    }
}
