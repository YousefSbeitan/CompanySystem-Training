using CompanySystem.Business.DTOs;
using CompanySystem.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CompanySystem.Web.Controllers;

public class DepartmentController : Controller
{
    private readonly IDepartmentService _departmentService;

    public DepartmentController(IDepartmentService departmentService)
    {
        _departmentService = departmentService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var departments = await _departmentService.GetAllAsync();

        return Json(departments);
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var department = await _departmentService.GetByIdAsync(id);

        if (department == null)
            return NotFound();

        return Json(department);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateDepartmentDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var department = await _departmentService.CreateAsync(dto);

        return Ok(department);
    }

    [HttpPut]
    public async Task<IActionResult> Edit([FromBody] EditDepartmentDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var department = await _departmentService.UpdateAsync(dto);

        if (department == null)
            return NotFound();

        return Ok(department);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _departmentService.DeleteAsync(id);

        if (!deleted)
            return NotFound();

        return Ok(new
        {
            Message = "Department deleted successfully."
        });
    }
}