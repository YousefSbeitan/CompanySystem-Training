using CompanySystem.Data.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace CompanySystem.Business.DTOs;

public class MainPageSectionDto
{
    public int SectionId { get; set; }

    public SectionType SectionType { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;

    public string CreatedBy { get; set; } = string.Empty;

    public DateTime CreatedDate { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }
}

public class CreateMainPageSectionDto
{
    [Required(ErrorMessage = "Section type is required.")]
    [Display(Name = "Section Type")]
    public SectionType SectionType { get; set; }

    [Required(ErrorMessage = "Title is required.")]
    [StringLength(200, ErrorMessage = "Title must not exceed 200 characters.")]
    [Display(Name = "Title")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Content is required.")]
    [Display(Name = "Content")]
    public string Content { get; set; } = string.Empty;
}

public class EditMainPageSectionDto
{
    [Required]
    public int SectionId { get; set; }

    [Required(ErrorMessage = "Section type is required.")]
    [Display(Name = "Section Type")]
    public SectionType SectionType { get; set; }

    [Required(ErrorMessage = "Title is required.")]
    [StringLength(200, ErrorMessage = "Title must not exceed 200 characters.")]
    [Display(Name = "Title")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "Content is required.")]
    [Display(Name = "Content")]
    public string Content { get; set; } = string.Empty;
}