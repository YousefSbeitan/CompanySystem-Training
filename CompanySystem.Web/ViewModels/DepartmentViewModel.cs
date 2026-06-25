using System.ComponentModel.DataAnnotations;

namespace CompanySystem.Web.ViewModels;

public class DepartmentViewModel
{
    public int DepartmentId { get; set; }

    [Display(Name = "Department Name")]
    public string DepartmentName { get; set; } = string.Empty;

    [Display(Name = "Manager ID")]
    public string ManagerId { get; set; } = string.Empty;

    [Display(Name = "Created By")]
    public string CreatedBy { get; set; } = string.Empty;

    [Display(Name = "Created Date")]
    public DateTime CreatedDate { get; set; }

    [Display(Name = "Updated By")]
    public string? UpdatedBy { get; set; }

    [Display(Name = "Updated Date")]
    public DateTime? UpdatedDate { get; set; }
}

public class CreateDepartmentViewModel
{
    [Required(ErrorMessage = "Department name is required")]
    [StringLength(100, ErrorMessage = "Department name must not exceed 100 characters")]
    [Display(Name = "Department Name")]
    public string DepartmentName { get; set; }

    [Required(ErrorMessage = "Manager ID is required")]
    [StringLength(50, ErrorMessage = "Manager ID must not exceed 50 characters")]
    [Display(Name = "Manager ID")]
    public string ManagerId { get; set; }
}

public class EditDepartmentViewModel
{
    public int DepartmentId { get; set; }

    [Required(ErrorMessage = "Department name is required")]
    [StringLength(100, ErrorMessage = "Department name must not exceed 100 characters")]
    [Display(Name = "Department Name")]
    public string DepartmentName { get; set; }

    [Required(ErrorMessage = "Manager ID is required")]
    [StringLength(50, ErrorMessage = "Manager ID must not exceed 50 characters")]
    [Display(Name = "Manager ID")]
    public string ManagerId { get; set; }
}