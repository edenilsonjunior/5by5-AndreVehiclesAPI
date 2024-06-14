using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Models.Insurances;

namespace AndreVehicles.InsuranceAPI.Data
{
    public class AndreVehiclesInsuranceAPIContext : DbContext
    {
        public AndreVehiclesInsuranceAPIContext (DbContextOptions<AndreVehiclesInsuranceAPIContext> options)
            : base(options)
        {
        }

        public DbSet<Models.Insurances.Insurance> Insurance { get; set; } = default!;
    }
}
