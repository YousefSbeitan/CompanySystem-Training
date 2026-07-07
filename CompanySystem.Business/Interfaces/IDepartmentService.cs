using CompanySystem.Business.DTOs;
using CompanySystem.Shared.Requests;
using CompanySystem.Shared.Responses;

namespace CompanySystem.Business.Interfaces;

public interface IDepartmentService
{
    Task<PagedResponse<DepartmentDto>> GetAllAsync(
        PaginationFilterRequest request);


    Task<DepartmentDto?> GetByIdAsync(
        int departmentId);


    Task<DepartmentDto> CreateAsync(
        CreateDepartmentDto dto);


    Task<DepartmentDto?> UpdateAsync(
        EditDepartmentDto dto);


    Task<bool> DeleteAsync(
        int departmentId);
}