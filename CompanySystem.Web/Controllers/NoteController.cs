using CompanySystem.Business.DTOs;
using CompanySystem.Business.Interfaces;
using CompanySystem.Shared.Exceptions;
using CompanySystem.Shared.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CompanySystem.Web.Controllers;


[Authorize]
public class NoteController : Controller
{
    private readonly INoteService _noteService;


    public NoteController(
        INoteService noteService)
    {
        _noteService = noteService;
    }


    // GET: /Note
    [HttpGet]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> Index(
        [FromQuery] PaginationFilterRequest request)
    {
        try
        {
            var notes =
                await _noteService.GetAllAsync(
                    request);


            return Ok(notes);
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


    // GET: /Note/Details/1
    [HttpGet]
    public async Task<IActionResult> Details(
        int id)
    {
        try
        {
            var note =
                await _noteService.GetByIdAsync(
                    id);


            if (!CanAccessNote(note.UserId))
                return Forbid();


            return Ok(
                note);
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


    // POST: /Note/Create
    [HttpPost]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> Create(
        [FromBody] CreateNoteDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(
                ModelState);


        try
        {
            var note =
                await _noteService.CreateAsync(
                    dto);


            return Ok(
                note);
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


    // PUT: /Note/Edit
    [HttpPut]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<IActionResult> Edit(
        [FromBody] EditNoteDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(
                ModelState);


        try
        {
            var note =
                await _noteService.UpdateAsync(
                    dto);


            return Ok(
                note);
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


    // DELETE: /Note/Delete/1
    [HttpDelete]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(
        int id)
    {
        try
        {
            await _noteService.DeleteAsync(
                id);


            return Ok(new
            {
                Message =
                    "Note deleted successfully."
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



    private bool CanAccessNote(
        string noteUserId)
    {
        var currentUserId =
            User.FindFirst(
                ClaimTypes.NameIdentifier)
            ?.Value;


        if (currentUserId == null)
            return false;


        if (User.IsInRole("Admin") ||
            User.IsInRole("Manager"))
        {
            return true;
        }


        return currentUserId == noteUserId;
    }
}