using CompanySystem.Business.DTOs;

namespace CompanySystem.Business.Interfaces;

public interface INoteService
{
    Task<IEnumerable<NoteDto>> GetAllAsync();

    Task<NoteDto?> GetByIdAsync(int noteId);

    Task<NoteDto> CreateAsync(CreateNoteDto dto);

    Task<NoteDto?> UpdateAsync(EditNoteDto dto);

    Task<bool> DeleteAsync(int noteId);
}