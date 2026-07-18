using System.ComponentModel.DataAnnotations;

namespace CompanySystem.Business.DTOs;

public class PermissionDto : TrackingDto
{
    public int PermissionId { get; set; }

    public string PermissionName { get; set; } = string.Empty;

    public string? Description { get; set; }
}

public class AssignUserPermissionsDto
{
    [Required(ErrorMessage = "User ID is required.")]
    [Display(Name = "User")]
    public string UserId { get; set; } = string.Empty;

    [Required(ErrorMessage = "At least one permission is required.")]
    [Display(Name = "Permissions")]
    public List<int> PermissionIds { get; set; } = new();
}
