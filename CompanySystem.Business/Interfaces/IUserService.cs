using CompanySystem.Business.DTOs;
using CompanySystem.Shared.Requests;
using CompanySystem.Shared.Responses;

namespace CompanySystem.Business.Interfaces;

public interface IUserService
{
    Task<PagedResponse<UserDto>> GetAllAsync(
        PaginationFilterRequest request);


    Task<UserDto?> GetByIdAsync(
        string userId);


    Task<UserDto> CreateAsync(
        CreateUserDto dto);


    Task<UserDto?> UpdateAsync(
        EditUserDto dto);


    Task<bool> DeleteAsync(
        string userId);
}