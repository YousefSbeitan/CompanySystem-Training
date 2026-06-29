using CompanySystem.Business.DTOs;
using CompanySystem.Business.Interfaces;
using CompanySystem.Business.Mappers;
using CompanySystem.Data.Entities;
using CompanySystem.Data.Repositories.Interfaces;

namespace CompanySystem.Business.Services;

public class RoleService : IRoleService
{
    private readonly IGenericRepository<Role> _repository;

    public RoleService(
        IGenericRepository<Role> repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<RoleDto>> GetAllAsync()
    {
        var roles = await _repository.FindAsync(r => !r.IsDeleted);

        return roles.Select(RoleMapper.ToDto);
    }

    public async Task<RoleDto?> GetByIdAsync(int roleId)
    {
        var role = await _repository.FirstOrDefaultAsync(
            r => r.RoleId == roleId &&
                 !r.IsDeleted);

        if (role == null)
            return null;

        return RoleMapper.ToDto(role);
    }

    public async Task<RoleDto> CreateAsync(CreateRoleDto dto)
    {
        var role = RoleMapper.ToEntity(dto);

        role.CreatedBy = "System";

        await _repository.AddAsync(role);

        await _repository.SaveChangesAsync();

        return RoleMapper.ToDto(role);
    }

    public async Task<RoleDto?> UpdateAsync(EditRoleDto dto)
    {
        var role = await _repository.FirstOrDefaultAsync(
            r => r.RoleId == dto.RoleId &&
                 !r.IsDeleted);

        if (role == null)
            return null;

        RoleMapper.UpdateEntity(role, dto);

        role.UpdatedBy = "System";
        role.UpdatedDate = DateTime.UtcNow;

        _repository.Update(role);

        await _repository.SaveChangesAsync();

        return RoleMapper.ToDto(role);
    }

    public async Task<bool> DeleteAsync(int roleId)
    {
        var role = await _repository.FirstOrDefaultAsync(
            r => r.RoleId == roleId &&
                 !r.IsDeleted);

        if (role == null)
            return false;

        role.IsDeleted = true;
        role.UpdatedBy = "System";
        role.UpdatedDate = DateTime.UtcNow;

        _repository.Update(role);

        await _repository.SaveChangesAsync();

        return true;
    }
}