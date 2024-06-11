using Microsoft.Data.SqlClient;
using Models.Cars;
using System.Configuration;

namespace Repositories.Cars;

public class CarOperationRepository
{
    private string _connectionString;

    public CarOperationRepository()
    {
        _connectionString = "Data Source=127.0.0.1; Initial Catalog=DBAndreVehicles; User Id=sa; Password=SqlServer2019!; TrustServerCertificate=Yes";
    }


    public List<CarOperation> Get(string technology)
    {
        if (technology.Equals("dapper"))
        {
            var list = new List<CarOperation>();

            foreach (var row in DapperUtilsRepository<dynamic>.GetAll(CarOperation.GETALL))
            {
                Car c = new()
                {
                    Plate = row.Plate,
                    Name = row.Name,
                    YearManufacture = row.YearManufacture,
                    YearModel = row.YearModel,
                    Color = row.Color,
                    Sold = row.Sold
                };

                Operation o = new()
                {
                    Id = row.OperationId,
                    Description = row.OperationDescription
                };

                list.Add(new CarOperation
                {
                    Id = row.CarOperationId,
                    Car = c,
                    Operation = o,
                    Status = row.CarOperationStatus
                });
            }

            return list;
        }

        if (technology.Equals("ado"))
        {
            List<CarOperation> list = new List<CarOperation>();

            try
            {
                using SqlConnection connection = new(_connectionString);

                connection.Open();

                using SqlCommand command = new(CarOperation.GETALL, connection);

                using SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Car c = new()
                    {
                        Plate = reader["Plate"].ToString(),
                        Name = reader["Name"].ToString(),
                        YearManufacture = Convert.ToInt32(reader["YearManufacture"]),
                        YearModel = Convert.ToInt32(reader["YearModel"]),
                        Color = reader["Color"].ToString(),
                        Sold = Convert.ToBoolean(reader["Sold"])
                    };

                    Operation o = new()
                    {
                        Id = Convert.ToInt32(reader["OperationId"]),
                        Description = reader["OperationDescription"].ToString()
                    };

                    CarOperation carOperation = new()
                    {
                        Id = Convert.ToInt32(reader["CarOperationId"]),
                        Car = c,
                        Operation = o,
                        Status = Convert.ToBoolean(reader["CarOperationStatus"])
                    };

                    list.Add(carOperation);
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


    public CarOperation Get(string technology, int id)
    {
        if (technology.Equals("dapper"))
        {
            var row = DapperUtilsRepository<dynamic>.Get(CarOperation.GET, new { CarOperationId = id });

            if (row == null)
                return null;

            Car car = new()
            {
                Plate = row.Plate,
                Name = row.Name,
                YearManufacture = row.YearManufacture,
                YearModel = row.YearModel,
                Color = row.Color,
                Sold = row.Sold
            };

            Operation operation = new()
            {
                Id = row.OperationId,
                Description = row.OperationDescription
            };

            CarOperation carOperation = new()
            {
                Id = row.CarOperationId,
                Car = car,
                Operation = operation,
                Status = row.CarOperationStatus
            };

            return carOperation;
        }

        if (technology.Equals("ado"))
        {
            try
            {
                using SqlConnection connection = new(_connectionString);

                connection.Open();

                using SqlCommand command = new(CarOperation.GET, connection);
                command.Parameters.AddWithValue("@CarOperationId", id);

                using SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {

                    Car car = new()
                    {
                        Plate = reader["Plate"].ToString(),
                        Name = reader["Name"].ToString(),
                        YearManufacture = Convert.ToInt32(reader["YearManufacture"]),
                        YearModel = Convert.ToInt32(reader["YearModel"]),
                        Color = reader["Color"].ToString(),
                        Sold = Convert.ToBoolean(reader["Sold"])
                    };

                    Operation operation = new()
                    {
                        Id = Convert.ToInt32(reader["OperationId"]),
                        Description = reader["OperationDescription"].ToString()
                    };

                    CarOperation carOperation = new()
                    {
                        Id = Convert.ToInt32(reader["CarOperationId"]),
                        Car = car,
                        Operation = operation,
                        Status = Convert.ToBoolean(reader["CarOperationStatus"])
                    };

                    return carOperation;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        return null;
    }


    public int Post(string technology, CarOperation carOperation)
    {
        if (technology.Equals("dapper"))
        {
            object obj = new
            {
                CarPlate = carOperation.Car.Plate,
                OperationId = carOperation.Operation.Id,
                Status = carOperation.Status
            };
            return DapperUtilsRepository<CarOperation>.InsertWithScalar(CarOperation.POST, obj);
        }

        if (technology.Equals("ado"))
        {
            try
            {
                using SqlConnection connection = new(_connectionString);

                connection.Open();

                using SqlCommand command = new(CarOperation.POST, connection);
                command.Parameters.AddWithValue("@CarPlate", carOperation.Car.Plate);
                command.Parameters.AddWithValue("@OperationId", carOperation.Operation.Id);
                command.Parameters.AddWithValue("@Status", carOperation.Status);

                return (int)command.ExecuteScalar();
            }
            catch (Exception)
            {
                return -1;
            }
        }

        return -1;
    }

}
