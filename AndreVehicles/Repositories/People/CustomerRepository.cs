using Microsoft.Data.SqlClient;
using Models.People;
using System.Configuration;

namespace Repositories.People;

public class CustomerRepository
{
    private string _connectionString;

    public CustomerRepository()
    {
        _connectionString = "Data Source=127.0.0.1; Initial Catalog=DBAndreVehicles; User Id=sa; Password=SqlServer2019!; TrustServerCertificate=Yes";
    }


    public List<Customer> Get(string technology)
    {
        if (technology.Equals("dapper"))
        {
            var list = new List<Customer>();

            foreach (var row in DapperUtilsRepository<dynamic>.GetAll(Customer.GETALL))
            {
                Address address = new(row.Street, row.PostalCode, row.District, row.StreetType, row.AdditionalInfo, row.Number, row.State, row.City);
                address.Id = row.Id;
                Customer customer = new(row.Document, row.Name, row.BirthDate, address, row.Phone, row.Email, row.Income);

                list.Add(customer);
            }

            return list;
        }

        if (technology.Equals("ado"))
        {
            List<Customer> list = new List<Customer>();

            try
            {
                using SqlConnection connection = new(_connectionString);
                using SqlCommand command = new(Customer.GETALL, connection);

                connection.Open();
                using SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Address address = new()
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

                    Customer customer = new()
                    {
                        Document = reader["Document"].ToString(),
                        Name = reader["Name"].ToString(),
                        BirthDate = Convert.ToDateTime(reader["BirthDate"]),
                        Address = address,
                        Phone = reader["Phone"].ToString(),
                        Email = reader["Email"].ToString(),
                        Income = Convert.ToDecimal(reader["Income"])
                    };
                    list.Add(customer);
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

    public Customer Get(string technology, string document)
    {
        if (technology.Equals("dapper"))
        {
            dynamic row = DapperUtilsRepository<dynamic>.Get(Customer.GET, new { Document = document });

            if(row == null) return null;

            Address address = new(row.Street, row.PostalCode, row.District, row.StreetType, row.AdditionalInfo, row.Number, row.State, row.City);
            address.Id = row.Id;
            Customer customer = new(row.Document, row.Name, row.BirthDate, address, row.Phone, row.Email, row.Income);
            return customer;
        }

        if (technology.Equals("ado"))
        {
            Customer customer = new();
            try
            {
                using SqlConnection connection = new(_connectionString);
                using SqlCommand command = new(Customer.GET, connection);

                command.Parameters.AddWithValue("@Document", document);

                connection.Open();
                using SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    Address address = new()
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

                    customer = new()
                    {
                        Document = reader["Document"].ToString(),
                        Name = reader["Name"].ToString(),
                        BirthDate = Convert.ToDateTime(reader["BirthDate"]),
                        Address = address,
                        Phone = reader["Phone"].ToString(),
                        Email = reader["Email"].ToString(),
                        Income = Convert.ToDecimal(reader["Income"])
                    };
                }
            }
            catch (Exception)
            {
                return null;
            }

            return customer;
        }

        return null;
    }

    public bool Post(string technology, Customer customer)
    {
        if (technology.Equals("dapper"))
        {
            object person = new
            {
                Document = customer.Document,
                Name = customer.Name,
                BirthDate = customer.BirthDate,
                AddressId = customer.Address.Id,
                Phone = customer.Phone,
                Email = customer.Email
            };

            bool firstSuccess = DapperUtilsRepository<Person>.Insert(Person.INSERT, person);

            if (firstSuccess)
            {
                return DapperUtilsRepository<Customer>.Insert(Customer.POST, customer);
            }
        }

        if (technology.Equals("ado"))
        {
            try
            {
                using SqlConnection connection = new(_connectionString);
                connection.Open();

                using SqlTransaction transaction = connection.BeginTransaction();

                using SqlCommand command = new(Person.INSERT, connection, transaction);
                command.Parameters.AddWithValue("@Document", customer.Document);
                command.Parameters.AddWithValue("@Name", customer.Name);
                command.Parameters.AddWithValue("@BirthDate", customer.BirthDate);
                command.Parameters.AddWithValue("@AddressId", customer.Address.Id);
                command.Parameters.AddWithValue("@Phone", customer.Phone);
                command.Parameters.AddWithValue("@Email", customer.Email);

                command.ExecuteNonQuery();

                command.CommandText = Customer.POST;
                command.Parameters.Clear();
                command.Parameters.AddWithValue("@Document", customer.Document);
                command.Parameters.AddWithValue("@Income", customer.Income);

                command.ExecuteNonQuery();

                transaction.Commit();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        return false;
    }

}
