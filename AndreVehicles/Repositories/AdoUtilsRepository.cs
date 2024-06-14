using Microsoft.Data.SqlClient;
using System.Data;

namespace Repositories;

public class AdoUtilsRepository
{
    private string _connectionString = "Data Source=127.0.0.1; Initial Catalog=DBAndreVehicles; User Id=sa; Password=SqlServer2019!; TrustServerCertificate=Yes";


    public async Task<int> ExecuteNonQuery(string query, List<SqlParameter> parameters)
    {
        using var connection = new SqlConnection(_connectionString);

        using var command = new SqlCommand(query, connection);

        command.Parameters.AddRange(parameters.ToArray());
        await connection.OpenAsync();
        return await command.ExecuteNonQueryAsync();
    }

    public async Task<DataTable> ExecuteReader(string query, List<SqlParameter> parameters)
    {
        using var connection = new SqlConnection(_connectionString);

        using var command = new SqlCommand(query, connection);

        if (parameters != null)
        {
            command.Parameters.AddRange(parameters.ToArray());
        }

        await connection.OpenAsync();

        using var reader = await command.ExecuteReaderAsync();

        DataTable dataTable = new();
        dataTable.Load(reader);

        return dataTable;
    }


    public async Task<int> ExecuteScalar(string query, List<SqlParameter> parameters)
    {
        using var connection = new SqlConnection(_connectionString);

        using var command = new SqlCommand(query, connection);

        command.Parameters.AddRange(parameters.ToArray());
        await connection.OpenAsync();

        return (int)await command.ExecuteScalarAsync();
    }



}
