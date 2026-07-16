using CompanySystem.Business.DTOs;

namespace CompanySystem.Business.Interfaces;

public interface IAuthService
{
    Task<AuthResponseDto> LoginAsync(
        LoginDto dto);


    Task<AuthResponseDto> RefreshTokenAsync(
        string refreshToken);


    Task<bool> LogoutAsync(
        string refreshToken,
        string userId);
}