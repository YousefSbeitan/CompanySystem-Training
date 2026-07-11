using CompanySystem.Business.DTOs;
using CompanySystem.Business.Interfaces;
using CompanySystem.Shared.Exceptions;
using CompanySystem.Shared.Requests;
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
    [AllowAnonymous]
    public IActionResult Index()
    {
        return View();
    }


    // MVC: GET /MainPageSection/Create
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public IActionResult Create()
    {
        return View();
    }


    // MVC: GET /MainPageSection/Edit/{id}
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public IActionResult Edit(
        int id)
    {
        return View();
    }


    // MVC: GET /MainPageSection/Details/{id}
    [HttpGet]
    [AllowAnonymous]
    public IActionResult Details(
        int id)
    {
        return View();
    }


    // MVC: GET /MainPageSection/Delete/{id}
    [HttpGet]
    [Authorize(Roles = "Admin")]
    [ActionName("Delete")]
    public IActionResult DeleteView(
        int id)
    {
        return View();
    }


    // API: GET /MainPageSection/GetAll
    [HttpGet]
    [AllowAnonymous]
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


    // API: GET /MainPageSection/GetById/1
    [HttpGet]
    [AllowAnonymous]
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


    // POST: /MainPageSection/Create
    [HttpPost]
    [Authorize(Roles = "Admin")]
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


    // PUT: /MainPageSection/Edit
    [HttpPut]
    [Authorize(Roles = "Admin")]
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


    // DELETE: /MainPageSection/Delete/1
    [HttpDelete]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(
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