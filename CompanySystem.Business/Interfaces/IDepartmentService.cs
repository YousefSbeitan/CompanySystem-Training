using CompanySystem.Business.DTOs;

namespace CompanySystem.Business.Interfaces;

public interface IDepartmentService
{
    Task<IEnumerable<DepartmentDto>> GetAllAsync();

    Task<DepartmentDto?> GetByIdAsync(int departmentId);

    Task<DepartmentDto> CreateAsync(CreateDepartmentDto dto);

    Task<DepartmentDto?> UpdateAsync(EditDepartmentDto dto);

    Task<bool> DeleteAsync(int departmentId);
}