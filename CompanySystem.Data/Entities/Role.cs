namespace CompanySystem.Data.Entities;

public class Role : TrackingEntity
{
    public int RoleId { get; set; }

    public string RoleName { get; set; }
}