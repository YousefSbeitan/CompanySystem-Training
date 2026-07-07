using CompanySystem.Business.DTOs;
using CompanySystem.Shared.Requests;
using CompanySystem.Shared.Responses;

namespace CompanySystem.Business.Interfaces;

public interface INoteService
{
    Task<PagedResponse<NoteDto>> GetAllAsync(
        PaginationFilterRequest request);


    Task<NoteDto?> GetByIdAsync(
        int noteId);


    Task<NoteDto> CreateAsync(
        CreateNoteDto dto);


    Task<NoteDto?> UpdateAsync(
        EditNoteDto dto);


    Task<bool> DeleteAsync(
        int noteId);
}