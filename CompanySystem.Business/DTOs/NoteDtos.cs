using System.ComponentModel.DataAnnotations;
using CompanySystem.Data.Entities.Enums;

namespace CompanySystem.Business.DTOs;

public class NoteDto
{
    public int NoteId { get; set; }

    public string UserId { get; set; }

    public NoteType NoteType { get; set; }

    public string Title { get; set; }

    public string Content { get; set; }

    public string CreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }
}

public class CreateNoteDto
{
    public string UserId { get; set; }

    public NoteType NoteType { get; set; }

    public string Title { get; set; }

    public string Content { get; set; }
}

public class EditNoteDto
{
    public int NoteId { get; set; }

    public NoteType NoteType { get; set; }

    public string Title { get; set; }

    public string Content { get; set; }
}