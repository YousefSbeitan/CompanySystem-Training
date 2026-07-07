using CompanySystem.Business.DTOs;
using CompanySystem.Business.Interfaces;
using CompanySystem.Shared.Exceptions;
using CompanySystem.Shared.Requests;
using Microsoft.AspNetCore.Mvc;

namespace CompanySystem.Web.Controllers;

public class UserController : Controller
{
    private readonly IUserService _userService;

    public UserController(
        IUserService userService)
    {
        _userService = userService;
    }


    // GET: /User
    [HttpGet]
    public async Task<IActionResult> Index(
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
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex.Message);
        }
    }


    // GET: /User/Details/EMP001
    [HttpGet]
    public async Task<IActionResult> Details(
        string id)
    {
        try
        {
            var user =
                await _userService.GetByIdAsync(
                    id);


            return Ok(user);
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


    // POST: /User/Create
    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateUserDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);


        try
        {
            var user =
                await _userService.CreateAsync(
                    dto);


            return Ok(user);
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


    // PUT: /User/Edit
    [HttpPut]
    public async Task<IActionResult> Edit(
        [FromBody] EditUserDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);


        try
        {
            var user =
                await _userService.UpdateAsync(
                    dto);


            return Ok(user);
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


    // DELETE: /User/Delete/EMP001
    [HttpDelete]
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