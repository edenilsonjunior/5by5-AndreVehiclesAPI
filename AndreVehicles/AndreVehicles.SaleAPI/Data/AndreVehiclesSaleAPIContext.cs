using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Models.People;
using Models.Sales;

namespace AndreVehicles.SaleAPI.Data
{
    public class AndreVehiclesSaleAPIContext : DbContext
    {
        public AndreVehiclesSaleAPIContext (DbContextOptions<AndreVehiclesSaleAPIContext> options)
            : base(options)
        {
        }

        public DbSet<Models.Sales.Sale> Sale { get; set; } = default!;
        public DbSet<Models.People.Customer> Customer { get; set; } = default!;
        public DbSet<Models.People.Employee> Employee { get; set; } = default!;
        public DbSet<Models.Cars.Car> Car { get; set; } = default!;
        public DbSet<Models.Sales.Payment> Payment { get; set; } = default!;


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Data Source=127.0.0.1; Initial Catalog=DBAndreVehiclesAPI; User Id=sa; Password=SqlServer2019!; TrustServerCertificate=Yes");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Customer>().HasBaseType<Person>();
            modelBuilder.Entity<Customer>().ToTable("Customer");

            modelBuilder.Entity<Employee>().HasBaseType<Person>();
            modelBuilder.Entity<Employee>().ToTable("Employee");
        }


    }
}

