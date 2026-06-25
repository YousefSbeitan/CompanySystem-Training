using CompanySystem.Business.DTOs;
using CompanySystem.Web.ViewModels;

namespace CompanySystem.Web.Mappers;

public static class DepartmentViewModelMapper
{
    public static DepartmentViewModel ToViewModel(DepartmentDto dto)
    {
        return new DepartmentViewModel
        {
            DepartmentId = dto.DepartmentId,
            DepartmentName = dto.DepartmentName,
            ManagerId = dto.ManagerId,
            CreatedBy = dto.CreatedBy,
            CreatedDate = dto.CreatedDate,
            UpdatedBy = dto.UpdatedBy,
            UpdatedDate = dto.UpdatedDate
        };
    }

    public static CreateDepartmentDto ToCreateDto(CreateDepartmentViewModel viewModel)
    {
        return new CreateDepartmentDto
        {
            DepartmentName = viewModel.DepartmentName,
            ManagerId = viewModel.ManagerId
        };
    }

    public static EditDepartmentDto ToEditDto(EditDepartmentViewModel viewModel)
    {
        return new EditDepartmentDto
        {
            DepartmentId = viewModel.DepartmentId,
            DepartmentName = viewModel.DepartmentName,
            ManagerId = viewModel.ManagerId
        };
    }

    public static EditDepartmentViewModel ToEditViewModel(DepartmentDto dto)
    {
        return new EditDepartmentViewModel
        {
            DepartmentId = dto.DepartmentId,
            DepartmentName = dto.DepartmentName,
            ManagerId = dto.ManagerId
        };
    }
}