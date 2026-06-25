using System.ComponentModel.DataAnnotations;

namespace CompanySystem.Web.ViewModels;

public class RoleViewModel
{
    public int RoleId { get; set; }

    [Display(Name = "Role Name")]
    public string RoleName { get; set; }

    [Display(Name = "Created By")]
    public string CreatedBy { get; set; } = string.Empty;

    [Display(Name = "Created Date")]
    public DateTime CreatedDate { get; set; }

    [Display(Name = "Updated By")]
    public string? UpdatedBy { get; set; }

    [Display(Name = "Updated Date")]
    public DateTime? UpdatedDate { get; set; }
}

public class CreateRoleViewModel
{
    [Required(ErrorMessage = "Role name is required")]
    [StringLength(100, ErrorMessage = "Role name must not exceed 100 characters")]
    [Display(Name = "Role Name")]
    public string RoleName { get; set; }
}

public class EditRoleViewModel
{
    public int RoleId { get; set; }

    [Required(ErrorMessage = "Role name is required")]
    [StringLength(100, ErrorMessage = "Role name must not exceed 100 characters")]
    [Display(Name = "Role Name")]
    public string RoleName { get; set; }
}