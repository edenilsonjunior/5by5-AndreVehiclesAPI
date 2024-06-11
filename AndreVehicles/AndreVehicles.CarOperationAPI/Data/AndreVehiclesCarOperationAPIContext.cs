using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Models.Cars;

namespace AndreVehicles.CarOperationAPI.Data
{
    public class AndreVehiclesCarOperationAPIContext : DbContext
    {
        public AndreVehiclesCarOperationAPIContext (DbContextOptions<AndreVehiclesCarOperationAPIContext> options)
            : base(options)
        {
        }

        public DbSet<Models.Cars.Car> Car { get; set; } = default!;
        public DbSet<Models.Cars.Operation> Operation { get; set; } = default!;
        public DbSet<Models.Cars.CarOperation> CarOperation { get; set; } = default!;
    }
}
