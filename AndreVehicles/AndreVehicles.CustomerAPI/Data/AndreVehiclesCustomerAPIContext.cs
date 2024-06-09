using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Models.People;

namespace AndreVehicles.CustomerAPI.Data
{
    public class AndreVehiclesCustomerAPIContext : DbContext
    {
        public AndreVehiclesCustomerAPIContext (DbContextOptions<AndreVehiclesCustomerAPIContext> options)
            : base(options)
        {
        }

        public DbSet<Models.People.Customer> Customer { get; set; } = default!;
    }
}
