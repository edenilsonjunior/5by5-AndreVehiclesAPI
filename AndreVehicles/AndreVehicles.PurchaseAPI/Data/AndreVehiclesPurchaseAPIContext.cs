using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Models.Cars;

namespace AndreVehicles.PurchaseAPI.Data
{
    public class AndreVehiclesPurchaseAPIContext : DbContext
    {
        public AndreVehiclesPurchaseAPIContext (DbContextOptions<AndreVehiclesPurchaseAPIContext> options)
            : base(options)
        {
        }

        public DbSet<Models.Cars.Purchase> Purchase { get; set; } = default!;
        public DbSet<Models.Cars.Car> Car { get; set; } = default!;
    }
}
