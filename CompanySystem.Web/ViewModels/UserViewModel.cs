using System.ComponentModel.DataAnnotations;

namespace CompanySystem.Web.ViewModels;

public class UserViewModel
{
    public string UserId { get; set; } = string.Empty;

    [Display(Name = "Username")]
    public string Username { get; set; } = string.Empty;

    [Display(Name = "Role")]
    public int RoleId { get; set; }

    [Display(Name = "Leader")]
    public string? LeaderId { get; set; }

    [Display(Name = "Department")]
    public int? DepartmentId { get; set; }

    [Display(Name = "Phone Number")]
    public string PhoneNumber { get; set; } = string.Empty;

    [Display(Name = "Start Date")]
    public DateTime StartDate { get; set; }

    [Display(Name = "Salary")]
    public decimal Salary { get; set; }

    [Display(Name = "Active")]
    public bool IsActive { get; set; }

    [Display(Name = "Created By")]
    public string CreatedBy { get; set; } = string.Empty;

    [Display(Name = "Created Date")]
    public DateTime CreatedDate { get; set; }

    [Display(Name = "Updated By")]
    public string? UpdatedBy { get; set; }

    [Display(Name = "Updated Date")]
    public DateTime? UpdatedDate { get; set; }
}

public class CreateUserViewModel
{
    [Required]
    [StringLength(50)]
    [Display(Name = "User ID")]
    public string UserId { get; set; }

    [Required]
    [StringLength(100)]
    [Display(Name = "Username")]
    public string Username { get; set; }

    [Required]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string PasswordHash { get; set; }

    [Display(Name = "Role")]
    public int RoleId { get; set; }

    [Display(Name = "Leader")]
    public string? LeaderId { get; set; }

    [Display(Name = "Department")]
    public int? DepartmentId { get; set; }

    [Required]
    [Phone]
    [Display(Name = "Phone Number")]
    public string PhoneNumber { get; set; }

    [Display(Name = "Start Date")]
    public DateTime StartDate { get; set; }

    [Range(0, double.MaxValue)]
    [Display(Name = "Salary")]
    public decimal Salary { get; set; }

    [Display(Name = "Active")]
    public bool IsActive { get; set; }
}

public class EditUserViewModel
{
    public string UserId { get; set; }

    [Required]
    [StringLength(100)]
    [Display(Name = "Username")]
    public string Username { get; set; }

    [Display(Name = "Role")]
    public int RoleId { get; set; }

    [Display(Name = "Leader")]
    public string? LeaderId { get; set; }

    [Display(Name = "Department")]
    public int? DepartmentId { get; set; }

    [Required]
    [Phone]
    [Display(Name = "Phone Number")]
    public string PhoneNumber { get; set; }

    [Display(Name = "Start Date")]
    public DateTime StartDate { get; set; }

    [Range(0, double.MaxValue)]
    [Display(Name = "Salary")]
    public decimal Salary { get; set; }

    [Display(Name = "Active")]
    public bool IsActive { get; set; }
}