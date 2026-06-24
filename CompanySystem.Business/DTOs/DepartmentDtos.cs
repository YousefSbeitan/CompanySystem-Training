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
    [Required(ErrorMessage = "Department name is required")]
    [StringLength(100, ErrorMessage = "Department name must not exceed 100 characters")]
    public string DepartmentName { get; set; }

    [Required(ErrorMessage = "Manager ID is required")]
    [StringLength(50, ErrorMessage = "Manager ID must not exceed 50 characters")]
    public string ManagerId { get; set; }
}

public class EditDepartmentDto
{
    [Required]
    public int DepartmentId { get; set; }

    [Required(ErrorMessage = "Department name is required")]
    [StringLength(100, ErrorMessage = "Department name must not exceed 100 characters")]
    public string DepartmentName { get; set; }

    [Required(ErrorMessage = "Manager ID is required")]
    [StringLength(50, ErrorMessage = "Manager ID must not exceed 50 characters")]
    public string ManagerId { get; set; }
}