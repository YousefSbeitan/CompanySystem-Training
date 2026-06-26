using CompanySystem.Business.DTOs;
using CompanySystem.Business.Interfaces;
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
    public async Task<IActionResult> Index()
    {
        var roles = await _roleService.GetAllAsync();

        return Json(roles);
    }

    // GET: /Role/Details/1
    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var role = await _roleService.GetByIdAsync(id);

        if (role == null)
            return NotFound();

        return Json(role);
    }

    // POST: /Role/Create
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateRoleDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var role = await _roleService.CreateAsync(dto);

        return Ok(role);
    }

    // PUT: /Role/Edit
    [HttpPut]
    public async Task<IActionResult> Edit([FromBody] EditRoleDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var role = await _roleService.UpdateAsync(dto);

        if (role == null)
            return NotFound();

        return Ok(role);
    }

    // DELETE: /Role/Delete/1
    [HttpDelete]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _roleService.DeleteAsync(id);

        if (!deleted)
            return NotFound();

        return Ok(new
        {
            Message = "Role deleted successfully."
        });
    }
}