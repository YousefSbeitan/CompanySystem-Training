using CompanySystem.Business.DTOs;
using CompanySystem.Data.Entities;

namespace CompanySystem.Business.Mappers;

public static class PermissionMapper
{
    public static PermissionDto ToDto(Permission permission)
    {
        return new PermissionDto
        {
            PermissionId = permission.PermissionId,
            PermissionName = permission.PermissionName,
            Description = permission.Description,
            CreatedBy = permission.CreatedBy,
            CreatedDate = permission.CreatedDate,
            UpdatedBy = permission.UpdatedBy,
            UpdatedDate = permission.UpdatedDate
        };
    }
}
