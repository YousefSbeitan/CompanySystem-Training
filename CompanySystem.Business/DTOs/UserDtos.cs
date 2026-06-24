using System.ComponentModel.DataAnnotations;

namespace CompanySystem.Business.DTOs;

public class UserDto
{
    public string UserId { get; set; }

    public string Username { get; set; }

    public int RoleId { get; set; }

    public string? LeaderId { get; set; }

    public int? DepartmentId { get; set; }

    public string PhoneNumber { get; set; }

    public DateTime StartDate { get; set; }

    public decimal Salary { get; set; }

    public bool IsActive { get; set; }

    public string CreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }
}

public class CreateUserDto
{
    [Required(ErrorMessage = "User ID is required")]
    [StringLength(50, ErrorMessage = "User ID must not exceed 50 characters")]
    public string UserId { get; set; }

    [Required(ErrorMessage = "Username is required")]
    [StringLength(100, ErrorMessage = "Username must not exceed 100 characters")]
    public string Username { get; set; }

    [Required(ErrorMessage = "Password is required")]
    public string PasswordHash { get; set; }

    [Required(ErrorMessage = "Role is required")]
    public int RoleId { get; set; }

    [StringLength(50, ErrorMessage = "Leader ID must not exceed 50 characters")]
    public string? LeaderId { get; set; }

    public int? DepartmentId { get; set; }

    [Required(ErrorMessage = "Phone number is required")]
    [StringLength(20, ErrorMessage = "Phone number must not exceed 20 characters")]
    public string PhoneNumber { get; set; }

    [Required(ErrorMessage = "Start date is required")]
    public DateTime StartDate { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Salary must be greater than or equal to 0")]
    public decimal Salary { get; set; }

    public bool IsActive { get; set; }
}

public class EditUserDto
{
    [Required(ErrorMessage = "User ID is required")]
    [StringLength(50, ErrorMessage = "User ID must not exceed 50 characters")]
    public string UserId { get; set; }

    [Required(ErrorMessage = "Username is required")]
    [StringLength(100, ErrorMessage = "Username must not exceed 100 characters")]
    public string Username { get; set; }

    [Required(ErrorMessage = "Role is required")]
    public int RoleId { get; set; }

    [StringLength(50, ErrorMessage = "Leader ID must not exceed 50 characters")]
    public string? LeaderId { get; set; }

    public int? DepartmentId { get; set; }

    [Required(ErrorMessage = "Phone number is required")]
    [StringLength(20, ErrorMessage = "Phone number must not exceed 20 characters")]
    public string PhoneNumber { get; set; }

    [Required(ErrorMessage = "Start date is required")]
    public DateTime StartDate { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Salary must be greater than or equal to 0")]
    public decimal Salary { get; set; }

    public bool IsActive { get; set; }
}