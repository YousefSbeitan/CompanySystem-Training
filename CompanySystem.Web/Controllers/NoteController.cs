using CompanySystem.Business.DTOs;
using CompanySystem.Business.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CompanySystem.Web.Controllers;

public class NoteController : Controller
{
    private readonly INoteService _noteService;

    public NoteController(INoteService noteService)
    {
        _noteService = noteService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var notes = await _noteService.GetAllAsync();

        return Json(notes);
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var note = await _noteService.GetByIdAsync(id);

        if (note == null)
            return NotFound();

        return Json(note);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateNoteDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var note = await _noteService.CreateAsync(dto);

        return Ok(note);
    }

    [HttpPut]
    public async Task<IActionResult> Edit([FromBody] EditNoteDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var note = await _noteService.UpdateAsync(dto);

        if (note == null)
            return NotFound();

        return Ok(note);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _noteService.DeleteAsync(id);

        if (!deleted)
            return NotFound();

        return Ok(new
        {
            Message = "Note deleted successfully."
        });
    }
}