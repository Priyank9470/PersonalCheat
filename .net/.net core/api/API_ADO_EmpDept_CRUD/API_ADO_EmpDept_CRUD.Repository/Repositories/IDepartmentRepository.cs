using API_ADO_EmpDept_CRUD.Models.Models;

namespace API_ADO_EmpDept_CRUD.Repository.Repositories
{
    public interface IDepartmentRepository
    {
        Task<IEnumerable<Department>> GetAllAsync();
        Task<Department?> GetByIdAsync(int id);
        Task<Department?> GetByNameAsync(string name);
        Task<int> CreateAsync(Department department);
        Task<bool> UpdateAsync(Department department);
        Task<bool> DeleteAsync(int id);
        Task<bool> HasEmployeesAsync(int departmentId);
    }
}
