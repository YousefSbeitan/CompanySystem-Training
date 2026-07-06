using CompanySystem.Business.DTOs;
using CompanySystem.Data.Entities;

namespace CompanySystem.Business.Mappers;

public static class UserMapper
{
    public static UserDto ToDto(User user)
    {
        return new UserDto
        {
            UserId = user.UserId,
            Username = user.Username,
            RoleId = user.RoleId,
            LeaderId = user.LeaderId,
            DepartmentId = user.DepartmentId,
            PhoneNumber = user.PhoneNumber,
            StartDate = user.StartDate,
            Salary = user.Salary,
            IsActive = user.IsActive,
            CreatedBy = user.CreatedBy,
            CreatedDate = user.CreatedDate,
            UpdatedBy = user.UpdatedBy,
            UpdatedDate = user.UpdatedDate
        };
    }


    public static User ToEntity(CreateUserDto dto)
    {
        return new User
        {
            Username = dto.Username,
            PasswordHash = dto.PasswordHash,
            RoleId = dto.RoleId,
            DepartmentId = dto.DepartmentId,
            PhoneNumber = dto.PhoneNumber,
            StartDate = dto.StartDate,
            Salary = dto.Salary,
            IsActive = dto.IsActive
        };
    }


    public static void UpdateEntity(
        User user,
        EditUserDto dto)
    {
        user.Username = dto.Username;
        user.RoleId = dto.RoleId;
        user.LeaderId = dto.LeaderId;
        user.DepartmentId = dto.DepartmentId;
        user.PhoneNumber = dto.PhoneNumber;
        user.StartDate = dto.StartDate;
        user.Salary = dto.Salary;
        user.IsActive = dto.IsActive;
    }
}