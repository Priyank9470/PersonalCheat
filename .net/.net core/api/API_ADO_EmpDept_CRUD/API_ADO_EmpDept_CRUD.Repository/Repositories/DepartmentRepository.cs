using System.Data;
using API_ADO_EmpDept_CRUD.Models.Models;
using API_ADO_EmpDept_CRUD.Repository.Data;
using Microsoft.Data.SqlClient;

namespace API_ADO_EmpDept_CRUD.Repository.Repositories
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly ISqlConnectionFactory _connectionFactory;

        public DepartmentRepository(ISqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<Department>> GetAllAsync()
        {
            var departments = new List<Department>();

            using var connection = (SqlConnection)_connectionFactory.CreateConnection();
            const string query = "SELECT DepartmentId, DepartmentName, Location, CreatedDate FROM Departments ORDER BY DepartmentName ASC";

            using var command = new SqlCommand(query, connection);
            await connection.OpenAsync();

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                departments.Add(MapReaderToDepartment(reader));
            }

            return departments;
        }

        public async Task<Department?> GetByIdAsync(int id)
        {
            using var connection = (SqlConnection)_connectionFactory.CreateConnection();
            const string query = "SELECT DepartmentId, DepartmentName, Location, CreatedDate FROM Departments WHERE DepartmentId = @DepartmentId";

            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@DepartmentId", id);

            await connection.OpenAsync();

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return MapReaderToDepartment(reader);
            }

            return null;
        }

        public async Task<Department?> GetByNameAsync(string name)
        {
            using var connection = (SqlConnection)_connectionFactory.CreateConnection();
            const string query = "SELECT DepartmentId, DepartmentName, Location, CreatedDate FROM Departments WHERE LOWER(DepartmentName) = LOWER(@DepartmentName)";

            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@DepartmentName", name.Trim());

            await connection.OpenAsync();

            using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                return MapReaderToDepartment(reader);
            }

            return null;
        }

        public async Task<int> CreateAsync(Department department)
        {
            using var connection = (SqlConnection)_connectionFactory.CreateConnection();
            const string query = @"
                INSERT INTO Departments (DepartmentName, Location, CreatedDate)
                VALUES (@DepartmentName, @Location, @CreatedDate);
                SELECT CAST(SCOPE_IDENTITY() as int);";

            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@DepartmentName", department.DepartmentName);
            command.Parameters.AddWithValue("@Location", (object?)department.Location ?? DBNull.Value);
            command.Parameters.AddWithValue("@CreatedDate", department.CreatedDate == default ? DateTime.Now : department.CreatedDate);

            await connection.OpenAsync();
            var newId = (int)(await command.ExecuteScalarAsync() ?? 0);
            return newId;
        }

        public async Task<bool> UpdateAsync(Department department)
        {
            using var connection = (SqlConnection)_connectionFactory.CreateConnection();
            const string query = @"
                UPDATE Departments
                SET DepartmentName = @DepartmentName,
                    Location = @Location
                WHERE DepartmentId = @DepartmentId";

            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@DepartmentId", department.DepartmentId);
            command.Parameters.AddWithValue("@DepartmentName", department.DepartmentName);
            command.Parameters.AddWithValue("@Location", (object?)department.Location ?? DBNull.Value);

            await connection.OpenAsync();
            int rowsAffected = await command.ExecuteNonQueryAsync();
            return rowsAffected > 0;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            using var connection = (SqlConnection)_connectionFactory.CreateConnection();
            const string query = "DELETE FROM Departments WHERE DepartmentId = @DepartmentId";

            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@DepartmentId", id);

            await connection.OpenAsync();
            int rowsAffected = await command.ExecuteNonQueryAsync();
            return rowsAffected > 0;
        }

        public async Task<bool> HasEmployeesAsync(int departmentId)
        {
            using var connection = (SqlConnection)_connectionFactory.CreateConnection();
            const string query = "SELECT COUNT(1) FROM Employees WHERE DepartmentId = @DepartmentId";

            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@DepartmentId", departmentId);

            await connection.OpenAsync();
            int count = Convert.ToInt32(await command.ExecuteScalarAsync());
            return count > 0;
        }

        private static Department MapReaderToDepartment(SqlDataReader reader)
        {
            return new Department
            {
                DepartmentId = reader.GetInt32(reader.GetOrdinal("DepartmentId")),
                DepartmentName = reader.GetString(reader.GetOrdinal("DepartmentName")),
                Location = reader.IsDBNull(reader.GetOrdinal("Location")) ? null : reader.GetString(reader.GetOrdinal("Location")),
                CreatedDate = reader.GetDateTime(reader.GetOrdinal("CreatedDate"))
            };
        }
    }
}
