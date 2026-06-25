using CompanySystem.Business.DTOs;
using CompanySystem.Web.ViewModels;

namespace CompanySystem.Web.Mappers;

public static class RoleViewModelMapper
{
    public static RoleViewModel ToViewModel(RoleDto dto)
    {
        return new RoleViewModel
        {
            RoleId = dto.RoleId,
            RoleName = dto.RoleName,
            CreatedBy = dto.CreatedBy,
            CreatedDate = dto.CreatedDate,
            UpdatedBy = dto.UpdatedBy,
            UpdatedDate = dto.UpdatedDate
        };
    }

    public static CreateRoleDto ToCreateDto(CreateRoleViewModel viewModel)
    {
        return new CreateRoleDto
        {
            RoleName = viewModel.RoleName
        };
    }

    public static EditRoleDto ToEditDto(EditRoleViewModel viewModel)
    {
        return new EditRoleDto
        {
            RoleId = viewModel.RoleId,
            RoleName = viewModel.RoleName
        };
    }

    public static EditRoleViewModel ToEditViewModel(RoleDto dto)
    {
        return new EditRoleViewModel
        {
            RoleId = dto.RoleId,
            RoleName = dto.RoleName
        };
    }
}