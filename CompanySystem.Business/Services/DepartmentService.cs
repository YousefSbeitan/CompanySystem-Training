using CompanySystem.Business.DTOs;
using CompanySystem.Business.Interfaces;
using CompanySystem.Business.Mappers;
using CompanySystem.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace CompanySystem.Business.Services;

public class DepartmentService : IDepartmentService
{
    private readonly CompanySystemDbContext _context;

    public DepartmentService(CompanySystemDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<DepartmentDto>> GetAllAsync()
    {
        var departments = await _context.Departments
            .Where(d => !d.IsDeleted)
            .ToListAsync();

        return departments.Select(DepartmentMapper.ToDto);
    }

    public async Task<DepartmentDto?> GetByIdAsync(int departmentId)
    {
        var department = await _context.Departments
            .FirstOrDefaultAsync(d =>
                d.DepartmentId == departmentId &&
                !d.IsDeleted);

        if (department == null)
            return null;

        return DepartmentMapper.ToDto(department);
    }

    public async Task<DepartmentDto> CreateAsync(CreateDepartmentDto dto)
    {
        var department = DepartmentMapper.ToEntity(dto);

        department.CreatedBy = "System";

        _context.Departments.Add(department);

        await _context.SaveChangesAsync();

        return DepartmentMapper.ToDto(department);
    }

    public async Task<DepartmentDto?> UpdateAsync(EditDepartmentDto dto)
    {
        var department = await _context.Departments
            .FirstOrDefaultAsync(d =>
                d.DepartmentId == dto.DepartmentId &&
                !d.IsDeleted);

        if (department == null)
            return null;

        DepartmentMapper.UpdateEntity(department, dto);

        department.UpdatedBy = "System";
        department.UpdatedDate = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return DepartmentMapper.ToDto(department);
    }

    public async Task<bool> DeleteAsync(int departmentId)
    {
        var department = await _context.Departments
            .FirstOrDefaultAsync(d =>
                d.DepartmentId == departmentId &&
                !d.IsDeleted);

        if (department == null)
            return false;

        department.IsDeleted = true;
        department.UpdatedBy = "System";
        department.UpdatedDate = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return true;
    }
}