using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Models.Financials;
using Models.People;

namespace AndreVehicles.FinancialPendingAPI.Data
{
    public class AndreVehiclesFinancialPendingAPIContext : DbContext
    {
        public AndreVehiclesFinancialPendingAPIContext (DbContextOptions<AndreVehiclesFinancialPendingAPIContext> options)
            : base(options)
        {
        }

        public DbSet<Models.Financials.FinancialPending> FinancialPendings { get; set; } = default!;
        public DbSet<Models.People.Customer> Customer { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>().HasBaseType<Person>();
            modelBuilder.Entity<Customer>().ToTable("Customer");
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=127.0.0.1; Initial Catalog=DBAndreVehiclesAPI; User Id=sa; Password=SqlServer2019!; TrustServerCertificate=Yes");
        }
    }
}
