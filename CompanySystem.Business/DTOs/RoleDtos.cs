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
    
    public string RoleName { get; set; }
}

public class EditRoleDto
{
    
    public int RoleId { get; set; }

    
    public string RoleName { get; set; }
}