using API_ADO_EmpDept_CRUD.Models.Common;
using API_ADO_EmpDept_CRUD.Models.DTOs;
using API_ADO_EmpDept_CRUD.Models.Models;
using API_ADO_EmpDept_CRUD.Repository.Repositories;

namespace API_ADO_EmpDept_CRUD.Service.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IDepartmentRepository _departmentRepository;

        public EmployeeService(IEmployeeRepository employeeRepository, IDepartmentRepository departmentRepository)
        {
            _employeeRepository = employeeRepository;
            _departmentRepository = departmentRepository;
        }

        public async Task<ServiceResult<IEnumerable<EmployeeDto>>> GetAllEmployeesAsync()
        {
            var employees = await _employeeRepository.GetAllAsync();
            var dtos = employees.Select(MapToDto);
            return ServiceResult<IEnumerable<EmployeeDto>>.Ok(dtos);
        }

        public async Task<ServiceResult<EmployeeDto>> GetEmployeeByIdAsync(int id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);
            if (employee == null)
            {
                return ServiceResult<EmployeeDto>.NotFound($"Employee with ID {id} was not found.");
            }

            return ServiceResult<EmployeeDto>.Ok(MapToDto(employee));
        }

        public async Task<ServiceResult<EmployeeDto>> CreateEmployeeAsync(CreateUpdateEmployeeDto dto)
        {
            if (string.IsNullOrWhiteSpace(dto.FirstName))
            {
                return ServiceResult<EmployeeDto>.BadRequest("First name is required.");
            }

            if (string.IsNullOrWhiteSpace(dto.LastName))
            {
                return ServiceResult<EmployeeDto>.BadRequest("Last name is required.");
            }

            if (string.IsNullOrWhiteSpace(dto.Email))
            {
                return ServiceResult<EmployeeDto>.BadRequest("Email is required.");
            }

            if (dto.Salary <= 0)
            {
                return ServiceResult<EmployeeDto>.BadRequest("Salary must be greater than zero.");
            }

            // CRITICAL VALIDATION: Department existence check
            var department = await _departmentRepository.GetByIdAsync(dto.DepartmentId);
            if (department == null)
            {
                return ServiceResult<EmployeeDto>.BadRequest($"Department with ID {dto.DepartmentId} does not exist. An employee must be assigned to an existing department.");
            }

            // CRITICAL VALIDATION: Email uniqueness check
            var existingEmployeeWithEmail = await _employeeRepository.GetByEmailAsync(dto.Email);
            if (existingEmployeeWithEmail != null)
            {
                return ServiceResult<EmployeeDto>.Conflict($"An employee with email '{dto.Email}' already exists.");
            }

            var employee = new Employee
            {
                FirstName = dto.FirstName.Trim(),
                LastName = dto.LastName.Trim(),
                Email = dto.Email.Trim().ToLowerInvariant(),
                Salary = dto.Salary,
                DepartmentId = dto.DepartmentId,
                DepartmentName = department.DepartmentName,
                HireDate = DateTime.Now
            };

            int newId = await _employeeRepository.CreateAsync(employee);
            employee.EmployeeId = newId;

            return ServiceResult<EmployeeDto>.Created(MapToDto(employee));
        }

        public async Task<ServiceResult<EmployeeDto>> UpdateEmployeeAsync(int id, CreateUpdateEmployeeDto dto)
        {
            var existingEmployee = await _employeeRepository.GetByIdAsync(id);
            if (existingEmployee == null)
            {
                return ServiceResult<EmployeeDto>.NotFound($"Employee with ID {id} was not found.");
            }

            if (string.IsNullOrWhiteSpace(dto.FirstName))
            {
                return ServiceResult<EmployeeDto>.BadRequest("First name is required.");
            }

            if (string.IsNullOrWhiteSpace(dto.LastName))
            {
                return ServiceResult<EmployeeDto>.BadRequest("Last name is required.");
            }

            if (string.IsNullOrWhiteSpace(dto.Email))
            {
                return ServiceResult<EmployeeDto>.BadRequest("Email is required.");
            }

            if (dto.Salary <= 0)
            {
                return ServiceResult<EmployeeDto>.BadRequest("Salary must be greater than zero.");
            }

            // CRITICAL VALIDATION: Department existence check
            var department = await _departmentRepository.GetByIdAsync(dto.DepartmentId);
            if (department == null)
            {
                return ServiceResult<EmployeeDto>.BadRequest($"Department with ID {dto.DepartmentId} does not exist. An employee must be assigned to an existing department.");
            }

            // CRITICAL VALIDATION: Email uniqueness check
            var existingEmployeeWithEmail = await _employeeRepository.GetByEmailAsync(dto.Email);
            if (existingEmployeeWithEmail != null && existingEmployeeWithEmail.EmployeeId != id)
            {
                return ServiceResult<EmployeeDto>.Conflict($"Another employee with email '{dto.Email}' already exists.");
            }

            existingEmployee.FirstName = dto.FirstName.Trim();
            existingEmployee.LastName = dto.LastName.Trim();
            existingEmployee.Email = dto.Email.Trim().ToLowerInvariant();
            existingEmployee.Salary = dto.Salary;
            existingEmployee.DepartmentId = dto.DepartmentId;
            existingEmployee.DepartmentName = department.DepartmentName;

            bool updated = await _employeeRepository.UpdateAsync(existingEmployee);
            if (!updated)
            {
                return ServiceResult<EmployeeDto>.BadRequest("Failed to update employee record.");
            }

            return ServiceResult<EmployeeDto>.Ok(MapToDto(existingEmployee));
        }

        public async Task<ServiceResult<bool>> DeleteEmployeeAsync(int id)
        {
            var employee = await _employeeRepository.GetByIdAsync(id);
            if (employee == null)
            {
                return ServiceResult<bool>.NotFound($"Employee with ID {id} was not found.");
            }

            bool deleted = await _employeeRepository.DeleteAsync(id);
            if (!deleted)
            {
                return ServiceResult<bool>.BadRequest("Failed to delete employee record.");
            }

            return ServiceResult<bool>.Ok(true);
        }

        private static EmployeeDto MapToDto(Employee emp)
        {
            return new EmployeeDto
            {
                EmployeeId = emp.EmployeeId,
                FirstName = emp.FirstName,
                LastName = emp.LastName,
                Email = emp.Email,
                Salary = emp.Salary,
                DepartmentId = emp.DepartmentId,
                DepartmentName = emp.DepartmentName,
                HireDate = emp.HireDate
            };
        }
    }
}
