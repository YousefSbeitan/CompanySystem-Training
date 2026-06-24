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
    [Required(ErrorMessage = "User ID is required")]
    [StringLength(50, ErrorMessage = "User ID must not exceed 50 characters")]
    public string UserId { get; set; }

    [Required(ErrorMessage = "Note type is required")]
    public NoteType NoteType { get; set; }

    [Required(ErrorMessage = "Title is required")]
    [StringLength(200, ErrorMessage = "Title must not exceed 200 characters")]
    public string Title { get; set; }

    [Required(ErrorMessage = "Content is required")]
    public string Content { get; set; }
}

public class EditNoteDto
{
    [Required]
    public int NoteId { get; set; }

    [Required(ErrorMessage = "Note type is required")]
    public NoteType NoteType { get; set; }

    [Required(ErrorMessage = "Title is required")]
    [StringLength(200, ErrorMessage = "Title must not exceed 200 characters")]
    public string Title { get; set; }

    [Required(ErrorMessage = "Content is required")]
    public string Content { get; set; }
}