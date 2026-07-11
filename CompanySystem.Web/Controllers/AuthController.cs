using CompanySystem.Business.DTOs;
using CompanySystem.Business.Interfaces;
using CompanySystem.Shared.Exceptions;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CompanySystem.Web.Controllers;


[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;


    public AuthController(
        IAuthService authService)
    {
        _authService = authService;
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

            // Extract role from JWT token
            var role = "User";
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(result.AccessToken);
                role = jwtToken.Claims
                    .FirstOrDefault(c => c.Type == ClaimTypes.Role)
                    ?.Value ?? "User";
            }
            catch
            {
                // Fallback to User role
            }

            // Create claims identity for cookie authentication
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, result.UserId),
                new Claim(ClaimTypes.Name, result.Username),
                new Claim(ClaimTypes.Role, role)
            };

            var claimsIdentity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme);

            var authProperties = new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = result.AccessTokenExpiresAt.ToUniversalTime(),
                IssuedUtc = DateTime.UtcNow
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);

            // Also set JWT in a cookie for MVC requests that might need it
            Response.Cookies.Append("CompanySystem.Jwt", result.AccessToken, new CookieOptions
            {
                HttpOnly = true,
                SameSite = SameSiteMode.Lax,
                Expires = result.AccessTokenExpiresAt.ToUniversalTime(),
                Secure = HttpContext.Request.IsHttps
            });

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
    [HttpPost("Logout")]
    [Authorize]
    public async Task<IActionResult> Logout(
        [FromBody] RefreshTokenRequestDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(
                ModelState);


        try
        {
            var currentUserId =
                User.FindFirst(
                    ClaimTypes.NameIdentifier)
                ?.Value;


            if (string.IsNullOrEmpty(
                    currentUserId))
            {
                return Unauthorized(
                    "Invalid access token.");
            }


            await _authService.LogoutAsync(
                dto.RefreshToken,
                currentUserId);

            // Sign out cookie authentication
            await HttpContext.SignOutAsync(
                CookieAuthenticationDefaults.AuthenticationScheme);

            // Remove JWT cookie
            Response.Cookies.Delete("CompanySystem.Jwt");

            return Ok(new
            {
                Message =
                    "Logged out successfully."
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
}