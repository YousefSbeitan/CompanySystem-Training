using CompanySystem.Business.DTOs;
using CompanySystem.Data.Entities;

namespace CompanySystem.Business.Mappers;

public static class DepartmentMapper
{
    public static DepartmentDto ToDto(Department department)
    {
        return new DepartmentDto
        {
            DepartmentId = department.DepartmentId,
            DepartmentName = department.DepartmentName,
            ManagerId = department.ManagerId,
            CreatedBy = department.CreatedBy,
            CreatedDate = department.CreatedDate,
            UpdatedBy = department.UpdatedBy,
            UpdatedDate = department.UpdatedDate
        };
    }


    public static Department ToEntity(CreateDepartmentDto dto)
    {
        return new Department
        {
            DepartmentName = dto.DepartmentName,
            ManagerId = dto.ManagerId
        };
    }


    public static void UpdateEntity(
        Department department,
        EditDepartmentDto dto)
    {
        department.DepartmentName = dto.DepartmentName;
        department.ManagerId = dto.ManagerId;
    }
}