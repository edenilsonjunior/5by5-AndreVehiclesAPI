using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Models.Insurances;
using Models.People;

namespace AndreVehicles.DriverAPI.Data
{
    public class AndreVehiclesDriverAPIContext : DbContext
    {
        public AndreVehiclesDriverAPIContext (DbContextOptions<AndreVehiclesDriverAPIContext> options)
            : base(options)
        {
        }

        public DbSet<Models.Insurances.Driver> Driver { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Driver>().HasBaseType<Person>();

            modelBuilder.Entity<Driver>().ToTable("Driver");
        }
    }
}
