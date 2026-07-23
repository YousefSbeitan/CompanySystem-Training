using CompanySystem.Business.DTOs;
using CompanySystem.Business.Interfaces;
using CompanySystem.Shared.Exceptions;
using CompanySystem.Shared.Requests;
using CompanySystem.Web.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CompanySystem.Web.Controllers;

[Authorize]
public class MainPageSectionController : Controller
{
    private readonly IMainPageSectionService _mainPageSectionService;


    public MainPageSectionController(
        IMainPageSectionService mainPageSectionService)
    {
        _mainPageSectionService =
            mainPageSectionService;
    }


    // MVC: GET /MainPageSection
    [HttpGet]
    [RequirePermission("MainPageSections.View")]
    public IActionResult Index()
    {
        return View();
    }


    // API: GET /MainPageSection/GetAll
    [HttpGet]
    [RequirePermission("MainPageSections.View")]
    public async Task<IActionResult> GetAll(
        [FromQuery] PaginationFilterRequest request)
    {
        try
        {
            var sections =
                await _mainPageSectionService.GetAllAsync(
                    request);


            return Ok(
                sections);
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


    // MVC: GET /MainPageSection/Details/{id}
    [HttpGet]
    [RequirePermission("MainPageSections.View")]
    public IActionResult Details(
        int id)
    {
        return View();
    }


    // API: GET /MainPageSection/GetById/{id}
    [HttpGet]
    [RequirePermission("MainPageSections.View")]
    public async Task<IActionResult> GetById(
        int id)
    {
        try
        {
            var section =
                await _mainPageSectionService.GetByIdAsync(
                    id);


            return Ok(
                section);
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


    // GET: /MainPageSection/Create
    [HttpGet]
    [RequirePermission("MainPageSections.Create")]
    public IActionResult Create()
    {
        return View();
    }


    // POST: /MainPageSection/Create
    [HttpPost]
    [RequirePermission("MainPageSections.Create")]
    public async Task<IActionResult> Create(
        [FromBody] CreateMainPageSectionDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(
                ModelState);


        try
        {
            var section =
                await _mainPageSectionService.CreateAsync(
                    dto);


            return Ok(
                section);
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


    // GET: /MainPageSection/Edit/{id}
    [HttpGet]
    [RequirePermission("MainPageSections.Edit")]
    public IActionResult Edit(
        int id)
    {
        return View();
    }


    // PUT: /MainPageSection/Edit
    [HttpPut]
    [RequirePermission("MainPageSections.Edit")]
    public async Task<IActionResult> Edit(
        [FromBody] EditMainPageSectionDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(
                ModelState);


        try
        {
            var section =
                await _mainPageSectionService.UpdateAsync(
                    dto);


            return Ok(
                section);
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


    // GET: /MainPageSection/Delete/{id}
    [HttpGet]
    [RequirePermission("MainPageSections.Delete")]
    public IActionResult Delete(
        int id)
    {
        return View();
    }


    // DELETE: /MainPageSection/Delete/1
    [HttpDelete]
    [ActionName("Delete")]
    [RequirePermission("MainPageSections.Delete")]
    public async Task<IActionResult> DeleteConfirmed(
        int id)
    {
        try
        {
            await _mainPageSectionService.DeleteAsync(
                id);


            return Ok(new
            {
                Message =
                    "Main page section deleted successfully."
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