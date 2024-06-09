using Microsoft.Data.SqlClient;
using Models.Cars;
using System.Configuration;

namespace Repositories.Cars;

public class OperationRepository
{
    private string _connectionString;

    public OperationRepository()
    {
        _connectionString = "Data Source=127.0.0.1; Initial Catalog=DBAndreVehiclesAPI; User Id=sa; Password=SqlServer2019!; TrustServerCertificate=Yes";
    }


    public List<Operation> Get(string technology)
    {
        if(technology.Equals("dapper"))
        {
            return DapperUtilsRepository<Operation>.GetAll(Operation.GETALL);
        }

        if (technology.Equals("ado"))
        {
            List<Operation> list = new List<Operation>();

            try
            {
                using SqlConnection connection = new(_connectionString);
                using SqlCommand command = new(Operation.GETALL, connection);

                connection.Open();
                using SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Operation operation = new Operation
                    {
                        Id = Convert.ToInt32(reader["Id"]),
                        Description = reader["Description"].ToString()
                    };

                    list.Add(operation);
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


    public Operation Get(string technology, int id)
    {
        if (technology.Equals("dapper"))
        {
            return DapperUtilsRepository<Operation>.Get(Operation.GET, new { Id = id });
        }

        if (technology.Equals("ado"))
        {
            Operation operation = new Operation();
            try
            {
                using SqlConnection connection = new(_connectionString);
                using SqlCommand command = new(Operation.GET, connection);

                connection.Open();
                command.Parameters.AddWithValue("@Id", id);

                using SqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    operation.Id = Convert.ToInt32(reader["Id"]);
                    operation.Description = reader["Description"].ToString();
                }
            }
            catch (Exception)
            {
                return null;
            }

            return operation;
        }

        return null;
    }


    public bool Post(string technology, Operation operation)
    {
        if (technology.Equals("dapper"))
        {
            object obj = new
            {
                operation.Description
            };

            return DapperUtilsRepository<Operation>.Insert(Operation.POST, obj);
        }

        if (technology.Equals("ado"))
        {
            try
            {
                using SqlConnection connection = new(_connectionString);
                using SqlCommand command = new(Operation.POST, connection);

                connection.Open();
                command.Parameters.AddWithValue("@Description", operation.Description);

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
