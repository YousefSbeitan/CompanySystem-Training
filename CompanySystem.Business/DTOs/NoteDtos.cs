using System.ComponentModel.DataAnnotations;
using CompanySystem.Data.Entities.Enums;

namespace CompanySystem.Business.DTOs;

public class NoteDto : TrackingDto
{
    public int NoteId { get; set; }

    public string UserId { get; set; } = string.Empty;

    public NoteType NoteType { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;
}

public class CreateNoteDto
{
    [Required(ErrorMessage = "User ID is required.")]
    [StringLength(50, ErrorMessage = "User ID must not exceed 50 characters.")]
    [Display(Name = "User")]
    public string UserId { get; set; } = string.Empty;

    [Required(ErrorMessage = "Note type is required.")]
    [Display(Name = "Note Type")]
    public NoteType NoteType { get; set; }

    [Required(ErrorMessage = "Title is required.")]
    [StringLength(200, ErrorMessage = "Title must not exceed 200 characters.")]
    [Display(Name = "Title")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Content is required.")]
    [Display(Name = "Content")]
    public string Content { get; set; } = string.Empty;
}

public class EditNoteDto
{
    [Required]
    public int NoteId { get; set; }

    [Required(ErrorMessage = "Note type is required.")]
    [Display(Name = "Note Type")]
    public NoteType NoteType { get; set; }

    [Required(ErrorMessage = "Title is required.")]
    [StringLength(200, ErrorMessage = "Title must not exceed 200 characters.")]
    [Display(Name = "Title")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Content is required.")]
    [Display(Name = "Content")]
    public string Content { get; set; } = string.Empty;
}