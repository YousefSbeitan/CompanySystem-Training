using CompanySystem.Business.DTOs;
using CompanySystem.Business.Interfaces;
using CompanySystem.Shared.Exceptions;
using CompanySystem.Shared.Requests;
using CompanySystem.Web.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CompanySystem.Web.Controllers;


[ApiController]
[Authorize]
[Route("api/[controller]")]
public class PermissionController : ControllerBase
{
    private readonly IPermissionService _permissionService;


    public PermissionController(
        IPermissionService permissionService)
    {
        _permissionService = permissionService;
    }


    // GET: api/Permission
    [HttpGet]
    [RequirePermission("Permissions.View")]
    public async Task<IActionResult> GetAll(
        [FromQuery] PaginationFilterRequest request)
    {
        try
        {
            var permissions =
                await _permissionService.GetAllAsync(
                    request);


            return Ok(
                permissions);
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


    // GET: api/Permission/User/{userId}
    [HttpGet("User/{userId}")]
    [RequirePermission("Permissions.View")]
    public async Task<IActionResult> GetUserPermissions(
        string userId)
    {
        try
        {
            var permissions =
                await _permissionService.GetUserPermissionsAsync(
                    userId);


            return Ok(
                permissions);
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


    // PUT: api/Permission/Assign
    [HttpPut("Assign")]
    [RequirePermission("Permissions.Edit")]
    public async Task<IActionResult> Assign(
        [FromBody] AssignUserPermissionsDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(
                ModelState);


        try
        {
            await _permissionService.AssignToUserAsync(
                dto);


            return Ok(new
            {
                Message =
                    "Permissions assigned successfully."
            });
        }
        catch (ResourceNotFoundException ex)
        {
            return NotFound(
                ex.Message);
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
