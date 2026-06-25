using CompanySystem.Business.DTOs;
using CompanySystem.Web.ViewModels;

namespace CompanySystem.Web.Mappers;

public static class UserViewModelMapper
{
    public static UserViewModel ToViewModel(UserDto dto)
    {
        return new UserViewModel
        {
            UserId = dto.UserId,
            Username = dto.Username,
            RoleId = dto.RoleId,
            LeaderId = dto.LeaderId,
            DepartmentId = dto.DepartmentId,
            PhoneNumber = dto.PhoneNumber,
            StartDate = dto.StartDate,
            Salary = dto.Salary,
            IsActive = dto.IsActive,
            CreatedBy = dto.CreatedBy,
            CreatedDate = dto.CreatedDate,
            UpdatedBy = dto.UpdatedBy,
            UpdatedDate = dto.UpdatedDate
        };
    }

    public static CreateUserDto ToCreateDto(CreateUserViewModel viewModel)
    {
        return new CreateUserDto
        {
            UserId = viewModel.UserId,
            Username = viewModel.Username,
            PasswordHash = viewModel.PasswordHash,
            RoleId = viewModel.RoleId,
            LeaderId = viewModel.LeaderId,
            DepartmentId = viewModel.DepartmentId,
            PhoneNumber = viewModel.PhoneNumber,
            StartDate = viewModel.StartDate,
            Salary = viewModel.Salary,
            IsActive = viewModel.IsActive
        };
    }

    public static EditUserDto ToEditDto(EditUserViewModel viewModel)
    {
        return new EditUserDto
        {
            UserId = viewModel.UserId,
            Username = viewModel.Username,
            RoleId = viewModel.RoleId,
            LeaderId = viewModel.LeaderId,
            DepartmentId = viewModel.DepartmentId,
            PhoneNumber = viewModel.PhoneNumber,
            StartDate = viewModel.StartDate,
            Salary = viewModel.Salary,
            IsActive = viewModel.IsActive
        };
    }

    public static EditUserViewModel ToEditViewModel(UserDto dto)
    {
        return new EditUserViewModel
        {
            UserId = dto.UserId,
            Username = dto.Username,
            RoleId = dto.RoleId,
            LeaderId = dto.LeaderId,
            DepartmentId = dto.DepartmentId,
            PhoneNumber = dto.PhoneNumber,
            StartDate = dto.StartDate,
            Salary = dto.Salary,
            IsActive = dto.IsActive
        };
    }
}