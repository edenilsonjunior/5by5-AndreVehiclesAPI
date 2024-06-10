using Microsoft.EntityFrameworkCore;
using Models.People;

namespace AndreVehicles.EmployeeAPI.Data
{
    public class AndreVehiclesEmployeeAPIContext : DbContext
    {
        public AndreVehiclesEmployeeAPIContext(DbContextOptions<AndreVehiclesEmployeeAPIContext> options)
            : base(options)
        {
        }

        public DbSet<Models.People.Employee> Employee { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Person>()
                .ToTable("Person")
                .HasKey(p => p.Document);

            modelBuilder.Entity<Employee>().ToTable("Employee");
        }
    }
}
