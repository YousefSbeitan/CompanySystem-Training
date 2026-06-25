using System.ComponentModel.DataAnnotations;
using CompanySystem.Data.Entities.Enums;

namespace CompanySystem.Web.ViewModels;

public class NoteViewModel
{
    public int NoteId { get; set; }

    [Display(Name = "User ID")]
    public string UserId { get; set; } = string.Empty;

    [Display(Name = "Note Type")]
    public NoteType NoteType { get; set; }

    [Display(Name = "Title")]
    public string Title { get; set; } = string.Empty;

    [Display(Name = "Content")]
    public string Content { get; set; } = string.Empty;

    [Display(Name = "Created By")]
    public string CreatedBy { get; set; } = string.Empty;

    [Display(Name = "Created Date")]
    public DateTime CreatedDate { get; set; }

    [Display(Name = "Updated By")]
    public string? UpdatedBy { get; set; }

    [Display(Name = "Updated Date")]
    public DateTime? UpdatedDate { get; set; }
}

public class CreateNoteViewModel
{
    [Required(ErrorMessage = "User ID is required")]
    [StringLength(50, ErrorMessage = "User ID must not exceed 50 characters")]
    [Display(Name = "User ID")]
    public string UserId { get; set; }

    [Required(ErrorMessage = "Note type is required")]
    [Display(Name = "Note Type")]
    public NoteType NoteType { get; set; }

    [Required(ErrorMessage = "Title is required")]
    [StringLength(200, ErrorMessage = "Title must not exceed 200 characters")]
    [Display(Name = "Title")]
    public string Title { get; set; }

    [Required(ErrorMessage = "Content is required")]
    [Display(Name = "Content")]
    public string Content { get; set; }
}

public class EditNoteViewModel
{
    public int NoteId { get; set; }

    [Required(ErrorMessage = "Note type is required")]
    [Display(Name = "Note Type")]
    public NoteType NoteType { get; set; }

    [Required(ErrorMessage = "Title is required")]
    [StringLength(200, ErrorMessage = "Title must not exceed 200 characters")]
    [Display(Name = "Title")]
    public string Title { get; set; }

    [Required(ErrorMessage = "Content is required")]
    [Display(Name = "Content")]
    public string Content { get; set; }
}