using CompanySystem.Business.DTOs;
using CompanySystem.Business.Interfaces;
using CompanySystem.Shared.Exceptions;
using CompanySystem.Shared.Requests;
using CompanySystem.Web.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CompanySystem.Web.Controllers;


[Authorize]
public class RoleController : Controller
{
    private readonly IRoleService _roleService;


    public RoleController(
        IRoleService roleService)
    {
        _roleService = roleService;
    }


    // GET: /Role (MVC View)
    [HttpGet]
    [RequirePermission("Roles.View")]
    public IActionResult Index()
    {
        return View();
    }


    // API: GET /Role/GetAll
    [HttpGet]
    [RequirePermission("Roles.View")]
    public async Task<IActionResult> GetAll(
        [FromQuery] PaginationFilterRequest request)
    {
        try
        {
            var roles =
                await _roleService.GetAllAsync(
                    request);


            return Ok(
                roles);
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


    // MVC: GET /Role/Details/{id}
    [HttpGet]
    [RequirePermission("Roles.View")]
    public IActionResult Details(int id)
    {
        return View();
    }


    // API: GET /Role/GetById/{id}
    [HttpGet]
    [RequirePermission("Roles.View")]
    public async Task<IActionResult> GetById(
        int id)
    {
        try
        {
            var role =
                await _roleService.GetByIdAsync(
                    id);


            return Ok(
                role);
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


    // MVC: GET /Role/Create
    [HttpGet]
    [RequirePermission("Roles.Create")]
    public IActionResult Create()
    {
        return View();
    }


    // POST: /Role/Create
    [HttpPost]
    [RequirePermission("Roles.Create")]
    public async Task<IActionResult> Create(
        [FromBody] CreateRoleDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(
                ModelState);


        try
        {
            var role =
                await _roleService.CreateAsync(
                    dto);


            return Ok(
                role);
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


    // MVC: GET /Role/Edit/{id}
    [HttpGet]
    [RequirePermission("Roles.Edit")]
    public IActionResult Edit(int id)
    {
        return View();
    }


    // PUT: /Role/Edit
    [HttpPut]
    [RequirePermission("Roles.Edit")]
    public async Task<IActionResult> Edit(
        [FromBody] EditRoleDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(
                ModelState);


        try
        {
            var role =
                await _roleService.UpdateAsync(
                    dto);


            return Ok(
                role);
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


    // MVC: GET /Role/Delete/{id}
    [HttpGet]
    [RequirePermission("Roles.Delete")]
    [ActionName("Delete")]
    public IActionResult DeleteView(int id)
    {
        return View("Delete");
    }


    // DELETE: /Role/Delete/{id}
    [HttpDelete]
    [RequirePermission("Roles.Delete")]
    public async Task<IActionResult> Delete(
        int id)
    {
        try
        {
            await _roleService.DeleteAsync(
                id);


            return Ok(new
            {
                Message =
                    "Role deleted successfully."
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
