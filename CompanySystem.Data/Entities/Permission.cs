using CompanySystem.Data.Entities;

namespace CompanySystem.Data.Entities;

public class Permission : TrackingEntity
{
    public int PermissionId { get; set; }

    public string PermissionName { get; set; }

    public string? Description { get; set; }
}
