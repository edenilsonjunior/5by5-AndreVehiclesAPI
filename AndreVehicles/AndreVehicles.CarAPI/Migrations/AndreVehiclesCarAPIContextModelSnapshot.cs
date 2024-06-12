﻿// <auto-generated />
using System;
using AndreVehicles.CarAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace AndreVehicles.CarAPI.Migrations
{
    [DbContext(typeof(AndreVehiclesCarAPIContext))]
    partial class AndreVehiclesCarAPIContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.29")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Models.Cars.Car", b =>
                {
                    b.Property<string>("Plate")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Color")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Sold")
                        .HasColumnType("bit");

                    b.Property<int>("YearManufacture")
                        .HasColumnType("int");

                    b.Property<int>("YearModel")
                        .HasColumnType("int");

                    b.HasKey("Plate");

                    b.ToTable("Car", (string)null);
                });

            modelBuilder.Entity("Models.Cars.CarOperation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("CarPlate")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("OperationId")
                        .HasColumnType("int");

                    b.Property<bool>("Status")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("CarPlate");

                    b.HasIndex("OperationId");

                    b.ToTable("CarOperation", (string)null);
                });

            modelBuilder.Entity("Models.Cars.Operation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Operation", (string)null);
                });

            modelBuilder.Entity("Models.Cars.Purchase", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("CarPlate")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("PurchaseDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("CarPlate");

                    b.ToTable("Purchase", (string)null);
                });

            modelBuilder.Entity("Models.Financials.FinancialPending", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("CustomerDocument")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("FinancialPendingDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("PaymentDate")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<bool>("Status")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("CustomerDocument");

                    b.ToTable("FinancialPending", (string)null);
                });

            modelBuilder.Entity("Models.People.Address", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("AdditionalInfo")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("District")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Number")
                        .HasColumnType("int");

                    b.Property<string>("PostalCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("State")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Street")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("StreetType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("_id")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Address", (string)null);
                });

            modelBuilder.Entity("Models.People.Person", b =>
                {
                    b.Property<string>("Document")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AddressId")
                        .HasColumnType("int");

                    b.Property<DateTime>("BirthDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Document");

                    b.HasIndex("AddressId");

                    b.ToTable("Person", (string)null);
                });

            modelBuilder.Entity("Models.People.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Role", (string)null);
                });

            modelBuilder.Entity("Models.Sales.BankSlip", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<DateTime>("DueDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("Number")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("BankSlip", (string)null);
                });

            modelBuilder.Entity("Models.Sales.Card", b =>
                {
                    b.Property<string>("CardNumber")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("CardHolderName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ExpirationDate")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SecurityCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CardNumber");

                    b.ToTable("Card", (string)null);
                });

            modelBuilder.Entity("Models.Sales.Payment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int?>("BankSlipId")
                        .HasColumnType("int");

                    b.Property<string>("CardNumber")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("PaymentDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("PixId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("BankSlipId");

                    b.HasIndex("CardNumber");

                    b.HasIndex("PixId");

                    b.ToTable("Payment", (string)null);
                });

            modelBuilder.Entity("Models.Sales.Pix", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("PixKey")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TypeId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("TypeId");

                    b.ToTable("Pix", (string)null);
                });

            modelBuilder.Entity("Models.Sales.PixType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("PixType", (string)null);
                });

            modelBuilder.Entity("Models.Sales.Sale", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("CarPlate")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("CustomerDocument")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("EmployeeDocument")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("PaymentId")
                        .HasColumnType("int");

                    b.Property<DateTime>("SaleDate")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("SalePrice")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Id");

                    b.HasIndex("CarPlate");

                    b.HasIndex("CustomerDocument");

                    b.HasIndex("EmployeeDocument");

                    b.HasIndex("PaymentId");

                    b.ToTable("Sale", (string)null);
                });

            modelBuilder.Entity("Models.People.Customer", b =>
                {
                    b.HasBaseType("Models.People.Person");

                    b.Property<decimal>("Income")
                        .HasColumnType("decimal(18,2)");

                    b.ToTable("Customer", (string)null);
                });

            modelBuilder.Entity("Models.People.Employee", b =>
                {
                    b.HasBaseType("Models.People.Person");

                    b.Property<decimal>("Commission")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("CommissionValue")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.HasIndex("RoleId");

                    b.ToTable("Employee", (string)null);
                });

            modelBuilder.Entity("Models.Cars.CarOperation", b =>
                {
                    b.HasOne("Models.Cars.Car", "Car")
                        .WithMany()
                        .HasForeignKey("CarPlate")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Models.Cars.Operation", "Operation")
                        .WithMany()
                        .HasForeignKey("OperationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Car");

                    b.Navigation("Operation");
                });

            modelBuilder.Entity("Models.Cars.Purchase", b =>
                {
                    b.HasOne("Models.Cars.Car", "Car")
                        .WithMany()
                        .HasForeignKey("CarPlate")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Car");
                });

            modelBuilder.Entity("Models.Financials.FinancialPending", b =>
                {
                    b.HasOne("Models.People.Customer", "Customer")
                        .WithMany()
                        .HasForeignKey("CustomerDocument")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("Models.People.Person", b =>
                {
                    b.HasOne("Models.People.Address", "Address")
                        .WithMany()
                        .HasForeignKey("AddressId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Address");
                });

            modelBuilder.Entity("Models.Sales.Payment", b =>
                {
                    b.HasOne("Models.Sales.BankSlip", "BankSlip")
                        .WithMany()
                        .HasForeignKey("BankSlipId");

                    b.HasOne("Models.Sales.Card", "Card")
                        .WithMany()
                        .HasForeignKey("CardNumber");

                    b.HasOne("Models.Sales.Pix", "Pix")
                        .WithMany()
                        .HasForeignKey("PixId");

                    b.Navigation("BankSlip");

                    b.Navigation("Card");

                    b.Navigation("Pix");
                });

            modelBuilder.Entity("Models.Sales.Pix", b =>
                {
                    b.HasOne("Models.Sales.PixType", "Type")
                        .WithMany()
                        .HasForeignKey("TypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Type");
                });

            modelBuilder.Entity("Models.Sales.Sale", b =>
                {
                    b.HasOne("Models.Cars.Car", "Car")
                        .WithMany()
                        .HasForeignKey("CarPlate")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Models.People.Customer", "Customer")
                        .WithMany()
                        .HasForeignKey("CustomerDocument")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Models.People.Employee", "Employee")
                        .WithMany()
                        .HasForeignKey("EmployeeDocument")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Models.Sales.Payment", "Payment")
                        .WithMany()
                        .HasForeignKey("PaymentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Car");

                    b.Navigation("Customer");

                    b.Navigation("Employee");

                    b.Navigation("Payment");
                });

            modelBuilder.Entity("Models.People.Customer", b =>
                {
                    b.HasOne("Models.People.Person", null)
                        .WithOne()
                        .HasForeignKey("Models.People.Customer", "Document")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Models.People.Employee", b =>
                {
                    b.HasOne("Models.People.Person", null)
                        .WithOne()
                        .HasForeignKey("Models.People.Employee", "Document")
                        .OnDelete(DeleteBehavior.ClientCascade)
                        .IsRequired();

                    b.HasOne("Models.People.Role", "Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");
                });
#pragma warning restore 612, 618
        }
    }
}
