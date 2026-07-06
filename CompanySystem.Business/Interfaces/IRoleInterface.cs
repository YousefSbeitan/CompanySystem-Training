using CompanySystem.Business.DTOs;

namespace CompanySystem.Business.Interfaces;

public interface IRoleService
{
    Task<IEnumerable<RoleDto>> GetAllAsync();

    Task<RoleDto?> GetByIdAsync(int roleId);

    Task<RoleDto> CreateAsync(CreateRoleDto dto);

    Task<RoleDto?> UpdateAsync(EditRoleDto dto);

    Task<bool> DeleteAsync(int roleId);
}