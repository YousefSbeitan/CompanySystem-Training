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
    [Authorize(Roles = "Admin")]
    public IActionResult Create()
    {
        return View();
    }


    // GET: /User/Edit/{id} (MVC View)
    [HttpGet]
    public IActionResult Edit(
        string id)
    {
        if (!CanAccessUser(id))
            return Forbid();


        return View();
    }


    // GET: /User/Details/{id} (MVC View)
    [HttpGet]
    public IActionResult Details(
        string id)
    {
        if (!CanAccessUser(id))
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
    [HttpGet]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> GetAll(
        [FromQuery] PaginationFilterRequest request)
    {
        try
        {
            var users =
                await _userService.GetAllAsync(
                    request);


            return Ok(users);
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


    // API: GET /User/GetById/EMP001
    [HttpGet]
    public async Task<IActionResult> GetById(
        string id)
    {
        if (!CanAccessUser(id))
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
    [Authorize(Roles = "Admin")]
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
    [HttpPut]
    public async Task<IActionResult> Edit(
        [FromBody] EditUserDto dto)
    {
        if (!CanAccessUser(dto.UserId))
            return Forbid();


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


    // API: DELETE /User/Delete/EMP001
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



    private bool CanAccessUser(
        string userId)
    {
        var currentUserId =
            User.FindFirst(
                ClaimTypes.NameIdentifier)
            ?.Value;


        if (currentUserId == null)
            return false;


        if (User.IsInRole("Admin") ||
            User.IsInRole("Manager"))
        {
            return true;
        }


        return currentUserId == userId;
    }
}