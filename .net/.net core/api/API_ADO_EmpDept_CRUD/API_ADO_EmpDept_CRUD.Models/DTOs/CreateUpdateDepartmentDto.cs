using System.ComponentModel.DataAnnotations;

namespace API_ADO_EmpDept_CRUD.Models.DTOs
{
    public class CreateUpdateDepartmentDto
    {
        [Required(ErrorMessage = "Department name is required.")]
        [StringLength(100, ErrorMessage = "Department name cannot exceed 100 characters.")]
        public string DepartmentName { get; set; } = string.Empty;

        [StringLength(150, ErrorMessage = "Location cannot exceed 150 characters.")]
        public string? Location { get; set; }
    }
}
