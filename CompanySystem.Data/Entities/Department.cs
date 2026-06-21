namespace CompanySystem.Data.Entities;

public class Department : TrackingEntity
{
    public int DepartmentId { get; set; }

    public string DepartmentName { get; set; }

    public string ManagerId { get; set; }
}