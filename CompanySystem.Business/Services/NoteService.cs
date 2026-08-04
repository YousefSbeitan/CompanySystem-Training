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
    PaginationFilterRequest request,
    string currentUserId,
    string currentUserRole)
    {
        try
        {
            var notes =
                await _noteRepository.FindAsync(
                    n => !n.IsDeleted);


            if (currentUserRole == "Employee")
            {
                notes =
                    notes.Where(
                        n => n.UserId == currentUserId);
            }


            if (currentUserRole == "Manager")
            {
                var employees =
                    await _userRepository.FindAsync(
                        u => u.LeaderId == currentUserId &&
                             !u.IsDeleted);


                var employeeIds =
                    employees
                    .Select(
                        u => u.UserId)
                    .ToList();


                employeeIds.Add(
                    currentUserId);


                notes =
                    notes.Where(
                        n => employeeIds.Contains(
                            n.UserId));
            }


            // Filtering
            if (!string.IsNullOrWhiteSpace(
                    request.Search))
            {
                notes =
                    notes.Where(
                        n =>
                        n.Title.ToLower()
                            .Contains(
                                request.Search.ToLower())
                        ||
                        n.Content.ToLower()
                            .Contains(
                                request.Search.ToLower())
                        ||
                        n.NoteType.ToString()
                            .ToLower()
                            .Contains(
                                request.Search.ToLower()));
            }


            // Sorting
            notes =
                request.SortBy?.ToLower() switch
                {
                    "title" =>
                        request.IsDescending
                            ? notes.OrderByDescending(
                                n => n.Title)
                            : notes.OrderBy(
                                n => n.Title),


                    "notetype" =>
                        request.IsDescending
                            ? notes.OrderByDescending(
                                n => n.NoteType)
                            : notes.OrderBy(
                                n => n.NoteType),


                    "createddate" =>
                        request.IsDescending
                            ? notes.OrderByDescending(
                                n => n.CreatedDate)
                            : notes.OrderBy(
                                n => n.CreatedDate),


                    _ =>
                        notes.OrderBy(
                            n => n.NoteId)
                };


            var totalRecords =
                notes.Count();


            var pagedNotes =
                notes
                .Skip(
                    (request.PageNumber - 1)
                    *
                    request.PageSize)
                .Take(
                    request.PageSize)
                .Select(
                    NoteMapper.ToDto)
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
                "Failed to retrieve notes.",
                ex);
        }
    }


    public async Task<NoteDto?> GetByIdAsync(int noteId)
    {
        try
        {
            if (noteId <= 0)
                throw new BusinessException(
                    "Invalid note id.");


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
        catch (BusinessException)
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
            if (dto == null)
                throw new BusinessException(
                    "Note data is required.");


            if (string.IsNullOrWhiteSpace(dto.UserId))
                throw new BusinessException(
                    "User id is required.");


            if (string.IsNullOrWhiteSpace(dto.Title))
                throw new BusinessException(
                    "Title is required.");


            if (string.IsNullOrWhiteSpace(dto.Content))
                throw new BusinessException(
                    "Content is required.");


            dto.UserId = dto.UserId.Trim();
            dto.Title = dto.Title.Trim();
            dto.Content = dto.Content.Trim();


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
        catch (BusinessException)
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
            if (dto == null)
                throw new BusinessException(
                    "Note data is required.");


            if (dto.NoteId <= 0)
                throw new BusinessException(
                    "Invalid note id.");


            if (string.IsNullOrWhiteSpace(dto.Title))
                throw new BusinessException(
                    "Title is required.");


            if (string.IsNullOrWhiteSpace(dto.Content))
                throw new BusinessException(
                    "Content is required.");


            dto.Title = dto.Title.Trim();
            dto.Content = dto.Content.Trim();


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
        catch (BusinessException)
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
            if (noteId <= 0)
                throw new BusinessException(
                    "Invalid note id.");


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
        catch (BusinessException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new BusinessException(
                "Failed to delete the note.", ex);
        }
    }

    public async Task<bool> CanAccessNoteAsync(
    int noteId,
    string currentUserId,
    string currentUserRole)
    {
        try
        {
            var note =
                await _noteRepository.FirstOrDefaultAsync(
                    n => n.NoteId == noteId &&
                         !n.IsDeleted);


            if (note == null)
                return false;


            if (currentUserRole == "Admin")
                return true;


            if (note.UserId == currentUserId)
                return true;


            if (currentUserRole == "Manager")
            {
                var user =
                    await _userRepository.FirstOrDefaultAsync(
                        u => u.UserId == note.UserId &&
                             !u.IsDeleted);


                if (user == null)
                    return false;


                return user.LeaderId ==
                       currentUserId;
            }


            return false;
        }
        catch (Exception ex)
        {
            throw new BusinessException(
                "Failed to check note permission.",
                ex);
        }
    }
}