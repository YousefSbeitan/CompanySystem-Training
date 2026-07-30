using CompanySystem.Business.DTOs;
using CompanySystem.Business.Interfaces;
using CompanySystem.Shared.Exceptions;
using CompanySystem.Shared.Requests;
using CompanySystem.Web.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CompanySystem.Web.Controllers;


[Authorize]
public class PermissionController : Controller
{
    private readonly IPermissionService _permissionService;


    public PermissionController(
        IPermissionService permissionService)
    {
        _permissionService = permissionService;
    }


    // MVC: GET /Permission (MVC View)
    [HttpGet]
    [RequirePermission("Permissions.View")]
    public IActionResult Index()
    {
        return View();
    }


    // MVC: GET /Permission/Create (MVC View)
    [HttpGet]
    [RequirePermission("Permissions.Create")]
    public IActionResult Create()
    {
        return View();
    }


    // MVC: GET /Permission/Edit/{id} (MVC View)
    [HttpGet]
    [RequirePermission("Permissions.Edit")]
    public IActionResult Edit(int id)
    {
        return View();
    }


    // MVC: GET /Permission/Details/{id} (MVC View)
    [HttpGet]
    [RequirePermission("Permissions.View")]
    public IActionResult Details(int id)
    {
        return View();
    }


    // MVC: GET /Permission/Delete/{id} (MVC View)
    [HttpGet]
    [ActionName("Delete")]
    [RequirePermission("Permissions.Delete")]
    public IActionResult DeleteView(int id)
    {
        return View("Delete");
    }


    // API: GET /Permission/GetAll
    [HttpGet("GetAll")]
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


    // API: GET /Permission/GetById/{id}
    [HttpGet("GetById/{id}")]
    [RequirePermission("Permissions.View")]
    public async Task<IActionResult> GetById(
        int id)
    {
        try
        {
            var allPermissions =
                await _permissionService.GetAllAsync(
                    new PaginationFilterRequest
                    {
                        PageNumber = 1,
                        PageSize = int.MaxValue
                    });


            var permission = allPermissions.Data?
                .FirstOrDefault(
                    p => p.PermissionId == id);


            if (permission == null)
                return NotFound(
                    "Permission not found.");


            return Ok(
                permission);
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


    // API: GET /Permission/GetUserPermissions/{userId}
    [HttpGet("GetUserPermissions/{userId}")]
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


    // API: PUT /Permission/Assign
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
