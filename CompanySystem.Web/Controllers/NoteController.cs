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
    private readonly IUserService _userService;


    public NoteController(
    INoteService noteService,
    IUserService userService)
    {
        _noteService = noteService;
        _userService = userService;
    }


    // GET: /Note
    [HttpGet]
    public async Task<IActionResult> Index(
        [FromQuery] PaginationFilterRequest request)
    {
        try
        {
            var currentUserId =
                GetCurrentUserId();

            var currentUserRole =
                GetCurrentUserRole();


            if (currentUserId == null ||
                currentUserRole == null)
                return Unauthorized();


            var notes =
                await _noteService.GetAllAsync(
                    request,
                    currentUserId,
                    currentUserRole);


            return Ok(
                notes);
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
            var currentUserId =
                GetCurrentUserId();

            var currentUserRole =
                GetCurrentUserRole();


            if (currentUserId == null ||
                currentUserRole == null)
                return Unauthorized();


            var canAccess =
                await _noteService.CanAccessNoteAsync(
                    id,
                    currentUserId,
                    currentUserRole);


            if (!canAccess)
                return Forbid();


            var note =
                await _noteService.GetByIdAsync(
                    id);


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
            var currentUserId =
                GetCurrentUserId();


            var currentUserRole =
                GetCurrentUserRole();


            if (currentUserId == null ||
                currentUserRole == null)
            {
                return Unauthorized();
            }


            if (currentUserRole == "Manager")
            {
                var canCreate =
                    dto.UserId == currentUserId
                    ||
                    await _userService
                        .CanManageUserAsync(
                            currentUserId,
                            dto.UserId);


                if (!canCreate)
                    return Forbid();
            }


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
            var currentUserId =
                GetCurrentUserId();

            var currentUserRole =
                GetCurrentUserRole();


            if (currentUserId == null ||
                currentUserRole == null)
                return Unauthorized();


            var canAccess =
                await _noteService.CanAccessNoteAsync(
                    dto.NoteId,
                    currentUserId,
                    currentUserRole);


            if (!canAccess)
                return Forbid();


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



    private string? GetCurrentUserId()
    {
        return User.FindFirst(
            ClaimTypes.NameIdentifier)
            ?.Value;
    }


    private string? GetCurrentUserRole()
    {
        return User.FindFirst(
            ClaimTypes.Role)
            ?.Value;
    }
}