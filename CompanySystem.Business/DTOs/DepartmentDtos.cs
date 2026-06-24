using System.ComponentModel.DataAnnotations;

namespace CompanySystem.Business.DTOs;

public class DepartmentDto
{
    public int DepartmentId { get; set; }

    public string DepartmentName { get; set; }

    public string ManagerId { get; set; }

    public string CreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }
}

public class CreateDepartmentDto
{
    
    public string DepartmentName { get; set; }

    
    public string ManagerId { get; set; }
}

public class EditDepartmentDto
{   
    public int DepartmentId { get; set; }
    
    public string DepartmentName { get; set; }

    public string ManagerId { get; set; }
}