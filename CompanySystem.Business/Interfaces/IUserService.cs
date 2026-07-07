using CompanySystem.Business.DTOs;

namespace CompanySystem.Business.Interfaces;

public interface IUserService
{
    Task<IEnumerable<UserDto>> GetAllAsync();

    Task<UserDto?> GetByIdAsync(string userId);

    Task<UserDto> CreateAsync(CreateUserDto dto);

    Task<UserDto?> UpdateAsync(EditUserDto dto);

    Task<bool> DeleteAsync(string userId);
}