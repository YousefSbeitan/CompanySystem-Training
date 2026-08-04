using CompanySystem.Business.DTOs;
using CompanySystem.Business.Interfaces;
using CompanySystem.Shared.Exceptions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CompanySystem.Web.Controllers;


[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IUserService _userService;


    public AuthController(
        IAuthService authService,
        IUserService userService)
    {
        _authService = authService;
        _userService = userService;
    }


    // POST: api/Auth/Register
    [HttpPost("Register")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Register(
        [FromBody] RegisterDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(
                ModelState);


        try
        {
            var createUserDto = new CreateUserDto
            {
                Username = dto.Username,
                Password = dto.Password,
                DepartmentId = dto.DepartmentId,
                PhoneNumber = dto.PhoneNumber,
                RoleId = 3, // Default to Employee
                IsLeader = false,
                StartDate = DateTime.UtcNow,
                Salary = 0,
                IsActive = true
            };


            var user =
                await _userService.CreateAsync(
                    createUserDto);


            return Ok(new
            {
                Message =
                    "User registered successfully.",
                UserId = user.UserId
            });
        }
        catch (BusinessException ex)
        {
            return BadRequest(
                ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(
                500,
                ex.Message);
        }
    }


    // POST: api/Auth/Login
    [HttpPost("Login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login(
        [FromBody] LoginDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(
                ModelState);


        try
        {
            var result =
                await _authService.LoginAsync(
                    dto);


            var claims =
                new List<Claim>
                {
                    new Claim(
                        ClaimTypes.NameIdentifier,
                        result.UserId),

                    new Claim(
                        ClaimTypes.Name,
                        result.Username),

                    new Claim(
                        ClaimTypes.Role,
                        result.Role)
                };

            foreach (var permission
                in result.Permissions)
            {
                claims.Add(
                    new Claim(
                        "Permission",
                        permission));
            }

            var claimsIdentity =
                new ClaimsIdentity(
                    claims,
                    CookieAuthenticationDefaults
                        .AuthenticationScheme);

            var claimsPrincipal =
                new ClaimsPrincipal(
                    claimsIdentity);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults
                    .AuthenticationScheme,
                claimsPrincipal);


            return Ok(
                result);
        }
        catch (BusinessException ex)
        {
            return BadRequest(
                ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(
                500,
                ex.Message);
        }
    }


    // POST: api/Auth/RefreshToken
    [HttpPost("RefreshToken")]
    [AllowAnonymous]
    public async Task<IActionResult> RefreshToken(
        [FromBody] RefreshTokenRequestDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(
                ModelState);


        try
        {
            var result =
                await _authService.RefreshTokenAsync(
                    dto.RefreshToken);


            var claims =
                new List<Claim>
                {
                    new Claim(
                        ClaimTypes.NameIdentifier,
                        result.UserId),

                    new Claim(
                        ClaimTypes.Name,
                        result.Username),

                    new Claim(
                        ClaimTypes.Role,
                        result.Role)
                };

            foreach (var permission
                in result.Permissions)
            {
                claims.Add(
                    new Claim(
                        "Permission",
                        permission));
            }

            var claimsIdentity =
                new ClaimsIdentity(
                    claims,
                    CookieAuthenticationDefaults
                        .AuthenticationScheme);

            var claimsPrincipal =
                new ClaimsPrincipal(
                    claimsIdentity);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults
                    .AuthenticationScheme,
                claimsPrincipal);


            return Ok(
                result);
        }
        catch (BusinessException ex)
        {
            return BadRequest(
                ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(
                500,
                ex.Message);
        }
    }


    // POST: api/Auth/Logout
    // AllowAnonymous so expired JWT still clears the HttpOnly auth cookie.
    [HttpPost("Logout")]
    [AllowAnonymous]
    public async Task<IActionResult> Logout(
        [FromBody] LogoutRequestDto? dto)
    {
        var currentUserId =
            User.FindFirst(
                ClaimTypes.NameIdentifier)
            ?.Value;


        if (!string.IsNullOrWhiteSpace(dto?.RefreshToken) &&
            !string.IsNullOrWhiteSpace(currentUserId))
        {
            try
            {
                await _authService.LogoutAsync(
                    dto.RefreshToken,
                    currentUserId);
            }
            catch (BusinessException)
            {
                // Best-effort revoke; always sign out cookie below
            }
            catch
            {
                // Best-effort revoke; always sign out cookie below
            }
        }


        await HttpContext.SignOutAsync(
            CookieAuthenticationDefaults
                .AuthenticationScheme);


        return Ok(new
        {
            Message =
                "Logged out successfully."
        });
    }
}