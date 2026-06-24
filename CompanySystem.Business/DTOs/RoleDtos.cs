using System.ComponentModel.DataAnnotations;

namespace CompanySystem.Business.DTOs;

public class RoleDto
{
    public int RoleId { get; set; }

    public string RoleName { get; set; }

    public string CreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }
}

public class CreateRoleDto
{
    [Required(ErrorMessage = "Role name is required")]
    [StringLength(100, ErrorMessage = "Role name must not exceed 100 characters")]
    public string RoleName { get; set; }
}

public class EditRoleDto
{
    [Required]
    public int RoleId { get; set; }

    [Required(ErrorMessage = "Role name is required")]
    [StringLength(100, ErrorMessage = "Role name must not exceed 100 characters")]
    public string RoleName { get; set; }
}