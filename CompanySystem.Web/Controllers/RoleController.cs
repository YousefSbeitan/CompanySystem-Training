using CompanySystem.Business.DTOs;
using CompanySystem.Business.Interfaces;
using CompanySystem.Shared.Exceptions;
using CompanySystem.Shared.Requests;
using CompanySystem.Web.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CompanySystem.Web.Controllers;

public class RoleController : Controller
{
    private readonly IRoleService _roleService;

    public RoleController(IRoleService roleService)
    {
        _roleService = roleService;
    }


    // GET: /Role
    [HttpGet]
    [RequirePermission("Roles.View")]
    public async Task<IActionResult> Index(
        [FromQuery] PaginationFilterRequest request)
    {
        try
        {
            var roles =
                await _roleService.GetAllAsync(request);


            return Ok(roles);
        }
        catch (BusinessException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }


    // GET: /Role/Details/1
    [HttpGet]
    [RequirePermission("Roles.View")]
    public async Task<IActionResult> Details(
        int id)
    {
        try
        {
            var role =
                await _roleService.GetByIdAsync(id);


            return Ok(role);
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
            return StatusCode(500, ex.Message);
        }
    }


    // POST: /Role/Create
    [HttpPost]
    [RequirePermission("Roles.Create")]
    public async Task<IActionResult> Create(
        [FromBody] CreateRoleDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);


        try
        {
            var role =
                await _roleService.CreateAsync(dto);


            return Ok(role);
        }
        catch (BusinessException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }


    // PUT: /Role/Edit
    [HttpPut]
    [RequirePermission("Roles.Edit")]
    public async Task<IActionResult> Edit(
        [FromBody] EditRoleDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);


        try
        {
            var role =
                await _roleService.UpdateAsync(dto);


            return Ok(role);
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
            return StatusCode(500, ex.Message);
        }
    }


    // DELETE: /Role/Delete/1
    [HttpDelete]
    [RequirePermission("Roles.Delete")]
    public async Task<IActionResult> Delete(
        int id)
    {
        try
        {
            await _roleService.DeleteAsync(id);


            return Ok(new
            {
                Message =
                    "Role deleted successfully."
            });
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
            return StatusCode(500, ex.Message);
        }
    }
}