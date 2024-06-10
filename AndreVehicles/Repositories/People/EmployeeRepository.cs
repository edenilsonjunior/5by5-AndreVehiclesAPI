using Microsoft.Data.SqlClient;
using Models.People;

namespace Repositories.People;

public class EmployeeRepository
{
    private string _connectionString;

    public EmployeeRepository()
    {
        _connectionString = "Data Source=127.0.0.1; Initial Catalog=DBAndreVehiclesAPI; User Id=sa; Password=SqlServer2019!; TrustServerCertificate=Yes";

    }


    public List<Employee> Get(string technology)
    {
        if (technology.Equals("dapper"))
        {
            var list = new List<Employee>();

            foreach (var row in DapperUtilsRepository<dynamic>.GetAll(Employee.GETALL))
            {
                Address address = new Address(row.Street, row.PostalCode, row.District, row.StreetType, row.AdditionalInfo, row.Number, row.State, row.City);
                Role role = new Role(row.RoleDescription);
                Employee employee = new Employee(row.Document, row.Name, row.BirthDate, address, row.Phone, row.Email, role, row.CommissionValue, row.Commission);

                list.Add(employee);
            }

            return list;
        }

        if (technology.Equals("ado"))
        {
            List<Employee> employees = new();

            SqlConnection sqlConnection = new SqlConnection(_connectionString);

            SqlCommand sqlCommand = new SqlCommand(Employee.GETALL, sqlConnection);

            sqlConnection.Open();

            SqlDataReader reader = sqlCommand.ExecuteReader();

            while (reader.Read())
            {
                Address address = new()
                {
                    Street = reader["Street"].ToString(),
                    PostalCode = reader["PostalCode"].ToString(),
                    District = reader["District"].ToString(),
                    StreetType = reader["StreetType"].ToString(),
                    AdditionalInfo = reader["AdditionalInfo"].ToString(),
                    Number = Convert.ToInt32(reader["Number"].ToString()),
                    State = reader["State"].ToString(),
                    City = reader["City"].ToString()
                };

                Role role = new Role(reader["RoleDescription"].ToString());

                var employee = new Employee()
                {
                    Document = reader["Document"].ToString(),
                    Name = reader["Name"].ToString(),
                    BirthDate = Convert.ToDateTime(reader["BirthDate"]),
                    Address = address,
                    Phone = reader["Phone"].ToString(),
                    Email = reader["Email"].ToString(),
                    Role = role,
                    CommissionValue = Convert.ToDecimal(reader["CommissionValue"]),
                    Commission = Convert.ToDecimal(reader["Commission"])
                };
                employees.Add(employee);
            }
            return employees;
        }

        return null;
    }


    public Employee Get(string technology, string document)
    {
        if (technology.Equals("dapper"))
        {
            var row = DapperUtilsRepository<dynamic>.Get(Employee.GET, new { Document = document });

            if (row != null)
            {
                Address address = new Address(row.Street, row.PostalCode, row.District, row.StreetType, row.AdditionalInfo, row.Number, row.State, row.City);
                Role role = new Role(row.RoleDescription);
                Employee employee = new Employee(row.Document, row.Name, row.BirthDate, address, row.Phone, row.Email, role, row.CommissionValue, row.Commission);

                return employee;
            }
        }

        if (technology.Equals("ado"))
        {
            SqlConnection sqlConnection = new SqlConnection(_connectionString);

            SqlCommand sqlCommand = new SqlCommand(Employee.GET, sqlConnection);
            sqlCommand.Parameters.AddWithValue("@Document", document);

            sqlConnection.Open();

            SqlDataReader reader = sqlCommand.ExecuteReader();

            if (reader.Read())
            {
                Address address = new()
                {
                    Street = reader["Street"].ToString(),
                    PostalCode = reader["PostalCode"].ToString(),
                    District = reader["District"].ToString(),
                    StreetType = reader["StreetType"].ToString(),
                    AdditionalInfo = reader["AdditionalInfo"].ToString(),
                    Number = Convert.ToInt32(reader["Number"].ToString()),
                    State = reader["State"].ToString(),
                    City = reader["City"].ToString()
                };

                Role role = new Role(reader["RoleDescription"].ToString());

                var employee = new Employee()
                {
                    Document = reader["Document"].ToString(),
                    Name = reader["Name"].ToString(),
                    BirthDate = Convert.ToDateTime(reader["BirthDate"]),
                    Address = address,
                    Phone = reader["Phone"].ToString(),
                    Email = reader["Email"].ToString(),
                    Role = role,
                    CommissionValue = Convert.ToDecimal(reader["CommissionValue"]),
                    Commission = Convert.ToDecimal(reader["Commission"])
                };

                return employee;
            }
        }


        return null;
    }


    public bool Post(string technology, Employee employee)
    {
        if (technology.Equals("dapper"))
        {
            var person = new
            {
                Document = employee.Document,
                Name = employee.Name,
                BirthDate = employee.BirthDate,
                AddressId = employee.Address.Id,
                Phone = employee.Phone,
                Email = employee.Email
            };

            bool personSuccess = DapperUtilsRepository<dynamic>.Insert(Person.INSERT, person);

            if (!personSuccess)
                return false;

            employee.Role.Id = DapperUtilsRepository<dynamic>.InsertWithScalar(Role.INSERT,
                new
                {
                    Description = employee.Role.Description
                });

            var employeeData = new
            {
                Document = employee.Document,
                RoleId = employee.Role.Id,
                CommissionValue = employee.CommissionValue,
                Commission = employee.Commission
            };

            return DapperUtilsRepository<dynamic>.Insert(Employee.POST, employeeData);
        }

        if (technology.Equals("ado"))
        {
            try
            {
                SqlConnection sqlConnection = new SqlConnection(_connectionString);

                var personCmd = new SqlCommand(Person.INSERT, sqlConnection);
                personCmd.Parameters.AddWithValue("@Document", employee.Document);
                personCmd.Parameters.AddWithValue("@Name", employee.Name);
                personCmd.Parameters.AddWithValue("@BirthDate", employee.BirthDate);
                personCmd.Parameters.AddWithValue("@AddressId", employee.Address.Id);
                personCmd.Parameters.AddWithValue("@Phone", employee.Phone);
                personCmd.Parameters.AddWithValue("@Email", employee.Email);

                var roleCmd = new SqlCommand(Role.INSERT, sqlConnection);
                roleCmd.Parameters.AddWithValue("@Description", employee.Role.Description);

                sqlConnection.Open();
                personCmd.ExecuteNonQuery();
                employee.Role.Id = (int)roleCmd.ExecuteScalar();

                var employeeCmd = new SqlCommand(Employee.POST, sqlConnection);
                employeeCmd.Parameters.AddWithValue("@Document", employee.Document);
                employeeCmd.Parameters.AddWithValue("@RoleId", employee.Role.Id);
                employeeCmd.Parameters.AddWithValue("@CommissionValue", employee.CommissionValue);
                employeeCmd.Parameters.AddWithValue("@Commission", employee.Commission);
                employeeCmd.ExecuteNonQuery();

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
