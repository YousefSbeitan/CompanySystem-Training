using CompanySystem.Business.DTOs;
using CompanySystem.Business.Interfaces;
using CompanySystem.Business.Services.Security;
using CompanySystem.Data.Entities;
using CompanySystem.Data.Repositories.Interfaces;
using CompanySystem.Shared.Exceptions;
using CompanySystem.Shared.Helpers;

namespace CompanySystem.Business.Services;

public class AuthService : IAuthService
{
    private readonly IGenericRepository<User> _userRepository;
    private readonly IGenericRepository<Role> _roleRepository;
    private readonly IGenericRepository<RefreshToken> _refreshTokenRepository;
    private readonly JwtService _jwtService;


    public AuthService(
        IGenericRepository<User> userRepository,
        IGenericRepository<Role> roleRepository,
        IGenericRepository<RefreshToken> refreshTokenRepository,
        JwtService jwtService)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _refreshTokenRepository = refreshTokenRepository;
        _jwtService = jwtService;
    }


    public async Task<AuthResponseDto> RegisterAsync(
    RegisterDto dto)
    {
        try
        {
            if (dto == null)
                throw new BusinessException(
                    "User data is required.");


            var existingUser =
                await _userRepository.FirstOrDefaultAsync(
                    u =>
                    (
                        u.Username.ToLower()
                        == dto.Username.ToLower()
                        ||
                        u.PhoneNumber
                        == dto.PhoneNumber
                    )
                    &&
                    !u.IsDeleted);


            if (existingUser != null)
                throw new BusinessException(
                    "Username or phone number already exists.");


            // Default Register Role = Employee
            const int employeeRoleId = 3;


            var role =
                await _roleRepository.FirstOrDefaultAsync(
                    r => r.RoleId == employeeRoleId
                         &&
                         !r.IsDeleted);


            if (role == null)
                throw new ResourceNotFoundException(
                    "Role",
                    employeeRoleId);


            var user =
                new User
                {
                    UserId =
                        UserIdGenerator.Generate(
                            dto.DepartmentId),


                    Username =
                        dto.Username.Trim(),


                    PasswordHash =
                        PasswordHasher.HashPassword(
                            dto.Password),


                    RoleId =
                        employeeRoleId,


                    DepartmentId =
                        dto.DepartmentId,


                    PhoneNumber =
                        dto.PhoneNumber,


                    StartDate =
                        DateTime.UtcNow,


                    Salary =
                        0,


                    IsActive =
                        true,


                    CreatedBy =
                        "System"
                };


            await _userRepository.AddAsync(
                user);


            await _userRepository.SaveChangesAsync();


            return await GenerateAuthResponse(
                user,
                role.RoleName);
        }
        catch (BusinessException)
        {
            throw;
        }
        catch (ResourceNotFoundException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new BusinessException(
                "Failed to register user.",
                ex);
        }
    }



    public async Task<AuthResponseDto> LoginAsync(
        LoginDto dto)
    {
        try
        {
            var user =
                await _userRepository.FirstOrDefaultAsync(
                    u => u.Username.ToLower()
                         == dto.Username.ToLower()
                         &&
                         !u.IsDeleted);


            if (user == null)
                throw new BusinessException(
                    "Invalid username or password.");


            var validPassword =
                PasswordHasher.VerifyPassword(
                    dto.Password,
                    user.PasswordHash);


            if (!validPassword)
                throw new BusinessException(
                    "Invalid username or password.");


            if (!user.IsActive)
                throw new BusinessException(
                    "User account is disabled.");


            var role =
                await _roleRepository.FirstOrDefaultAsync(
                    r => r.RoleId == user.RoleId &&
                         !r.IsDeleted);


            if (role == null)
                throw new ResourceNotFoundException(
                    "Role",
                    user.RoleId);


            return await GenerateAuthResponse(
                user,
                role.RoleName);
        }
        catch (BusinessException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new BusinessException(
                "Failed to login.", ex);
        }
    }



    public async Task<AuthResponseDto> RefreshTokenAsync(
    string refreshToken)
    {
        try
        {
            var token =
                await _refreshTokenRepository.FirstOrDefaultAsync(
                    t => t.Token == refreshToken &&
                         !t.IsDeleted);


            if (token == null)
                throw new BusinessException(
                    "Invalid refresh token.");


            if (token.IsRevoked)
                throw new BusinessException(
                    "Refresh token already revoked.");


            if (token.ExpiresAt < DateTime.UtcNow)
                throw new BusinessException(
                    "Refresh token expired.");


            var user =
                await _userRepository.FirstOrDefaultAsync(
                    u => u.UserId == token.UserId &&
                         !u.IsDeleted);


            if (user == null)
                throw new ResourceNotFoundException(
                    "User",
                    token.UserId);


            var role =
                await _roleRepository.FirstOrDefaultAsync(
                    r => r.RoleId == user.RoleId &&
                         !r.IsDeleted);


            if (role == null)
                throw new ResourceNotFoundException(
                    "Role",
                    user.RoleId);


            // Revoke old refresh token
            token.IsRevoked = true;

            token.UpdatedBy =
                "System";

            token.UpdatedDate =
                DateTime.UtcNow;


            _refreshTokenRepository.Update(
                token);


            await _refreshTokenRepository.SaveChangesAsync();


            // Generate new access + refresh token
            return await GenerateAuthResponse(
                user,
                role.RoleName);
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
                "Failed to refresh token.",
                ex);
        }
    }



    public async Task<bool> LogoutAsync(
    string refreshToken,
    string userId)
    {
        try
        {
            var token =
                await _refreshTokenRepository.FirstOrDefaultAsync(
                    t => t.Token == refreshToken &&
                         !t.IsDeleted);


            if (token == null)
                throw new BusinessException(
                    "Invalid refresh token.");


            // Check token owner
            if (token.UserId != userId)
                throw new BusinessException(
                    "You cannot logout another user.");


            if (token.IsRevoked)
                throw new BusinessException(
                    "Refresh token already revoked.");


            if (token.ExpiresAt < DateTime.UtcNow)
                throw new BusinessException(
                    "Refresh token expired.");


            token.IsRevoked =
                true;


            token.UpdatedBy =
                userId;


            token.UpdatedDate =
                DateTime.UtcNow;


            _refreshTokenRepository.Update(
                token);


            await _refreshTokenRepository.SaveChangesAsync();


            return true;
        }
        catch (BusinessException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new BusinessException(
                "Failed to logout.",
                ex);
        }
    }



    private async Task<AuthResponseDto> GenerateAuthResponse(
        User user,
        string roleName)
    {
        var accessToken =
            _jwtService.GenerateAccessToken(
                user,
                roleName);


        var refreshToken =
            new RefreshToken
            {
                Token =
                    _jwtService.GenerateRefreshToken(),

                UserId =
                    user.UserId,

                ExpiresAt =
                    _jwtService.GetRefreshTokenExpiration(),

                CreatedBy =
                    "System"
            };


        await _refreshTokenRepository.AddAsync(
            refreshToken);


        await _refreshTokenRepository.SaveChangesAsync();


        return new AuthResponseDto
        {
            UserId =
                user.UserId,

            Username =
                user.Username,

            AccessToken =
                accessToken,

            RefreshToken =
                refreshToken.Token,

            AccessTokenExpiresAt =
                _jwtService.GetAccessTokenExpiration(),

            RefreshTokenExpiresAt =
                refreshToken.ExpiresAt
        };
    }
}