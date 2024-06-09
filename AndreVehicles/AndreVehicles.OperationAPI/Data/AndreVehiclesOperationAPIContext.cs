using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Models.Cars;

namespace AndreVehicles.OperationAPI.Data
{
    public class AndreVehiclesOperationAPIContext : DbContext
    {
        public AndreVehiclesOperationAPIContext (DbContextOptions<AndreVehiclesOperationAPIContext> options)
            : base(options)
        {
        }

        public DbSet<Models.Cars.Operation> Operation { get; set; } = default!;
    }
}
