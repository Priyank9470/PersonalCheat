using API_ADO_EmpDept_CRUD.Models.Common;
using API_ADO_EmpDept_CRUD.Models.DTOs;

namespace API_ADO_EmpDept_CRUD.Service.Services
{
    public interface IDepartmentService
    {
        Task<ServiceResult<IEnumerable<DepartmentDto>>> GetAllDepartmentsAsync();
        Task<ServiceResult<DepartmentDto>> GetDepartmentByIdAsync(int id);
        Task<ServiceResult<DepartmentDto>> CreateDepartmentAsync(CreateUpdateDepartmentDto dto);
        Task<ServiceResult<DepartmentDto>> UpdateDepartmentAsync(int id, CreateUpdateDepartmentDto dto);
        Task<ServiceResult<bool>> DeleteDepartmentAsync(int id);
    }
}
