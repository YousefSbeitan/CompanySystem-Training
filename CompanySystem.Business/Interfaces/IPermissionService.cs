using CompanySystem.Business.DTOs;
using CompanySystem.Shared.Requests;
using CompanySystem.Shared.Responses;

namespace CompanySystem.Business.Interfaces;

public interface IPermissionService
{
    Task<PagedResponse<PermissionDto>> GetAllAsync(
        PaginationFilterRequest request);

    Task<List<PermissionDto>> GetUserPermissionsAsync(
        string userId);

    Task AssignToUserAsync(
        AssignUserPermissionsDto dto);
}
