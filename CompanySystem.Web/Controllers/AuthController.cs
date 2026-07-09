using CompanySystem.Business.DTOs;
using CompanySystem.Business.Interfaces;
using CompanySystem.Shared.Exceptions;
using Microsoft.AspNetCore.Mvc;

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


    // POST: api/Auth/Register
    [HttpPost("Register")]
    public async Task<IActionResult> Register(
        [FromBody] RegisterDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);


        try
        {
            var result =
                await _authService.RegisterAsync(
                    dto);


            return Ok(result);
        }
        catch (ResourceNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (BusinessException ex)
        {
            return BadRequest(ex.Message);
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
    public async Task<IActionResult> Login(
        [FromBody] LoginDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);


        try
        {
            var result =
                await _authService.LoginAsync(
                    dto);


            return Ok(result);
        }
        catch (BusinessException ex)
        {
            return BadRequest(ex.Message);
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
    public async Task<IActionResult> RefreshToken(
        [FromBody] RefreshTokenRequestDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);


        try
        {
            var result =
                await _authService.RefreshTokenAsync(
                    dto.RefreshToken);


            return Ok(result);
        }
        catch (BusinessException ex)
        {
            return BadRequest(ex.Message);
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
    public async Task<IActionResult> Logout(
        [FromBody] RefreshTokenRequestDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);


        try
        {
            await _authService.LogoutAsync(
                dto.RefreshToken);


            return Ok(new
            {
                Message =
                    "Logged out successfully."
            });
        }
        catch (BusinessException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(
                500,
                ex.Message);
        }
    }
}