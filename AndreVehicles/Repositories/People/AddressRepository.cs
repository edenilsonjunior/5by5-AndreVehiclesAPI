using Microsoft.Data.SqlClient;
using Models.People;
using System.Configuration;
using static Dapper.SqlMapper;
using System.Net;
using MongoDB.Driver;

namespace Repositories.People;

public class AddressRepository
{
    private string _connectionString;

    public AddressRepository()
    {
        _connectionString = "Data Source=127.0.0.1; Initial Catalog=DBAndreVehicles; User Id=sa; Password=SqlServer2019!; TrustServerCertificate=Yes";
    }

    public List<Address> Get(string technology)
    {
        if (technology.Equals("dapper"))
        {
            return DapperUtilsRepository<Address>.GetAll(Address.GETALL);
        }

        if (technology.Equals("ado"))
        {
            List<Address> list = new List<Address>();

            try
            {
                using SqlConnection connection = new(_connectionString);
                using SqlCommand command = new(Address.GETALL, connection);

                connection.Open();
                using SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Address address = new Address
                    {
                        Id = reader["Id"].ToString(),
                        Street = reader["Street"].ToString(),
                        PostalCode = reader["PostalCode"].ToString(),
                        District = reader["District"].ToString(),
                        StreetType = reader["StreetType"].ToString(),
                        AdditionalInfo = reader["AdditionalInfo"].ToString(),
                        Number = Convert.ToInt32(reader["Number"]),
                        State = reader["State"].ToString(),
                        City = reader["City"].ToString()
                    };

                    list.Add(address);
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

    public Address Get(string technology, int id)
    {
        if (technology.Equals("dapper"))
        {
            var list = DapperUtilsRepository<Address>.Get(Address.GET, new { Id = id });
            return list;
        }

        if (technology.Equals("ado"))
        {
            Address address = new Address();
            try
            {
                using SqlConnection connection = new(_connectionString);
                using SqlCommand command = new(Address.GET, connection);

                command.Parameters.AddWithValue("@Id", id);

                connection.Open();
                using SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    address.Id = reader["Id"].ToString();
                    address.Street = reader["Street"].ToString();
                    address.PostalCode = reader["PostalCode"].ToString();
                    address.District = reader["District"].ToString();
                    address.StreetType = reader["StreetType"].ToString();
                    address.AdditionalInfo = reader["AdditionalInfo"].ToString();
                    address.Number = Convert.ToInt32(reader["Number"]);
                    address.State = reader["State"].ToString();
                    address.City = reader["City"].ToString();
                }
            }
            catch (Exception)
            {
                return null;
            }

            return address;
        }
        return null;    
    }

    public bool Post(string technology, Address address)
    {
        if (technology.Equals("dapper"))
        {
            return DapperUtilsRepository<Address>.Insert(Address.POST, address);
        }

        if (technology.Equals("ado"))
        {
            try
            {
                SqlConnection connection = new(_connectionString);

                SqlCommand command = new(Address.POST, connection);

                command.Parameters.AddWithValue("@Id", address.Id);
                command.Parameters.AddWithValue("@Street", address.Street);
                command.Parameters.AddWithValue("@PostalCode", address.PostalCode);
                command.Parameters.AddWithValue("@District", address.District);
                command.Parameters.AddWithValue("@StreetType", address.StreetType);
                command.Parameters.AddWithValue("@AdditionalInfo", address.AdditionalInfo);
                command.Parameters.AddWithValue("@Number", address.Number);
                command.Parameters.AddWithValue("@State", address.State);
                command.Parameters.AddWithValue("@City", address.City);

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


    public Address PostMongo(Address address)
    {
        string connectionString = "mongodb://root:Mongo%402024%23@localhost:27017/";
        string databaseName = "DBAndreVehicles";
        string collectionName = "Address";

        var client = new MongoClient(connectionString);
        var database = client.GetDatabase(databaseName);
        var collection = database.GetCollection<Address>(collectionName);

        collection.InsertOne(address);

        return address;
    }
}
