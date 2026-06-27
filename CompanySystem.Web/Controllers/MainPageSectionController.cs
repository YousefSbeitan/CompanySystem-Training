using CompanySystem.Business.DTOs;
using CompanySystem.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CompanySystem.Web.Controllers;

public class MainPageSectionController : Controller
{
    private readonly IMainPageSectionService _mainPageSectionService;

    public MainPageSectionController(IMainPageSectionService mainPageSectionService)
    {
        _mainPageSectionService = mainPageSectionService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var sections = await _mainPageSectionService.GetAllAsync();

        return Json(sections);
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var section = await _mainPageSectionService.GetByIdAsync(id);

        if (section == null)
            return NotFound();

        return Json(section);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateMainPageSectionDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var section = await _mainPageSectionService.CreateAsync(dto);

        return Ok(section);
    }

    [HttpPut]
    public async Task<IActionResult> Edit([FromBody] EditMainPageSectionDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var section = await _mainPageSectionService.UpdateAsync(dto);

        if (section == null)
            return NotFound();

        return Ok(section);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _mainPageSectionService.DeleteAsync(id);

        if (!deleted)
            return NotFound();

        return Ok(new
        {
            Message = "Main page section deleted successfully."
        });
    }
}