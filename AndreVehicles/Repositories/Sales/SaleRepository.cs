using Microsoft.Data.SqlClient;
using Models.Cars;
using Models.People;
using Models.Sales;
using Repositories.Cars;
using Repositories.People;
using System.Configuration;

namespace Repositories.Sales;

public class SaleRepository
{
    private string _connectionString;

    public SaleRepository()
    {
        _connectionString = "Data Source=127.0.0.1; Initial Catalog=DBAndreVehicles; User Id=sa; Password=SqlServer2019!; TrustServerCertificate=Yes";
    }

    public bool Insert(Sale sale)
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

        return DapperUtilsRepository<Sale>.Insert(Sale.POST, sale);
    }



    public List<Sale> Get(string technology)
    {
        throw new NotImplementedException();
    }

    public Sale Get(string technology, int id)
    {
        throw new NotImplementedException();
    }

    public bool Post(string technology, Sale sale)
    {
        throw new NotImplementedException();
    }

}



