using AndreVehicles.SaleAPI.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Models.Cars;
using Models.DTO.Sales;
using Models.People;
using Models.Sales;
using Repositories;
using Services.Cars;
using Services.People;
using Services.Sales;
using System.Diagnostics;

namespace AndreVehicles.SaleAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SalesController : ControllerBase
{
    private readonly AndreVehiclesSaleAPIContext _context;
    private readonly SaleService _service;

    private readonly CustomerService _customerService;
    private readonly EmployeeService _employeeService;
    private readonly CarService _carService;
    private readonly PaymentService _paymentService;

    public SalesController(AndreVehiclesSaleAPIContext context)
    {
        _context = context;
        _service = new SaleService();
        _customerService = new CustomerService();
        _employeeService = new EmployeeService();
        _carService = new CarService();
        _paymentService = new PaymentService();
    }


    [HttpGet("{technology}")]
    public async Task<ActionResult<IEnumerable<Sale>>> GetSale(string technology)
    {
        switch (technology)
        {
            case "entity":

                return await _context.Sale
                    .Include(c => c.Customer)
                    .Include(c => c.Customer.Address)
                    .Include(c => c.Employee)
                    .Include(c => c.Employee.Address)
                    .Include(c => c.Employee.Role)
                    .Include(p => p.Payment)
                    .Include(p => p.Payment.BankSlip)
                    .Include(p => p.Payment.Pix)
                    .Include(p => p.Payment.Pix.Type)
                    .Include(p => p.Payment.Card)
                    .ToListAsync();
            case "dapper":
            case "ado":
                var sales = _service.Get(technology);
                return sales == null || sales.Count == 0 ? NotFound() : sales;

            default:
                return BadRequest("Invalid technology. Valid values are: entity, dapper, ado");
        }
    }


    [HttpGet("{technology}/{id}")]
    public async Task<ActionResult<Sale>> GetSale(string technology, int id)
    {
        Sale? sale;
        switch (technology)
        {
            case "entity":
                if (_context.Sale == null)
                    return NotFound();

                sale = await _context.Sale.Include(c => c.Customer).Include(e => e.Employee).Include(p => p.Payment).SingleOrDefaultAsync(s => s.Id == id);

                return sale != null ? sale : NotFound();

            case "dapper":
            case "ado":
                sale = _service.Get(technology, id);
                return sale != null ? sale : NotFound();

            default:
                return BadRequest("Invalid technology. Valid values are: entity, dapper, ado");
        }
    }


    [HttpPost("{technology}")]
    public async Task<ActionResult<Sale>> PostSale(string technology, SaleDTO saleDTO)
    {
        Sale sale;
        Car? car;
        Employee? employee;
        Customer? customer;
        Payment? payment;


        switch (technology)
        {
            case "entity":

                var employeeTask = ApiConsume<Employee>.Get("https://localhost:7296/api/Employees/", $"entity/{saleDTO.EmployeeDocument}");
                var customerTask = ApiConsume<Customer>.Get("https://localhost:7063/api/Customers/", $"entity/{saleDTO.CustomerDocument}");
                var paymentTask = ApiConsume<Payment>.Get("https://localhost:7255/api/Payments/", $"entity/{saleDTO.PaymentId}");
                var carTask = ApiConsume<Car>.Get("https://localhost:7274/api/Cars/", $"entity/{saleDTO.CarPlate}");

                Task.WaitAll(employeeTask, customerTask, paymentTask, carTask);

                employee = employeeTask.Result;
                customer = customerTask.Result;
                payment = paymentTask.Result;
                car = carTask.Result;


                if (car == null || employee == null || customer == null || payment == null)
                    return BadRequest("Invalid car, employee, customer or payment.");

                sale = new Sale
                {
                    Car = car,
                    Employee = employee,
                    Customer = customer,
                    Payment = payment,
                    SaleDate = saleDTO.SaleDate,
                    SalePrice = saleDTO.SalePrice
                };


                var parameters = new[] {
                    new SqlParameter("@CustomerDocument", sale.Customer.Document),
                    new SqlParameter("@EmployeeDocument", sale.Employee.Document),
                    new SqlParameter("@CarPlate", sale.Car.Plate),
                    new SqlParameter("@PaymentId", sale.Payment.Id),
                    new SqlParameter("@SaleDate", sale.Customer.Document),
                    new SqlParameter("@SalePrice", sale.SalePrice)
                };

                int sucess = await _context.Database.ExecuteSqlRawAsync(Sale.POST, parameters);


                if (sucess > 0)
                    return CreatedAtAction("GetSale", new { technology, id = sale.Id }, sale);
                else
                    return BadRequest();

            case "dapper" or "ado":

                var employeeT = ApiConsume<Employee>.Get("https://localhost:7296/api/Employees/", $"{technology}/{saleDTO.EmployeeDocument}");
                var customerT = ApiConsume<Customer>.Get("https://localhost:7063/api/Customers/", $"{technology}/{saleDTO.CustomerDocument}");
                var paymentT = ApiConsume<Payment>.Get("https://localhost:7255/api/Payments/", $"{technology}/{saleDTO.PaymentId}");
                var carT = ApiConsume<Car>.Get("https://localhost:7274/api/Cars/", $"{technology}/{saleDTO.CarPlate}");

                Task.WaitAll(employeeT, customerT, paymentT, carT);

                employee = employeeT.Result;
                customer = customerT.Result;
                payment = paymentT.Result;
                car = carT.Result;


                if (car == null || employee == null || customer == null || payment == null)
                    return BadRequest("Invalid car, employee, customer or payment.");

                sale = new Sale
                {
                    Car = car,
                    Employee = employee,
                    Customer = customer,
                    Payment = payment,
                    SaleDate = saleDTO.SaleDate,
                    SalePrice = saleDTO.SalePrice
                };
                bool success = _service.Post(technology, sale);
                return success ? CreatedAtAction("GetSale", new { technology, id = sale.Id }, sale) : BadRequest();

            default:
                return BadRequest("Invalid technology. Valid values are: entity, dapper, ado");
        }
    }



    [HttpGet("/GetSaleById/{technology}/{id}")]
    public Sale? GetSaleById(string technology, int id)
    {
        var sale = _service.Get(technology, id);
        return sale;
    }






    private async Task<Customer> GetCustomer(string document)
    {
        Customer? customer;

        try
        {
            using HttpClient client = new();
            client.BaseAddress = new Uri("https://localhost:7063/api/Customers/");

            HttpResponseMessage response = await client.GetAsync($"entity/{document}");

            response.EnsureSuccessStatusCode();
            customer = await response.Content.ReadFromJsonAsync<Customer>();
        }
        catch (Exception)
        {
            return null;
        }

        if (customer == null)
            return null;

        return customer;
    }

    private async Task<Employee?> GetEmployee(string document)
    {
        Employee? employee;

        try
        {
            using HttpClient client = new();
            client.BaseAddress = new Uri("https://localhost:7296/api/Employees/");

            HttpResponseMessage response = await client.GetAsync($"entity/{document}");

            response.EnsureSuccessStatusCode();
            employee = await response.Content.ReadFromJsonAsync<Employee>();
        }
        catch (Exception)
        {
            return null;
        }

        if (employee == null)
            return null;

        return employee;
    }



}
