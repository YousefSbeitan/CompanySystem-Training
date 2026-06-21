namespace CompanySystem.Data.Entities;

public class Department
{
    public int DepartmentId { get; set; }

    public string DepartmentName { get; set; }

    public int ManagerId { get; set; }
}