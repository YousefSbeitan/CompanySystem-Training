using CompanySystem.Data.Entities;

namespace CompanySystem.Data.Entities;

public class UserPermission : TrackingEntity
{
    public int UserPermissionId { get; set; }

    public string UserId { get; set; } = string.Empty;

    public int PermissionId { get; set; }
}
