using Microsoft.Data.SqlClient;
using Models.People;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.People;

public class DependentRepository
{
    private readonly string _connectionString;

    public DependentRepository()
    {
        _connectionString = "Data Source=127.0.0.1; Initial Catalog=DBAndreVehicles; User Id=sa; Password=SqlServer2019!; TrustServerCertificate=Yes";
    }

    public List<Dependent> Get()
    {
        using SqlConnection connection = new(_connectionString);
        SqlCommand command = new(Dependent.GETALL, connection);

        connection.Open();
        SqlDataReader reader = command.ExecuteReader();

        List<Dependent> list = new();
        while (reader.Read())
        {
            string customerDocument = reader["CustomerDocument"].ToString();
            Customer customer = new CustomerRepository().Get("ado", customerDocument);

            Address address = new()
            {
                Id = reader["Id"].ToString(),
                Street = reader["Street"].ToString(),
                PostalCode = reader["PostalCode"].ToString(),
                District = reader["District"].ToString(),
                StreetType = reader["StreetType"].ToString(),
                AdditionalInfo = reader["AdditionalInfo"].ToString(),
                Number = Convert.ToInt32(reader["Number"].ToString()),
                State = reader["State"].ToString(),
            };

            Dependent dependent = new()
            {
                Document = reader["Document"].ToString(),
                Name = reader["Name"].ToString(),
                BirthDate = Convert.ToDateTime(reader["BirthDate"]),
                Phone = reader["Phone"].ToString(),
                Email = reader["Email"].ToString(),
                Address = address,
                Customer = customer
            };

            list.Add(dependent);
        }

        return list;
    }

    public Dependent Get(string document)
    {
        using SqlConnection connection = new(_connectionString);
        SqlCommand command = new(Dependent.GET, connection);

        command.Parameters.AddWithValue("@Document", document);

        connection.Open();
        SqlDataReader reader = command.ExecuteReader();

        if (reader.Read())
        {
            string customerDocument = reader["CustomerDocument"].ToString();
            Customer customer = new CustomerRepository().Get("ado", customerDocument);

            Address address = new()
            {
                Id = reader["Id"].ToString(),
                Street = reader["Street"].ToString(),
                PostalCode = reader["PostalCode"].ToString(),
                District = reader["District"].ToString(),
                StreetType = reader["StreetType"].ToString(),
                AdditionalInfo = reader["AdditionalInfo"].ToString(),
                Number = Convert.ToInt32(reader["Number"].ToString()),
                State = reader["State"].ToString(),
            };

            Dependent dependent = new()
            {
                Document = reader["Document"].ToString(),
                Name = reader["Name"].ToString(),
                BirthDate = Convert.ToDateTime(reader["BirthDate"]),
                Phone = reader["Phone"].ToString(),
                Email = reader["Email"].ToString(),
                Address = address,
                Customer = customer
            };

            return dependent;
        }
        else
        {
            return null;
        }
    }

    public bool Post(Dependent dependent)
    {
        try
        {
            using SqlConnection connection = new(_connectionString);

            //using SqlTransaction transaction = connection.BeginTransaction();





            using SqlCommand command = new(Person.INSERT, connection);
            command.Parameters.AddWithValue("@Document", dependent.Document);
            command.Parameters.AddWithValue("@Name", dependent.Name);
            command.Parameters.AddWithValue("@BirthDate", dependent.BirthDate);
            command.Parameters.AddWithValue("@AddressId", dependent.Address.Id);
            command.Parameters.AddWithValue("@Phone", dependent.Phone);
            command.Parameters.AddWithValue("@Email", dependent.Email);

            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();

            command.CommandText = Dependent.POST;
            command.Parameters.Clear();

            command.Parameters.AddWithValue("@CustomerDocument", dependent.Customer.Document);
            command.Parameters.AddWithValue("@Document", dependent.Document);

            connection.Open();
            command.ExecuteNonQuery();
            connection.Close();

            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}
