using AndreVehicles.SaleAPI.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models.Cars;
using Models.DTO.Sales;
using Models.People;
using Models.Sales;
using Services.Cars;
using Services.People;
using Services.Sales;

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
                return _context.Sale == null ? NotFound() : await _context.Sale.Include(c => c.Customer).Include(e => e.Employee).Include(p => p.Payment).ToListAsync();

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
        Car car;
        Employee employee;
        Customer customer;
        Payment payment;

        switch (technology)
        {
            case "entity":

                car = _context.Car.FirstOrDefault(c => c.Plate == saleDTO.CarPlate);

                employee = _context.Employee.FirstOrDefault(e => e.Document == saleDTO.EmployeeDocument);

                customer = _context.Customer.FirstOrDefault(c => c.Document == saleDTO.CustomerDocument);
                payment = _context.Payment.FirstOrDefault(p => p.Id == saleDTO.PaymentId);

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

                _context.Sale.Add(sale);
                await _context.SaveChangesAsync();
                return CreatedAtAction("GetSale", new { id = sale.Id }, sale);

            case "dapper":
            case "ado":

                car = _carService.Get(technology, saleDTO.CarPlate);
                employee = _employeeService.Get(technology, saleDTO.EmployeeDocument);
                customer = _customerService.Get(technology, saleDTO.CustomerDocument);
                payment = _paymentService.Get(technology, saleDTO.PaymentId);

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
                return success ? CreatedAtAction("GetSale", new { id = sale.Id }, sale) : BadRequest();

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

}
