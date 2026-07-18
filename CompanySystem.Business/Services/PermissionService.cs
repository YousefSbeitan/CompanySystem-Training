using CompanySystem.Business.DTOs;
using CompanySystem.Business.Interfaces;
using CompanySystem.Business.Mappers;
using CompanySystem.Data.Entities;
using CompanySystem.Data.Repositories.Interfaces;
using CompanySystem.Shared.Exceptions;
using CompanySystem.Shared.Requests;
using CompanySystem.Shared.Responses;

namespace CompanySystem.Business.Services;

public class PermissionService : IPermissionService
{
    private readonly IGenericRepository<Permission> _permissionRepository;
    private readonly IGenericRepository<UserPermission> _userPermissionRepository;
    private readonly IGenericRepository<User> _userRepository;

    public PermissionService(
        IGenericRepository<Permission> permissionRepository,
        IGenericRepository<UserPermission> userPermissionRepository,
        IGenericRepository<User> userRepository)
    {
        _permissionRepository = permissionRepository;
        _userPermissionRepository = userPermissionRepository;
        _userRepository = userRepository;
    }

    public async Task<PagedResponse<PermissionDto>> GetAllAsync(
        PaginationFilterRequest request)
    {
        try
        {
            var permissions = await _permissionRepository.FindAsync(
                p => !p.IsDeleted);

            // Filtering
            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                permissions = permissions.Where(
                    p => p.PermissionName.ToLower()
                        .Contains(request.Search.ToLower()));
            }

            // Sorting
            permissions = request.SortBy?.ToLower() switch
            {
                "permissionname" => request.IsDescending
                    ? permissions.OrderByDescending(p => p.PermissionName)
                    : permissions.OrderBy(p => p.PermissionName),

                "createddate" => request.IsDescending
                    ? permissions.OrderByDescending(p => p.CreatedDate)
                    : permissions.OrderBy(p => p.CreatedDate),

                _ => permissions.OrderBy(p => p.PermissionId)
            };

            var totalRecords = permissions.Count();

            var pagedPermissions = permissions
                .Skip(
                    (request.PageNumber - 1)
                    * request.PageSize)
                .Take(request.PageSize)
                .Select(PermissionMapper.ToDto)
                .ToList();

            return new PagedResponse<PermissionDto>(
                pagedPermissions,
                request.PageNumber,
                request.PageSize,
                totalRecords);
        }
        catch (Exception ex)
        {
            throw new BusinessException(
                "Failed to retrieve permissions.", ex);
        }
    }

    public async Task<List<PermissionDto>> GetUserPermissionsAsync(
        string userId)
    {
        if (string.IsNullOrWhiteSpace(userId))
            throw new BusinessException("User id is required.");

        try
        {
            var userPermissions =
                await _userPermissionRepository.FindAsync(
                    up => up.UserId == userId && !up.IsDeleted);

            var permissionIds = userPermissions
                .Select(up => up.PermissionId)
                .ToList();

            var permissions = await _permissionRepository.FindAsync(
                p => permissionIds.Contains(p.PermissionId) && !p.IsDeleted);

            return permissions
                .Select(PermissionMapper.ToDto)
                .ToList();
        }
        catch (Exception ex) when (ex is not BusinessException)
        {
            throw new BusinessException(
                "Failed to retrieve user permissions.", ex);
        }
    }

    public async Task AssignToUserAsync(
        AssignUserPermissionsDto dto)
    {
        if (dto == null)
            throw new BusinessException("Permission data is required.");

        if (string.IsNullOrWhiteSpace(dto.UserId))
            throw new BusinessException("User id is required.");

        if (dto.PermissionIds == null || dto.PermissionIds.Count == 0)
            throw new BusinessException("At least one permission is required.");

        dto.PermissionIds = dto.PermissionIds.Distinct().ToList();

        try
        {
            // Validate user exists
            var user = await _userRepository.FirstOrDefaultAsync(
                u => u.UserId == dto.UserId && !u.IsDeleted);

            if (user == null)
                throw new ResourceNotFoundException("User", dto.UserId);

            // Validate all permission ids exist
            var existingPermissions = await _permissionRepository.FindAsync(
                p => dto.PermissionIds.Contains(p.PermissionId) && !p.IsDeleted);

            var existingIds = existingPermissions
                .Select(p => p.PermissionId)
                .ToList();

            var invalidIds = dto.PermissionIds
                .Where(id => !existingIds.Contains(id))
                .ToList();

            if (invalidIds.Count > 0)
                throw new BusinessException(
                    $"Invalid permission IDs: {string.Join(", ", invalidIds)}");

            // Remove existing permissions for the user
            var existingUserPermissions =
                await _userPermissionRepository.FindAsync(
                    up => up.UserId == dto.UserId && !up.IsDeleted);

            foreach (var permission in existingUserPermissions)
            {
                permission.IsDeleted = true;
                permission.UpdatedBy = "System";
                permission.UpdatedDate = DateTime.UtcNow;

                _userPermissionRepository.Update(permission);
            }

            // Add new permissions
            foreach (var permissionId in dto.PermissionIds)
            {
                var userPermission = new UserPermission
                {
                    UserId = dto.UserId,
                    PermissionId = permissionId,
                    CreatedBy = "System"
                };

                await _userPermissionRepository.AddAsync(userPermission);
            }

            await _userPermissionRepository.SaveChangesAsync();
        }
        catch (Exception ex) when (ex is not BusinessException && ex is not ResourceNotFoundException)
        {
            throw new BusinessException(
                "Failed to assign permissions.", ex);
        }
    }
}
