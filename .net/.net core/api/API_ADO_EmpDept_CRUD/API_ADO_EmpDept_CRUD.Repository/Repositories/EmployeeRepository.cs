using System.Data;
using API_ADO_EmpDept_CRUD.Models.Models;
using API_ADO_EmpDept_CRUD.Repository.Data;
using Microsoft.Data.SqlClient;

namespace API_ADO_EmpDept_CRUD.Repository.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly ISqlConnectionFactory _connectionFactory;

        public EmployeeRepository(ISqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<Employee>> GetAllAsync()
        {
            var employees = new List<Employee>();

            using var connection = (SqlConnection)_connectionFactory.CreateConnection();
            const string query = @"
                SELECT e.EmployeeId, e.FirstName, e.LastName, e.Email, e.Salary, 
                       e.DepartmentId, d.DepartmentName, e.HireDate
                FROM Employees e
                INNER JOIN Departments d ON e.DepartmentId = d.DepartmentId
                ORDER BY e.EmployeeId DESC";

            using var command = new SqlCommand(query, connection);
            await connection.OpenAsync();

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                employees.Add(MapReaderToEmployee(reader));
            }

            return employees;
        }

        public async Task<Employee?> GetByIdAsync(int id)
        {
            using var connection = (SqlConnection)_connectionFactory.CreateConnection();
            const string query = @"
                SELECT e.EmployeeId, e.FirstName, e.LastName, e.Email, e.Salary, 
                       e.DepartmentId, d.DepartmentName, e.HireDate
                FROM Employees e
                INNER JOIN Departments d ON e.DepartmentId = d.DepartmentId
                WHERE e.EmployeeId = @EmployeeId";

            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@EmployeeId", id);

            await connection.OpenAsync();

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return MapReaderToEmployee(reader);
            }

            return null;
        }

        public async Task<Employee?> GetByEmailAsync(string email)
        {
            using var connection = (SqlConnection)_connectionFactory.CreateConnection();
            const string query = @"
                SELECT e.EmployeeId, e.FirstName, e.LastName, e.Email, e.Salary, 
                       e.DepartmentId, d.DepartmentName, e.HireDate
                FROM Employees e
                INNER JOIN Departments d ON e.DepartmentId = d.DepartmentId
                WHERE LOWER(e.Email) = LOWER(@Email)";

            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@Email", email.Trim());

            await connection.OpenAsync();

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return MapReaderToEmployee(reader);
            }

            return null;
        }

        public async Task<int> CreateAsync(Employee employee)
        {
            using var connection = (SqlConnection)_connectionFactory.CreateConnection();
            const string query = @"
                INSERT INTO Employees (FirstName, LastName, Email, Salary, DepartmentId, HireDate)
                VALUES (@FirstName, @LastName, @Email, @Salary, @DepartmentId, @HireDate);
                SELECT CAST(SCOPE_IDENTITY() as int);";

            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@FirstName", employee.FirstName);
            command.Parameters.AddWithValue("@LastName", employee.LastName);
            command.Parameters.AddWithValue("@Email", employee.Email);
            command.Parameters.AddWithValue("@Salary", employee.Salary);
            command.Parameters.AddWithValue("@DepartmentId", employee.DepartmentId);
            command.Parameters.AddWithValue("@HireDate", employee.HireDate == default ? DateTime.Now : employee.HireDate);

            await connection.OpenAsync();
            var newId = (int)(await command.ExecuteScalarAsync() ?? 0);
            return newId;
        }

        public async Task<bool> UpdateAsync(Employee employee)
        {
            using var connection = (SqlConnection)_connectionFactory.CreateConnection();
            const string query = @"
                UPDATE Employees
                SET FirstName = @FirstName,
                    LastName = @LastName,
                    Email = @Email,
                    Salary = @Salary,
                    DepartmentId = @DepartmentId
                WHERE EmployeeId = @EmployeeId";

            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@EmployeeId", employee.EmployeeId);
            command.Parameters.AddWithValue("@FirstName", employee.FirstName);
            command.Parameters.AddWithValue("@LastName", employee.LastName);
            command.Parameters.AddWithValue("@Email", employee.Email);
            command.Parameters.AddWithValue("@Salary", employee.Salary);
            command.Parameters.AddWithValue("@DepartmentId", employee.DepartmentId);

            await connection.OpenAsync();
            int rowsAffected = await command.ExecuteNonQueryAsync();
            return rowsAffected > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using var connection = (SqlConnection)_connectionFactory.CreateConnection();
            const string query = "DELETE FROM Employees WHERE EmployeeId = @EmployeeId";

            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@EmployeeId", id);

            await connection.OpenAsync();
            int rowsAffected = await command.ExecuteNonQueryAsync();
            return rowsAffected > 0;
        }

        private static Employee MapReaderToEmployee(SqlDataReader reader)
        {
            return new Employee
            {
                EmployeeId = reader.GetInt32(reader.GetOrdinal("EmployeeId")),
                FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                LastName = reader.GetString(reader.GetOrdinal("LastName")),
                Email = reader.GetString(reader.GetOrdinal("Email")),
                Salary = reader.GetDecimal(reader.GetOrdinal("Salary")),
                DepartmentId = reader.GetInt32(reader.GetOrdinal("DepartmentId")),
                DepartmentName = reader.GetString(reader.GetOrdinal("DepartmentName")),
                HireDate = reader.GetDateTime(reader.GetOrdinal("HireDate"))
            };
        }
    }
}
