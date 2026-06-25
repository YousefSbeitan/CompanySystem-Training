using CompanySystem.Business.DTOs;
using CompanySystem.Business.Interfaces;
using CompanySystem.Business.Mappers;
using CompanySystem.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace CompanySystem.Business.Services;

public class RoleService : IRoleService
{
    private readonly CompanySystemDbContext _context;

    public RoleService(CompanySystemDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<RoleDto>> GetAllAsync()
    {
        var roles = await _context.Roles
            .Where(r => !r.IsDeleted)
            .ToListAsync();

        return roles.Select(RoleMapper.ToDto);
    }

    public async Task<RoleDto?> GetByIdAsync(int roleId)
    {
        var role = await _context.Roles
            .FirstOrDefaultAsync(r =>
                r.RoleId == roleId &&
                !r.IsDeleted);

        if (role == null)
            return null;

        return RoleMapper.ToDto(role);
    }

    public async Task<RoleDto> CreateAsync(CreateRoleDto dto)
    {
        var role = RoleMapper.ToEntity(dto);

        role.CreatedBy = "System";

        _context.Roles.Add(role);

        await _context.SaveChangesAsync();

        return RoleMapper.ToDto(role);
    }

    public async Task<RoleDto?> UpdateAsync(EditRoleDto dto)
    {
        var role = await _context.Roles
            .FirstOrDefaultAsync(r =>
                r.RoleId == dto.RoleId &&
                !r.IsDeleted);

        if (role == null)
            return null;

        RoleMapper.UpdateEntity(role, dto);

        role.UpdatedBy = "System";
        role.UpdatedDate = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return RoleMapper.ToDto(role);
    }

    public async Task<bool> DeleteAsync(int roleId)
    {
        var role = await _context.Roles
            .FirstOrDefaultAsync(r =>
                r.RoleId == roleId &&
                !r.IsDeleted);

        if (role == null)
            return false;

        role.IsDeleted = true;
        role.UpdatedBy = "System";
        role.UpdatedDate = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return true;
    }
}