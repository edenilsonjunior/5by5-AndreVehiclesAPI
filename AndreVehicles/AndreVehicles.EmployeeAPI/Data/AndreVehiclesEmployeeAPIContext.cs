using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Models.People;

namespace AndreVehicles.EmployeeAPI.Data
{
    public class AndreVehiclesEmployeeAPIContext : DbContext
    {
        public AndreVehiclesEmployeeAPIContext (DbContextOptions<AndreVehiclesEmployeeAPIContext> options)
            : base(options)
        {
        }

        public DbSet<Models.People.Employee> Employee { get; set; } = default!;
    }
}
