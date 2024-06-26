﻿using Microsoft.Data.SqlClient;
using Models.Cars;
using System.Drawing;

namespace Repositories.Cars;

public class PurchaseRepository
{
    private string _connectionString;

    public PurchaseRepository()
    {
        _connectionString = "Data Source=127.0.0.1; Initial Catalog=DBAndreVehicles; User Id=sa; Password=SqlServer2019!; TrustServerCertificate=Yes";
    }


    public List<Purchase> Get(string technology)
    {
        if (technology.Equals("dapper"))
        {
            var list = new List<Purchase>();

            foreach (var row in DapperUtilsRepository<dynamic>.GetAll(Purchase.GETALL))
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

                var p = new Purchase()
                {
                    Id = row.PurchaseId,
                    Car = c,
                    Price = row.PurchacePrice,
                    PurchaseDate = row.PurchaseDate
                };
                list.Add(p);
            }
            return list;
        }

        if (technology.Equals("ado"))
        {
            List<Purchase> list = new List<Purchase>();

            try
            {
                using SqlConnection connection = new(_connectionString);
                using SqlCommand command = new(Purchase.GETALL, connection);

                connection.Open();
                using SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
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

                    Purchase purchase = new Purchase
                    {
                        Id = Convert.ToInt32(reader["PurchaseId"]),
                        Price = Convert.ToDecimal(reader["PurchacePrice"]),
                        PurchaseDate = Convert.ToDateTime(reader["PurchaseDate"]),
                        Car = car
                    };

                    list.Add(purchase);
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


    public Purchase Get(string technology, int id)
    {
        if (technology.Equals("dapper"))
        {
            var row = DapperUtilsRepository<dynamic>.Get(Purchase.GET, new { Id = id });

            Car c = new()
            {
                Plate = row.Plate,
                Name = row.Name,
                YearManufacture = row.YearManufacture,
                YearModel = row.YearModel,
                Color = row.Color,
                Sold = row.Sold
            };

            var p = new Purchase()
            {
                Id = row.PurchaseId,
                Car = c,
                Price = row.PurchacePrice,
                PurchaseDate = row.PurchaseDate
            };

            return p;
        }

        if (technology.Equals("ado"))
        {
            Purchase purchase = new();
            try
            {
                using SqlConnection connection = new(_connectionString);
                using SqlCommand command = new(Purchase.GET, connection);

                connection.Open();
                command.Parameters.AddWithValue("@Id", id);

                using SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
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

                    Purchase p = new Purchase
                    {
                        Id = Convert.ToInt32(reader["PurchaseId"]),
                        Price = Convert.ToDecimal(reader["PurchacePrice"]),
                        PurchaseDate = Convert.ToDateTime(reader["PurchaseDate"]),
                        Car = car
                    };

                    return p;
                }
                else
                    return null;
            }
            catch (Exception)
            {
                return null;
            }

        }
        return null;
    }


    public bool Post(string technology, Purchase purchase)
    {
        if (technology.Equals("dapper"))
        {
            object obj = new
            {
                CarPlate = purchase.Car.Plate,
                purchase.Price,
                purchase.PurchaseDate
            };

            return DapperUtilsRepository<Purchase>.Insert(Purchase.POST, obj);
        }

        if (technology.Equals("ado"))
        {
            try
            {
                using SqlConnection connection = new(_connectionString);
                using SqlCommand command = new(Purchase.POST, connection);

                command.Parameters.AddWithValue("@CarPlate", purchase.Car.Plate);
                command.Parameters.AddWithValue("@Price", purchase.Price);
                command.Parameters.AddWithValue("@PurchaseDate", purchase.PurchaseDate);

                connection.Open();
                return command.ExecuteNonQuery() > 0;
            }
            catch (Exception)
            {
                return false;
            }
        }

        return false;
    }

}
