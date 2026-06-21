namespace CompanySystem.Data.Entities;

public class User
{
    public int UserId { get; set; }

    public string Username { get; set; }

    public string PasswordHash { get; set; }

    public int RoleId { get; set; }

    public int? LeaderId { get; set; }

    public int? DepartmentId { get; set; }

    public string PhoneNumber { get; set; }

    public int YearsOfExperience { get; set; }

    public decimal Salary { get; set; }

    public bool IsActive { get; set; }
}