using CompanySystem.Business.DTOs;
using CompanySystem.Business.Interfaces;
using CompanySystem.Business.Mappers;
using CompanySystem.Data.Entities;
using CompanySystem.Data.Repositories.Interfaces;
using CompanySystem.Shared.Exceptions;
using CompanySystem.Shared.Requests;
using CompanySystem.Shared.Responses;

namespace CompanySystem.Business.Services;

public class NoteService : INoteService
{
    private readonly IGenericRepository<Note> _noteRepository;
    private readonly IGenericRepository<User> _userRepository;


    public NoteService(
        IGenericRepository<Note> noteRepository,
        IGenericRepository<User> userRepository)
    {
        _noteRepository = noteRepository;
        _userRepository = userRepository;
    }


    public async Task<PagedResponse<NoteDto>> GetAllAsync(
        PaginationFilterRequest request)
    {
        try
        {
            var notes =
                await _noteRepository.FindAsync(
                    n => !n.IsDeleted);


            // Filtering
            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                notes = notes.Where(
                    n =>
                    n.Title.ToLower()
                        .Contains(request.Search.ToLower())
                    ||
                    n.Content.ToLower()
                        .Contains(request.Search.ToLower())
                    ||
                    n.NoteType.ToString()
                        .ToLower()
                        .Contains(request.Search.ToLower()));
            }


            // Sorting
            notes = request.SortBy?.ToLower() switch
            {
                "title" => request.IsDescending
                    ? notes.OrderByDescending(n => n.Title)
                    : notes.OrderBy(n => n.Title),


                "notetype" => request.IsDescending
                    ? notes.OrderByDescending(n => n.NoteType)
                    : notes.OrderBy(n => n.NoteType),


                "createddate" => request.IsDescending
                    ? notes.OrderByDescending(n => n.CreatedDate)
                    : notes.OrderBy(n => n.CreatedDate),


                _ => notes.OrderBy(n => n.NoteId)
            };


            var totalRecords =
                notes.Count();


            var pagedNotes =
                notes
                .Skip(
                    (request.PageNumber - 1)
                    * request.PageSize)
                .Take(request.PageSize)
                .Select(NoteMapper.ToDto)
                .ToList();


            return new PagedResponse<NoteDto>(
                pagedNotes,
                request.PageNumber,
                request.PageSize,
                totalRecords);
        }
        catch (Exception ex)
        {
            throw new BusinessException(
                "Failed to retrieve notes.", ex);
        }
    }


    public async Task<NoteDto?> GetByIdAsync(int noteId)
    {
        try
        {
            var note =
                await _noteRepository.FirstOrDefaultAsync(
                    n => n.NoteId == noteId &&
                         !n.IsDeleted);


            if (note == null)
                throw new ResourceNotFoundException(
                    "Note",
                    noteId);


            return NoteMapper.ToDto(note);
        }
        catch (ResourceNotFoundException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new BusinessException(
                "Failed to retrieve the note.", ex);
        }
    }


    public async Task<NoteDto> CreateAsync(
        CreateNoteDto dto)
    {
        try
        {
            var user =
                await _userRepository.FirstOrDefaultAsync(
                    u => u.UserId == dto.UserId &&
                         !u.IsDeleted);


            if (user == null)
                throw new ResourceNotFoundException(
                    "User",
                    dto.UserId);


            var note =
                NoteMapper.ToEntity(dto);


            note.CreatedBy = "System";


            await _noteRepository.AddAsync(note);


            await _noteRepository.SaveChangesAsync();


            return NoteMapper.ToDto(note);
        }
        catch (ResourceNotFoundException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new BusinessException(
                "Failed to create the note.", ex);
        }
    }


    public async Task<NoteDto?> UpdateAsync(
        EditNoteDto dto)
    {
        try
        {
            var note =
                await _noteRepository.FirstOrDefaultAsync(
                    n => n.NoteId == dto.NoteId &&
                         !n.IsDeleted);


            if (note == null)
                throw new ResourceNotFoundException(
                    "Note",
                    dto.NoteId);


            NoteMapper.UpdateEntity(
                note,
                dto);


            note.UpdatedBy = "System";
            note.UpdatedDate = DateTime.UtcNow;


            _noteRepository.Update(note);


            await _noteRepository.SaveChangesAsync();


            return NoteMapper.ToDto(note);
        }
        catch (ResourceNotFoundException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new BusinessException(
                "Failed to update the note.", ex);
        }
    }


    public async Task<bool> DeleteAsync(
        int noteId)
    {
        try
        {
            var note =
                await _noteRepository.FirstOrDefaultAsync(
                    n => n.NoteId == noteId &&
                         !n.IsDeleted);


            if (note == null)
                throw new ResourceNotFoundException(
                    "Note",
                    noteId);


            note.IsDeleted = true;

            note.UpdatedBy = "System";
            note.UpdatedDate = DateTime.UtcNow;


            _noteRepository.Update(note);


            await _noteRepository.SaveChangesAsync();


            return true;
        }
        catch (ResourceNotFoundException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new BusinessException(
                "Failed to delete the note.", ex);
        }
    }
}