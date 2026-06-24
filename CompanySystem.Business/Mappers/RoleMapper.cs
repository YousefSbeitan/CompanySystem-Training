using CompanySystem.Business.DTOs;
using CompanySystem.Data.Entities;

namespace CompanySystem.Business.Mappers;

public static class RoleMapper
{
    public static RoleDto ToDto(Role role)
    {
        return new RoleDto
        {
            RoleId = role.RoleId,
            RoleName = role.RoleName,
            CreatedBy = role.CreatedBy,
            CreatedDate = role.CreatedDate,
            UpdatedBy = role.UpdatedBy,
            UpdatedDate = role.UpdatedDate
        };
    }

    public static Role ToEntity(CreateRoleDto dto)
    {
        return new Role
        {
            RoleName = dto.RoleName
        };
    }

    public static void UpdateEntity(Role role, EditRoleDto dto)
    {
        role.RoleName = dto.RoleName;
    }
}