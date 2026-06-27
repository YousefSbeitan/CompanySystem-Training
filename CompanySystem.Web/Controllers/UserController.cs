using CompanySystem.Business.DTOs;
using CompanySystem.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CompanySystem.Web.Controllers;

public class UserController : Controller
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var users = await _userService.GetAllAsync();

        return Json(users);
    }

    [HttpGet]
    public async Task<IActionResult> Details(string id)
    {
        var user = await _userService.GetByIdAsync(id);

        if (user == null)
            return NotFound();

        return Json(user);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateUserDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var user = await _userService.CreateAsync(dto);

        return Ok(user);
    }

    [HttpPut]
    public async Task<IActionResult> Edit([FromBody] EditUserDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var user = await _userService.UpdateAsync(dto);

        if (user == null)
            return NotFound();

        return Ok(user);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(string id)
    {
        var deleted = await _userService.DeleteAsync(id);

        if (!deleted)
            return NotFound();

        return Ok(new
        {
            Message = "User deleted successfully."
        });
    }
}