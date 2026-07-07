using CompanySystem.Business.DTOs;
using CompanySystem.Data.Entities;

namespace CompanySystem.Business.Mappers;

public static class NoteMapper
{
    public static NoteDto ToDto(Note note)
    {
        return new NoteDto
        {
            NoteId = note.NoteId,
            UserId = note.UserId,
            NoteType = note.NoteType,
            Title = note.Title,
            Content = note.Content,
            CreatedBy = note.CreatedBy,
            CreatedDate = note.CreatedDate,
            UpdatedBy = note.UpdatedBy,
            UpdatedDate = note.UpdatedDate
        };
    }

    public static Note ToEntity(CreateNoteDto dto)
    {
        return new Note
        {
            UserId = dto.UserId,
            NoteType = dto.NoteType,
            Title = dto.Title,
            Content = dto.Content
        };
    }

    public static void UpdateEntity(
        Note note,
        EditNoteDto dto)
    {
        note.NoteType = dto.NoteType;
        note.Title = dto.Title;
        note.Content = dto.Content;
    }
}