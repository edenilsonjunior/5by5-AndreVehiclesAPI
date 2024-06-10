using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Models.Sales;

namespace AndreVehicles.PaymentAPI.Data
{
    public class AndreVehiclesPaymentAPIContext : DbContext
    {
        public AndreVehiclesPaymentAPIContext (DbContextOptions<AndreVehiclesPaymentAPIContext> options)
            : base(options)
        {
        }

        public DbSet<Models.Sales.Payment> Payment { get; set; } = default!;

    }
}
