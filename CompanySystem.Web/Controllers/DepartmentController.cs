using CompanySystem.Business.DTOs;
using CompanySystem.Business.Interfaces;
using CompanySystem.Shared.Exceptions;
using CompanySystem.Shared.Requests;
using CompanySystem.Web.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CompanySystem.Web.Controllers;


[Authorize]
public class DepartmentController : Controller
{
    private readonly IDepartmentService _departmentService;


    public DepartmentController(
        IDepartmentService departmentService)
    {
        _departmentService = departmentService;
    }



    // GET: /Department (MVC View)
    [HttpGet]
    [RequirePermission("Departments.View")]
    public IActionResult Index()
    {
        return View();
    }


    // GET: /Department/Create (MVC View)
    [HttpGet]
    [RequirePermission("Departments.Create")]
    public IActionResult Create()
    {
        return View();
    }


    // GET: /Department/Edit/{id} (MVC View)
    [HttpGet]
    [RequirePermission("Departments.Edit")]
    public IActionResult Edit(
        int id)
    {
        return View();
    }


    // GET: /Department/Details/{id} (MVC View)
    [HttpGet]
    [RequirePermission("Departments.View")]
    public IActionResult Details(
        int id)
    {
        return View();
    }


    // GET: /Department/Delete/{id} (MVC View)
    [HttpGet]
    [RequirePermission("Departments.Delete")]
    [ActionName("Delete")]
    public IActionResult DeleteView(
        int id)
    {
        return View();
    }


    // API: GET /Department/GetAll
    [HttpGet]
    [RequirePermission("Departments.View")]
    public async Task<IActionResult> GetAll(
        [FromQuery] PaginationFilterRequest request)
    {
        try
        {
            var departments =
                await _departmentService.GetAllAsync(
                    request);


            return Ok(
                departments);
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


    // API: GET /Department/GetById/1
    [HttpGet]
    [RequirePermission("Departments.View")]
    public async Task<IActionResult> GetById(
        int id)
    {
        try
        {
            var department =
                await _departmentService.GetByIdAsync(
                    id);


            return Ok(
                department);
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


    // API: POST /Department/Create
    [HttpPost]
    [RequirePermission("Departments.Create")]
    public async Task<IActionResult> Create(
        [FromBody] CreateDepartmentDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(
                ModelState);


        try
        {
            var department =
                await _departmentService.CreateAsync(
                    dto);


            return Ok(
                department);
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


    // API: PUT /Department/Edit
    [HttpPut]
    [RequirePermission("Departments.Edit")]
    public async Task<IActionResult> Edit(
        [FromBody] EditDepartmentDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(
                ModelState);


        try
        {
            var department =
                await _departmentService.UpdateAsync(
                    dto);


            return Ok(
                department);
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


    // API: DELETE /Department/Delete/1
    [HttpDelete]
    [RequirePermission("Departments.Delete")]
    public async Task<IActionResult> Delete(
        int id)
    {
        try
        {
            await _departmentService.DeleteAsync(
                id);


            return Ok(new
            {
                Message =
                    "Department deleted successfully."
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