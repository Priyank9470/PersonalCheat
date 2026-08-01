namespace API_ADO_EmpDept_CRUD.Models.Models
{
    public class Department
    {
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; } = string.Empty;
        public string? Location { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
