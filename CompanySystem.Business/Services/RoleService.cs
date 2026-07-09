using CompanySystem.Business.DTOs;
using CompanySystem.Business.Interfaces;
using CompanySystem.Business.Mappers;
using CompanySystem.Data.Entities;
using CompanySystem.Data.Repositories.Interfaces;
using CompanySystem.Shared.Exceptions;
using CompanySystem.Shared.Requests;
using CompanySystem.Shared.Responses;

namespace CompanySystem.Business.Services;

public class RoleService : IRoleService
{
    private readonly IGenericRepository<Role> _repository;

    public RoleService(IGenericRepository<Role> repository)
    {
        _repository = repository;
    }


    public async Task<PagedResponse<RoleDto>> GetAllAsync(
        PaginationFilterRequest request)
    {
        try
        {
            var roles = await _repository.FindAsync(
                r => !r.IsDeleted);


            // Filtering
            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                roles = roles.Where(
                    r => r.RoleName.ToLower()
                    .Contains(request.Search.ToLower()));
            }


            // Sorting
            roles = request.SortBy?.ToLower() switch
            {
                "rolename" => request.IsDescending
                    ? roles.OrderByDescending(r => r.RoleName)
                    : roles.OrderBy(r => r.RoleName),

                "createddate" => request.IsDescending
                    ? roles.OrderByDescending(r => r.CreatedDate)
                    : roles.OrderBy(r => r.CreatedDate),

                _ => roles.OrderBy(r => r.RoleId)
            };


            var totalRecords = roles.Count();


            // Pagination
            var pagedRoles = roles
                .Skip(
                    (request.PageNumber - 1)
                    * request.PageSize)
                .Take(request.PageSize)
                .Select(RoleMapper.ToDto)
                .ToList();


            return new PagedResponse<RoleDto>(
                pagedRoles,
                request.PageNumber,
                request.PageSize,
                totalRecords);
        }
        catch (Exception ex)
        {
            throw new BusinessException(
                "Failed to retrieve roles.", ex);
        }
    }

    public async Task<RoleDto?> GetByIdAsync(int roleId)
    {
        try
        {
            if (roleId <= 0)
                throw new BusinessException(
                    "Invalid role id.");

            var role = await _repository.FirstOrDefaultAsync(
                r => r.RoleId == roleId &&
                     !r.IsDeleted);

            if (role == null)
                throw new ResourceNotFoundException(
                    "Role", roleId);


            return RoleMapper.ToDto(role);
        }
        catch (ResourceNotFoundException)
        {
            throw;
        }
        catch (BusinessException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new BusinessException(
                "Failed to retrieve the role.", ex);
        }
    }

    public async Task<RoleDto> CreateAsync(CreateRoleDto dto)
    {
        try
        {
            if (dto == null)
                throw new BusinessException(
                    "Role data is required.");

            if (string.IsNullOrWhiteSpace(dto.RoleName))
                throw new BusinessException(
                    "Role name is required.");

            dto.RoleName = dto.RoleName.Trim();

            var existingRole =
                await _repository.FirstOrDefaultAsync(
                    r => r.RoleName.ToLower()
                         == dto.RoleName.ToLower()
                         &&
                         !r.IsDeleted);

            if (existingRole != null)
                throw new BusinessException(
                    "Role name already exists.");

            var role = RoleMapper.ToEntity(dto);


            role.CreatedBy = "System";

            await _repository.AddAsync(role);

            await _repository.SaveChangesAsync();

            return RoleMapper.ToDto(role);
        }
        catch (BusinessException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new BusinessException(
                "Failed to create the role.", ex);
        }
    }

    public async Task<RoleDto?> UpdateAsync(EditRoleDto dto)
    {
        try
        {
            if (dto == null)
                throw new BusinessException(
                    "Role data is required.");

            if (dto.RoleId <= 0)
                throw new BusinessException(
                    "Invalid role id.");

            if (string.IsNullOrWhiteSpace(dto.RoleName))
                throw new BusinessException(
                    "Role name is required.");

            dto.RoleName = dto.RoleName.Trim();

            var role =
                await _repository.FirstOrDefaultAsync(
                    r => r.RoleId == dto.RoleId &&
                         !r.IsDeleted);


            if (role == null)
                throw new ResourceNotFoundException(
                    "Role", dto.RoleId);

            var existingRole =
                await _repository.FirstOrDefaultAsync(
                    r => r.RoleName.ToLower()
                         == dto.RoleName.ToLower()
                         &&
                         r.RoleId != dto.RoleId
                         &&
                         !r.IsDeleted);


            if (existingRole != null)
                throw new BusinessException(
                    "Role name already exists.");

            RoleMapper.UpdateEntity(role, dto);

            role.UpdatedBy = "System";
            role.UpdatedDate = DateTime.UtcNow;

            _repository.Update(role);

            await _repository.SaveChangesAsync();

            return RoleMapper.ToDto(role);
        }
        catch (ResourceNotFoundException)
        {
            throw;
        }
        catch (BusinessException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new BusinessException(
                "Failed to update the role.", ex);
        }
    }

    public async Task<bool> DeleteAsync(int roleId)
    {
        try
        {
            if (roleId <= 0)
                throw new BusinessException(
                    "Invalid role id.");

            var role =
                await _repository.FirstOrDefaultAsync(
                    r => r.RoleId == roleId &&
                         !r.IsDeleted);


            if (role == null)
                throw new ResourceNotFoundException(
                    "Role", roleId);

            role.IsDeleted = true;

            role.UpdatedBy = "System";
            role.UpdatedDate = DateTime.UtcNow;

            _repository.Update(role);

            await _repository.SaveChangesAsync();

            return true;
        }
        catch (ResourceNotFoundException)
        {
            throw;
        }
        catch (BusinessException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new BusinessException(
                "Failed to delete the role.", ex);
        }
    }
}