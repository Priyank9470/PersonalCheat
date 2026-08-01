namespace API_ADO_EmpDept_CRUD.Models.DTOs
{
    public class DepartmentDto
    {
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; } = string.Empty;
        public string? Location { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
