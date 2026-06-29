using CompanySystem.Business.DTOs;
using CompanySystem.Business.Interfaces;
using CompanySystem.Business.Mappers;
using CompanySystem.Data.Entities;
using CompanySystem.Data.Repositories.Interfaces;

namespace CompanySystem.Business.Services;

public class DepartmentService : IDepartmentService
{
    private readonly IGenericRepository<Department> _repository;

    public DepartmentService(
        IGenericRepository<Department> repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<DepartmentDto>> GetAllAsync()
    {
        var departments = await _repository.FindAsync(
            d => !d.IsDeleted);

        return departments.Select(DepartmentMapper.ToDto);
    }

    public async Task<DepartmentDto?> GetByIdAsync(int departmentId)
    {
        var department = await _repository.FirstOrDefaultAsync(
            d => d.DepartmentId == departmentId &&
                 !d.IsDeleted);

        if (department == null)
            return null;

        return DepartmentMapper.ToDto(department);
    }

    public async Task<DepartmentDto> CreateAsync(CreateDepartmentDto dto)
    {
        var department = DepartmentMapper.ToEntity(dto);

        department.CreatedBy = "System";

        await _repository.AddAsync(department);

        await _repository.SaveChangesAsync();

        return DepartmentMapper.ToDto(department);
    }

    public async Task<DepartmentDto?> UpdateAsync(EditDepartmentDto dto)
    {
        var department = await _repository.FirstOrDefaultAsync(
            d => d.DepartmentId == dto.DepartmentId &&
                 !d.IsDeleted);

        if (department == null)
            return null;

        DepartmentMapper.UpdateEntity(department, dto);

        department.UpdatedBy = "System";
        department.UpdatedDate = DateTime.UtcNow;

        _repository.Update(department);

        await _repository.SaveChangesAsync();

        return DepartmentMapper.ToDto(department);
    }

    public async Task<bool> DeleteAsync(int departmentId)
    {
        var department = await _repository.FirstOrDefaultAsync(
            d => d.DepartmentId == departmentId &&
                 !d.IsDeleted);

        if (department == null)
            return false;

        department.IsDeleted = true;
        department.UpdatedBy = "System";
        department.UpdatedDate = DateTime.UtcNow;

        _repository.Update(department);

        await _repository.SaveChangesAsync();

        return true;
    }
}