using CompanySystem.Business.DTOs;
using CompanySystem.Shared.Requests;
using CompanySystem.Shared.Responses;

namespace CompanySystem.Business.Interfaces;

public interface IRoleService
{
    Task<PagedResponse<RoleDto>> GetAllAsync(
        PaginationFilterRequest request);


    Task<RoleDto?> GetByIdAsync(
        int roleId);


    Task<RoleDto> CreateAsync(
        CreateRoleDto dto);


    Task<RoleDto?> UpdateAsync(
        EditRoleDto dto);


    Task<bool> DeleteAsync(
        int roleId);
}