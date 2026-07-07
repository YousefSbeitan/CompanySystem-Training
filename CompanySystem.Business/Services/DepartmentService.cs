using CompanySystem.Business.DTOs;
using CompanySystem.Business.Interfaces;
using CompanySystem.Business.Mappers;
using CompanySystem.Data.Entities;
using CompanySystem.Data.Repositories.Interfaces;
using CompanySystem.Shared.Exceptions;
using CompanySystem.Shared.Requests;
using CompanySystem.Shared.Responses;

namespace CompanySystem.Business.Services;

public class DepartmentService : IDepartmentService
{
    private readonly IGenericRepository<Department> _repository;

    public DepartmentService(
        IGenericRepository<Department> repository)
    {
        _repository = repository;
    }


    public async Task<PagedResponse<DepartmentDto>> GetAllAsync(
        PaginationFilterRequest request)
    {
        try
        {
            var departments =
                await _repository.FindAsync(
                    d => !d.IsDeleted);


            // Filtering
            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                departments = departments.Where(
                    d => d.DepartmentName.ToLower()
                        .Contains(request.Search.ToLower()));
            }


            // Sorting
            departments = request.SortBy?.ToLower() switch
            {
                "departmentname" => request.IsDescending
                    ? departments.OrderByDescending(d => d.DepartmentName)
                    : departments.OrderBy(d => d.DepartmentName),

                "createddate" => request.IsDescending
                    ? departments.OrderByDescending(d => d.CreatedDate)
                    : departments.OrderBy(d => d.CreatedDate),

                _ => departments.OrderBy(d => d.DepartmentId)
            };


            var totalRecords =
                departments.Count();


            // Pagination
            var pagedDepartments =
                departments
                    .Skip(
                        (request.PageNumber - 1)
                        * request.PageSize)
                    .Take(request.PageSize)
                    .Select(DepartmentMapper.ToDto)
                    .ToList();


            return new PagedResponse<DepartmentDto>(
                pagedDepartments,
                request.PageNumber,
                request.PageSize,
                totalRecords);
        }
        catch (Exception ex)
        {
            throw new BusinessException(
                "Failed to retrieve departments.", ex);
        }
    }


    public async Task<DepartmentDto?> GetByIdAsync(int departmentId)
    {
        try
        {
            var department =
                await _repository.FirstOrDefaultAsync(
                    d => d.DepartmentId == departmentId &&
                         !d.IsDeleted);


            if (department == null)
                throw new ResourceNotFoundException(
                    "Department",
                    departmentId);


            return DepartmentMapper.ToDto(department);
        }
        catch (ResourceNotFoundException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new BusinessException(
                "Failed to retrieve the department.", ex);
        }
    }


    public async Task<DepartmentDto> CreateAsync(
        CreateDepartmentDto dto)
    {
        try
        {
            var existingDepartment =
                await _repository.FirstOrDefaultAsync(
                    d => d.DepartmentName.ToLower()
                         == dto.DepartmentName.ToLower()
                         &&
                         !d.IsDeleted);


            if (existingDepartment != null)
                throw new BusinessException(
                    "Department name already exists.");


            var department =
                DepartmentMapper.ToEntity(dto);


            department.CreatedBy = "System";


            await _repository.AddAsync(department);

            await _repository.SaveChangesAsync();


            return DepartmentMapper.ToDto(department);
        }
        catch (BusinessException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new BusinessException(
                "Failed to create the department.", ex);
        }
    }


    public async Task<DepartmentDto?> UpdateAsync(
        EditDepartmentDto dto)
    {
        try
        {
            var department =
                await _repository.FirstOrDefaultAsync(
                    d => d.DepartmentId == dto.DepartmentId &&
                         !d.IsDeleted);


            if (department == null)
                throw new ResourceNotFoundException(
                    "Department",
                    dto.DepartmentId);


            var existingDepartment =
                await _repository.FirstOrDefaultAsync(
                    d => d.DepartmentName.ToLower()
                         == dto.DepartmentName.ToLower()
                         &&
                         d.DepartmentId != dto.DepartmentId
                         &&
                         !d.IsDeleted);


            if (existingDepartment != null)
                throw new BusinessException(
                    "Department name already exists.");


            DepartmentMapper.UpdateEntity(
                department,
                dto);


            department.UpdatedBy = "System";
            department.UpdatedDate = DateTime.UtcNow;


            _repository.Update(department);

            await _repository.SaveChangesAsync();


            return DepartmentMapper.ToDto(department);
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
                "Failed to update the department.", ex);
        }
    }


    public async Task<bool> DeleteAsync(
        int departmentId)
    {
        try
        {
            var department =
                await _repository.FirstOrDefaultAsync(
                    d => d.DepartmentId == departmentId &&
                         !d.IsDeleted);


            if (department == null)
                throw new ResourceNotFoundException(
                    "Department",
                    departmentId);


            department.IsDeleted = true;

            department.UpdatedBy = "System";
            department.UpdatedDate = DateTime.UtcNow;


            _repository.Update(department);

            await _repository.SaveChangesAsync();


            return true;
        }
        catch (ResourceNotFoundException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new BusinessException(
                "Failed to delete the department.", ex);
        }
    }
}