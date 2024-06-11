using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Models.People;

namespace AndreVehicles.AddressAPI.Data
{
    public class AndreVehiclesAddressAPIContext : DbContext
    {
        public AndreVehiclesAddressAPIContext (DbContextOptions<AndreVehiclesAddressAPIContext> options)
            : base(options)
        {
        }

        public DbSet<Models.People.Address> Address { get; set; } = default!;
    }
}
