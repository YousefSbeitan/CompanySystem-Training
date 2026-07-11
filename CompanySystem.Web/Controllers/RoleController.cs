using CompanySystem.Business.DTOs;
using CompanySystem.Business.Interfaces;
using CompanySystem.Shared.Exceptions;
using CompanySystem.Shared.Requests;
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
    [Authorize(Roles = "Admin")]
    public IActionResult Index()
    {
        return View();
    }


    // GET: /Role/Create (MVC View)
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public IActionResult Create()
    {
        return View();
    }


    // GET: /Role/Edit/{id} (MVC View)
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public IActionResult Edit(
        int id)
    {
        return View();
    }


    // GET: /Role/Details/{id} (MVC View)
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public IActionResult Details(
        int id)
    {
        return View();
    }


    // GET: /Role/Delete/{id} (MVC View)
    [HttpGet]
    [Authorize(Roles = "Admin")]
    [ActionName("Delete")]
    public IActionResult DeleteView(
        int id)
    {
        return View();
    }


    // API: GET /Role/GetAll
    [HttpGet]
    [Authorize(Roles = "Admin")]
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


    // API: GET /Role/GetById/1
    [HttpGet]
    [Authorize(Roles = "Admin")]
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


    // POST: /Role/Create
    [HttpPost]
    [Authorize(Roles = "Admin")]
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


    // PUT: /Role/Edit
    [HttpPut]
    [Authorize(Roles = "Admin")]
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


    // DELETE: /Role/Delete/1
    [HttpDelete]
    [Authorize(Roles = "Admin")]
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