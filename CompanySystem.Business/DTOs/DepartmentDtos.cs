using System.ComponentModel.DataAnnotations;

namespace CompanySystem.Business.DTOs;

public class DepartmentDto
{
    public int DepartmentId { get; set; }

    public string DepartmentName { get; set; } = string.Empty;

    public string ManagerId { get; set; } = string.Empty;

    public string CreatedBy { get; set; } = string.Empty;

    public DateTime CreatedDate { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }
}

public class CreateDepartmentDto
{
    [Required(ErrorMessage = "Department name is required.")]
    [StringLength(100, ErrorMessage = "Department name must not exceed 100 characters.")]
    [Display(Name = "Department Name")]
    public string DepartmentName { get; set; } = string.Empty;

    [StringLength(50, ErrorMessage = "Manager ID must not exceed 50 characters.")]
    [Display(Name = "Manager")]
    public string ManagerId { get; set; } = string.Empty;
}

public class EditDepartmentDto
{
    [Required]
    public int DepartmentId { get; set; }

    [Required(ErrorMessage = "Department name is required.")]
    [StringLength(100, ErrorMessage = "Department name must not exceed 100 characters.")]
    [Display(Name = "Department Name")]
    public string DepartmentName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Manager ID is required.")]
    [StringLength(50, ErrorMessage = "Manager ID must not exceed 50 characters.")]
    [Display(Name = "Manager")]
    public string ManagerId { get; set; } = string.Empty;
}