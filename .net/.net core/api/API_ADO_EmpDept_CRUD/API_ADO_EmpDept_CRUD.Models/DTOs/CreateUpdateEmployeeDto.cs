using System.ComponentModel.DataAnnotations;

namespace API_ADO_EmpDept_CRUD.Models.DTOs
{
    public class CreateUpdateEmployeeDto
    {
        [Required(ErrorMessage = "First name is required.")]
        [StringLength(50, ErrorMessage = "First name cannot exceed 50 characters.")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last name is required.")]
        [StringLength(50, ErrorMessage = "Last name cannot exceed 50 characters.")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        [StringLength(150, ErrorMessage = "Email cannot exceed 150 characters.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Salary is required.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Salary must be greater than zero.")]
        public decimal Salary { get; set; }

        [Required(ErrorMessage = "DepartmentId is required.")]
        [Range(1, int.MaxValue, ErrorMessage = "DepartmentId must be a positive integer.")]
        public int DepartmentId { get; set; }
    }
}
