namespace CompanySystem.Data.Entities;

public class User : TrackingEntity
{
    public string UserId { get; set; }

    public string Username { get; set; }

    public string PasswordHash { get; set; }

    public int RoleId { get; set; }

    public string? LeaderId { get; set; }

    public int? DepartmentId { get; set; }

    public string PhoneNumber { get; set; }

    public DateTime StartDate { get; set; }

    public decimal Salary { get; set; }

    public bool IsActive { get; set; }
}