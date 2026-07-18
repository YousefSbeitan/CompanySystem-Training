using CompanySystem.Business.DTOs;
using CompanySystem.Business.Interfaces;
using CompanySystem.Shared.Exceptions;
using CompanySystem.Shared.Requests;
using CompanySystem.Web.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CompanySystem.Web.Controllers;

public class MainPageSectionController : Controller
{
    private readonly IMainPageSectionService _mainPageSectionService;

    public MainPageSectionController(
        IMainPageSectionService mainPageSectionService)
    {
        _mainPageSectionService = mainPageSectionService;
    }


    // GET: /MainPageSection
    [HttpGet]
    [RequirePermission("MainPageSections.View")]
    public async Task<IActionResult> Index(
        [FromQuery] PaginationFilterRequest request)
    {
        try
        {
            var sections =
                await _mainPageSectionService.GetAllAsync(
                    request);


            return Ok(sections);
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


    // GET: /MainPageSection/Details/1
    [HttpGet]
    [RequirePermission("MainPageSections.View")]
    public async Task<IActionResult> Details(
        int id)
    {
        try
        {
            var section =
                await _mainPageSectionService.GetByIdAsync(
                    id);


            return Ok(section);
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


    // POST: /MainPageSection/Create
    [HttpPost]
    [RequirePermission("MainPageSections.Create")]
    public async Task<IActionResult> Create(
        [FromBody] CreateMainPageSectionDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);


        try
        {
            var section =
                await _mainPageSectionService.CreateAsync(
                    dto);


            return Ok(section);
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


    // PUT: /MainPageSection/Edit
    [HttpPut]
    [RequirePermission("MainPageSections.Edit")]
    public async Task<IActionResult> Edit(
        [FromBody] EditMainPageSectionDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);


        try
        {
            var section =
                await _mainPageSectionService.UpdateAsync(
                    dto);


            return Ok(section);
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


    // DELETE: /MainPageSection/Delete/1
    [HttpDelete]
    [RequirePermission("MainPageSections.Delete")]
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