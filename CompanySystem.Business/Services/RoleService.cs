using CompanySystem.Business.DTOs;
using CompanySystem.Business.Interfaces;
using CompanySystem.Business.Mappers;
using CompanySystem.Data.Entities;
using CompanySystem.Data.Repositories.Interfaces;
using CompanySystem.Shared.Exceptions;

namespace CompanySystem.Business.Services;

public class RoleService : IRoleService
{
    private readonly IGenericRepository<Role> _repository;

    public RoleService(IGenericRepository<Role> repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<RoleDto>> GetAllAsync()
    {
        try
        {
            var roles = await _repository.FindAsync(r => !r.IsDeleted);

            return roles.Select(RoleMapper.ToDto);
        }
        catch (Exception ex)
        {
            throw new BusinessException("Failed to retrieve roles.", ex);
        }
    }

    public async Task<RoleDto?> GetByIdAsync(int roleId)
    {
        try
        {
            var role = await _repository.FirstOrDefaultAsync(
                r => r.RoleId == roleId &&
                     !r.IsDeleted);

            if (role == null)
                throw new ResourceNotFoundException("Role", roleId);

            return RoleMapper.ToDto(role);
        }
        catch (ResourceNotFoundException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new BusinessException("Failed to retrieve the role.", ex);
        }
    }

    public async Task<RoleDto> CreateAsync(CreateRoleDto dto)
    {
        try
        {
            var role = RoleMapper.ToEntity(dto);

            role.CreatedBy = "System";

            await _repository.AddAsync(role);

            await _repository.SaveChangesAsync();

            return RoleMapper.ToDto(role);
        }
        catch (Exception ex)
        {
            throw new BusinessException("Failed to create the role.", ex);
        }
    }

    public async Task<RoleDto?> UpdateAsync(EditRoleDto dto)
    {
        try
        {
            var role = await _repository.FirstOrDefaultAsync(
                r => r.RoleId == dto.RoleId &&
                     !r.IsDeleted);

            if (role == null)
                throw new ResourceNotFoundException("Role", dto.RoleId);

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
        catch (Exception ex)
        {
            throw new BusinessException("Failed to update the role.", ex);
        }
    }

    public async Task<bool> DeleteAsync(int roleId)
    {
        try
        {
            var role = await _repository.FirstOrDefaultAsync(
                r => r.RoleId == roleId &&
                     !r.IsDeleted);

            if (role == null)
                throw new ResourceNotFoundException("Role", roleId);

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
        catch (Exception ex)
        {
            throw new BusinessException("Failed to delete the role.", ex);
        }
    }
}