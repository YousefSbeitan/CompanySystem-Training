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

public class EditUserDto
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
}