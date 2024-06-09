using Microsoft.Data.SqlClient;
using Models.Cars;
using System.Configuration;

namespace Repositories.Cars;

public class CarRepository
{
    private string _connectionString;

    public CarRepository()
    {
        _connectionString = "Data Source=127.0.0.1; Initial Catalog=DBAndreVehiclesAPI; User Id=sa; Password=SqlServer2019!; TrustServerCertificate=Yes";
    }

    public List<Car> Get(string technology)
    {
        if (technology.Equals("dapper"))
            return DapperUtilsRepository<Car>.GetAll(Car.GETALL);

        if (technology.Equals("ado"))
        {
            List<Car> list = new List<Car>();

            //    public readonly static string SELECT = "SELECT Plate, Name, YearManufacture, YearModel, Color, Sold FROM Car";

            try
            {
                using SqlConnection connection = new(_connectionString);

                connection.Open();

                using SqlCommand command = new(Car.GETALL, connection);

                using SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Car car = new Car
                    {
                        Plate = reader["Plate"].ToString(),
                        Name = reader["Name"].ToString(),
                        YearManufacture = Convert.ToInt32(reader["YearManufacture"]),
                        YearModel = Convert.ToInt32(reader["YearModel"]),
                        Color = reader["Color"].ToString(),
                        Sold = Convert.ToBoolean(reader["Sold"])
                    };

                    list.Add(car);
                }
            }
            catch (Exception)
            {
                return null;
            }

            return list;
        }

        return null;
    }

    public Car Get(string technology, string plate)
    {
        if (technology.Equals("dapper"))
            return DapperUtilsRepository<Car>.Get(Car.GET, new { Plate = plate });


        if (technology.Equals("ado"))
        {
            Car car = new();
            try
            {
                using SqlConnection connection = new(_connectionString);

                connection.Open();

                using SqlCommand command = new(Car.GET, connection);

                command.Parameters.AddWithValue("@Plate", plate);

                using SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    car.Plate = reader["Plate"].ToString();
                    car.Name = reader["Name"].ToString();
                    car.YearManufacture = Convert.ToInt32(reader["YearManufacture"]);
                    car.YearModel = Convert.ToInt32(reader["YearModel"]);
                    car.Color = reader["Color"].ToString();
                    car.Sold = Convert.ToBoolean(reader["Sold"]);
                }
            }
            catch (Exception)
            {
                return null;
            }

            return car;
        }

        return null;
    }

    public bool Post(string technology, Car car)
    {
        if (technology.Equals("dapper"))
        {
            object obj = new
            {
                car.Plate,
                car.Name,
                car.YearManufacture,
                car.YearModel,
                car.Color,
                car.Sold
            };
            return DapperUtilsRepository<Car>.Insert(Car.POST, obj);
        }

        if (technology.Equals("ado"))
        {
            try
            {
                using SqlConnection connection = new(_connectionString);

                connection.Open();

                using SqlCommand command = new(Car.POST, connection);

                command.Parameters.AddWithValue("@Plate", car.Plate);
                command.Parameters.AddWithValue("@Name", car.Name);
                command.Parameters.AddWithValue("@YearManufacture", car.YearManufacture);
                command.Parameters.AddWithValue("@YearModel", car.YearModel);
                command.Parameters.AddWithValue("@Color", car.Color);
                command.Parameters.AddWithValue("@Sold", car.Sold);

                command.ExecuteNonQuery();
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }
        return false;
    }

}
