using CompanySystem.Business.DTOs;
using CompanySystem.Business.Interfaces;
using CompanySystem.Shared.Exceptions;
using CompanySystem.Shared.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CompanySystem.Web.Controllers;


[Authorize]
public class UserController : Controller
{
    private readonly IUserService _userService;


    public UserController(
        IUserService userService)
    {
        _userService = userService;
    }


    // GET: /User (MVC View)
    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }


    // GET: /User/Create (MVC View)
    [HttpGet]
    [Authorize(Roles = "Admin,HR")]
    public IActionResult Create()
    {
        return View();
    }



    [HttpGet]
    public async Task<IActionResult> Edit(
        string id)
    {
        if (!await CanAccessUserAsync(id))
            return Forbid();


        return View();
    }

    // GET: /User/Details/{id} (MVC View)
    [HttpGet]
    public async Task<IActionResult> Details(
        string id)
    {
        if (!await CanAccessUserAsync(id))
            return Forbid();


        return View();
    }

    // GET: /User/Delete/{id} (MVC View)
    [HttpGet]
    [Authorize(Roles = "Admin")]
    [ActionName("Delete")]
    public IActionResult DeleteView(
        string id)
    {
        return View();
    }

    // API: GET /User/GetAll
    // Admin + Manager
    [HttpGet]
    [Authorize(Roles = "Admin,Manager,HR")]
    public async Task<IActionResult> GetAll(
        [FromQuery] PaginationFilterRequest request)
    {
        try
        {
            var currentUserId =
                User.FindFirst(
                    ClaimTypes.NameIdentifier)
                ?.Value;


            var currentUserRole =
                User.FindFirst(
                    ClaimTypes.Role)
                ?.Value;


            if (string.IsNullOrEmpty(currentUserId) ||
                string.IsNullOrEmpty(currentUserRole))
            {
                return Unauthorized(
                    "Invalid access token.");
            }


            var users =
                await _userService.GetAllAsync(
                    request,
                    currentUserId,
                    currentUserRole);


            return Ok(
                users);
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


    [HttpGet]
    public async Task<IActionResult> GetById(
        string id)
    {
        if (!await CanAccessUserAsync(id))
            return Forbid();


        try
        {
            var user =
                await _userService.GetByIdAsync(
                    id);


            return Ok(
                user);
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

    // API: POST /User/Create
    [HttpPost]
    [Authorize(Roles = "Admin,HR")]
    public async Task<IActionResult> Create(
        [FromBody] CreateUserDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(
                ModelState);


        try
        {
            var user =
                await _userService.CreateAsync(
                    dto);


            return Ok(
                user);
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


    // API: PUT /User/Edit
    // Admin only because DTO contains Role, Salary, Department
    [HttpPut]
    [Authorize(Roles = "Admin,HR")]
    public async Task<IActionResult> Edit(
        [FromBody] EditUserDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(
                ModelState);

        try
        {
            var user =
                await _userService.UpdateAsync(
                    dto);


            return Ok(
                user);
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

    [HttpDelete]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(
        string id)
    {
        try
        {
            await _userService.DeleteAsync(
                id);


            return Ok(new
            {
                Message =
                    "User deleted successfully."
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



    private async Task<bool> CanAccessUserAsync(
        string userId)
    {
        var currentUserId =
            User.FindFirst(
                ClaimTypes.NameIdentifier)
            ?.Value;


        if (currentUserId == null)
            return false;


        // Admin and HR can access everyone
        if (User.IsInRole("Admin") ||
            User.IsInRole("HR"))
        {
            return true;
        }


        // User can access himself
        if (currentUserId == userId)
            return true;


        // Manager can access his employees only
        if (User.IsInRole("Manager"))
        {
            return await _userService
                .CanManageUserAsync(
                    currentUserId,
                    userId);
        }


        return false;
    }
}