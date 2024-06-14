using Microsoft.Data.SqlClient;
using Models.Cars;
using Models.People;
using Models.Sales;
using Repositories.Cars;
using Repositories.People;
using System.Configuration;
using System.Data;

namespace Repositories.Sales;

public class SaleRepository
{
    private string _connectionString;
    private AdoUtilsRepository _adoRepository;
    private CustomerRepository _customerRepository;
    private EmployeeRepository _employeeRepository;
    private CarRepository _carRepository;
    private PaymentRepository _paymentRepository;



    public SaleRepository()
    {
        _connectionString = "Data Source=127.0.0.1; Initial Catalog=DBAndreVehicles; User Id=sa; Password=SqlServer2019!; TrustServerCertificate=Yes";
        _adoRepository = new();
        _customerRepository = new();
        _employeeRepository = new();
        _carRepository = new();
        _paymentRepository = new();
    }

    public List<Sale> Get(string technology)
    {
        if (technology.Equals("dapper"))
        {
            var list = new List<Sale>();

            foreach (var row in DapperUtilsRepository<dynamic>.GetAll(Sale.GETALL))
            {
                var customer = _customerRepository.Get("dapper", row.CustomerDocument);
                var employee = _employeeRepository.Get("dapper", row.EmployeeDocument);
                var car = _carRepository.Get("dapper", row.CarPlate);
                var payment = _paymentRepository.Get("dapper", row.PaymentId);

                list.Add(new Sale(row.Id, customer, employee, car, payment, row.SaleDate, row.SalePrice));
            }
            return list;
        }
        else if (technology.Equals("ado"))
        {
            var list = new List<Sale>();
            var data = _adoRepository.ExecuteReader(Sale.GETALL, null).Result;

            foreach (DataRow row in data.Rows)
            {
                var customer = new CustomerRepository().Get("ado", row["CustomerDocument"].ToString());
                var employee = new EmployeeRepository().Get("ado", row["EmployeeDocument"].ToString());
                var car = new CarRepository().Get("ado", row["CarPlate"].ToString());
                var payment = new PaymentRepository().Get("ado", (int)row["PaymentId"]);

                list.Add(new Sale((int)row["Id"], customer, employee, car, payment, (DateTime)row["SaleDate"], (decimal)row["SalePrice"]));
            }
            return list;
        }
        return null;
    }

    public Sale Get(string technology, int id)
    {
        if (technology.Equals("dapper"))
        {
            var row = DapperUtilsRepository<dynamic>.Get(Sale.GET, new { Id = id });

            var customer = _customerRepository.Get("dapper", row.CustomerDocument);
            var employee = _employeeRepository.Get("dapper", row.EmployeeDocument);
            var car = _carRepository.Get("dapper", row.CarPlate);
            var payment = _paymentRepository.Get("dapper", row.PaymentId);

            return new Sale(row.Id, customer, employee, car, payment, row.SaleDate, row.SalePrice);
        }
        else if (technology.Equals("ado"))
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@Id", id)
            };

            var row = _adoRepository.ExecuteReader(Sale.GET, parameters).Result.Rows[0];

            var customer = new CustomerRepository().Get("ado", row["CustomerDocument"].ToString());
            var employee = new EmployeeRepository().Get("ado", row["EmployeeDocument"].ToString());
            var car = new CarRepository().Get("ado", row["CarPlate"].ToString());
            var payment = new PaymentRepository().Get("ado", (int)row["PaymentId"]);

            return new Sale((int)row["Id"], customer, employee, car, payment, (DateTime)row["SaleDate"], (decimal)row["SalePrice"]);
        }

        return null;
    }

    public bool Post(string technology, Sale sale)
    {
        if (technology.Equals("dapper"))
        {
            object obj = new
            {
                CustomerDocument = sale.Customer.Document,
                EmployeeDocument = sale.Employee.Document,
                CarPlate = sale.Car.Plate,
                PaymentId = sale.Payment.Id,
                SaleDate = sale.SaleDate,
                SalePrice = sale.SalePrice
            };

            return DapperUtilsRepository<Sale>.Insert(Sale.POST, obj);
        }
        else if (technology.Equals("ado"))
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@CustomerDocument", sale.Customer.Document),
                new SqlParameter("@EmployeeDocument", sale.Employee.Document),
                new SqlParameter("@CarPlate", sale.Car.Plate),
                new SqlParameter("@PaymentId", sale.Payment.Id),
                new SqlParameter("@SaleDate", sale.SaleDate),
                new SqlParameter("@SalePrice", sale.SalePrice)
            };

            return _adoRepository.ExecuteNonQuery(Sale.POST, parameters).Result > 0;
        }
        return false;
    }

}



