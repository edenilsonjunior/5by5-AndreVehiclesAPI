using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Models.Cars;

namespace AndreVehicles.CarAPI.Data
{
    public class AndreVehiclesCarAPIContext : DbContext
    {
        public AndreVehiclesCarAPIContext(DbContextOptions<AndreVehiclesCarAPIContext> options)
            : base(options)
        {
        }

        public DbSet<Models.Cars.Car> Car { get; set; } = default!;


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
