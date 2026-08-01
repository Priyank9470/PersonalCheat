using API_ADO_EmpDept_CRUD.Models.Common;
using API_ADO_EmpDept_CRUD.Models.DTOs;
using API_ADO_EmpDept_CRUD.Models.Models;
using API_ADO_EmpDept_CRUD.Repository.Repositories;

namespace API_ADO_EmpDept_CRUD.Service.Services
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentRepository _departmentRepository;

        public DepartmentService(IDepartmentRepository departmentRepository)
        {
            _departmentRepository = departmentRepository;
        }

        public async Task<ServiceResult<IEnumerable<DepartmentDto>>> GetAllDepartmentsAsync()
        {
            var departments = await _departmentRepository.GetAllAsync();
            var dtos = departments.Select(MapToDto);
            return ServiceResult<IEnumerable<DepartmentDto>>.Ok(dtos);
        }

        public async Task<ServiceResult<DepartmentDto>> GetDepartmentByIdAsync(int id)
        {
            var department = await _departmentRepository.GetByIdAsync(id);
            if (department == null)
            {
                return ServiceResult<DepartmentDto>.NotFound($"Department with ID {id} was not found.");
            }

            return ServiceResult<DepartmentDto>.Ok(MapToDto(department));
        }

        public async Task<ServiceResult<DepartmentDto>> CreateDepartmentAsync(CreateUpdateDepartmentDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.DepartmentName))
            {
                return ServiceResult<DepartmentDto>.BadRequest("Department name is required.");
            }

            var existingDept = await _departmentRepository.GetByNameAsync(dto.DepartmentName);
            if (existingDept != null)
            {
                return ServiceResult<DepartmentDto>.Conflict($"A department named '{dto.DepartmentName}' already exists.");
            }

            var department = new Department
            {
                DepartmentName = dto.DepartmentName.Trim(),
                Location = dto.Location?.Trim(),
                CreatedDate = DateTime.Now
            };

            int newId = await _departmentRepository.CreateAsync(department);
            department.DepartmentId = newId;

            return ServiceResult<DepartmentDto>.Created(MapToDto(department));
        }

        public async Task<ServiceResult<DepartmentDto>> UpdateDepartmentAsync(int id, CreateUpdateDepartmentDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.DepartmentName))
            {
                return ServiceResult<DepartmentDto>.BadRequest("Department name is required.");
            }

            var existingDept = await _departmentRepository.GetByIdAsync(id);
            if (existingDept == null)
            {
                return ServiceResult<DepartmentDto>.NotFound($"Department with ID {id} was not found.");
            }

            var deptWithName = await _departmentRepository.GetByNameAsync(dto.DepartmentName);
            if (deptWithName != null && deptWithName.DepartmentId != id)
            {
                return ServiceResult<DepartmentDto>.Conflict($"Another department with name '{dto.DepartmentName}' already exists.");
            }

            existingDept.DepartmentName = dto.DepartmentName.Trim();
            existingDept.Location = dto.Location?.Trim();

            bool updated = await _departmentRepository.UpdateAsync(existingDept);
            if (!updated)
            {
                return ServiceResult<DepartmentDto>.BadRequest("Failed to update department details.");
            }

            return ServiceResult<DepartmentDto>.Ok(MapToDto(existingDept));
        }

        public async Task<ServiceResult<bool>> DeleteDepartmentAsync(int id)
        {
            var department = await _departmentRepository.GetByIdAsync(id);
            if (department == null)
            {
                return ServiceResult<bool>.NotFound($"Department with ID {id} was not found.");
            }

            bool hasEmployees = await _departmentRepository.HasEmployeesAsync(id);
            if (hasEmployees)
            {
                return ServiceResult<bool>.BadRequest($"Cannot delete department '{department.DepartmentName}' (ID {id}) because it has active employees assigned to it. Reassign or delete those employees first.");
            }

            bool deleted = await _departmentRepository.DeleteAsync(id);
            if (!deleted)
            {
                return ServiceResult<bool>.BadRequest("Failed to delete the department.");
            }

            return ServiceResult<bool>.Ok(true);
        }

        private static DepartmentDto MapToDto(Department dept)
        {
            return new DepartmentDto
            {
                DepartmentId = dept.DepartmentId,
                DepartmentName = dept.DepartmentName,
                Location = dept.Location,
                CreatedDate = dept.CreatedDate
            };
        }
    }
}
