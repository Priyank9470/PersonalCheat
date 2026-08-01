using API_ADO_EmpDept_CRUD.Models.Common;
using API_ADO_EmpDept_CRUD.Models.DTOs;

namespace API_ADO_EmpDept_CRUD.Service.Services
{
    public interface IEmployeeService
    {
        Task<ServiceResult<IEnumerable<EmployeeDto>>> GetAllEmployeesAsync();
        Task<ServiceResult<EmployeeDto>> GetEmployeeByIdAsync(int id);
        Task<ServiceResult<EmployeeDto>> CreateEmployeeAsync(CreateUpdateEmployeeDto dto);
        Task<ServiceResult<EmployeeDto>> UpdateEmployeeAsync(int id, CreateUpdateEmployeeDto dto);
        Task<ServiceResult<bool>> DeleteEmployeeAsync(int id);
    }
}
