using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Models.Cars;
using Models.Financials;
using Models.People;
using Models.Sales;

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

            CreateCarsBuilder(modelBuilder);
            CreatePeopleBuilder(modelBuilder);
            CreateSalesBuilder(modelBuilder);

            modelBuilder.Entity<FinancialPending>(entity =>
            {
                entity.ToTable("FinancialPending");
                entity.HasKey(fp => fp.Id);

                entity.Property(fp => fp.Id)
                    .UseIdentityColumn(seed: 1, increment: 1);

                entity.HasOne(fp => fp.Customer)
                .WithMany()
                .HasForeignKey("CustomerDocument");
            });
        }

        private void CreateCarsBuilder(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Car>(entity =>
            {
                entity.ToTable("Car");
                entity.HasKey(c => c.Plate);
            });

            modelBuilder.Entity<Operation>(entity =>
            {
                entity.ToTable("Operation");
                entity.HasKey(o => o.Id);

                entity.Property(o => o.Id)
                    .UseIdentityColumn(seed: 1, increment: 1);
            });

            modelBuilder.Entity<CarOperation>(entity =>
            {
                entity.ToTable("CarOperation");
                entity.HasKey(co => co.Id);

                entity.Property(co => co.Id)
                    .UseIdentityColumn(seed: 1, increment: 1);
            });

            modelBuilder.Entity<Purchase>(entity =>
            {
                entity.ToTable("Purchase");
                entity.HasKey(p => p.Id);

                entity.Property(p => p.Id)
                    .UseIdentityColumn(seed: 1, increment: 1);
            });

        }


        private void CreatePeopleBuilder(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Address>(entity =>
            {
                entity.ToTable("Address");
                entity.HasKey(a => a.Id);
            });

            modelBuilder.Entity<Person>(entity =>
            {
                entity.ToTable("Person");
                entity.HasKey(p => p.Document);
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("Role");
                entity.HasKey(r => r.Id);

                entity.Property(r => r.Id)
                    .UseIdentityColumn(seed: 1, increment: 1);
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.ToTable("Employee");
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.ToTable("Customer");
            });

            modelBuilder.Entity<Dependent>(entity =>
            {
                entity.ToTable("Dependent");
            });

        }

        private void CreateSalesBuilder(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BankSlip>(entity =>
            {
                entity.ToTable("BankSlip");
                entity.HasKey(b => b.Id);

                entity.Property(b => b.Id)
                    .UseIdentityColumn(seed: 1, increment: 1);
            });

            modelBuilder.Entity<Card>(entity =>
            {
                entity.ToTable("Card");
                entity.HasKey(c => c.CardNumber);
            });

            modelBuilder.Entity<PixType>(entity =>
            {
                entity.ToTable("PixType");
                entity.HasKey(pt => pt.Id);
                entity.Property(pt => pt.Id)
                    .UseIdentityColumn(seed: 1, increment: 1);
            });

            modelBuilder.Entity<Pix>(entity =>
            {
                entity.ToTable("Pix");
                entity.HasKey(p => p.Id);

                entity.Property(p => p.Id)
                    .UseIdentityColumn(seed: 1, increment: 1);
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.ToTable("Payment");
                entity.HasKey(p => p.Id);

                entity.Property(p => p.Id)
                    .UseIdentityColumn(seed: 1, increment: 1);
            });

            modelBuilder.Entity<Sale>(entity =>
            {
                entity.ToTable("Sale");
                entity.HasKey(s => s.Id);

                entity.Property(s => s.Id)
                    .UseIdentityColumn(seed: 1, increment: 1);
            });
        }

    }
}
