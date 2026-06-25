using System.ComponentModel.DataAnnotations;
using CompanySystem.Data.Entities.Enums;

namespace CompanySystem.Web.ViewModels;

public class MainPageSectionViewModel
{
    public int SectionId { get; set; }

    [Display(Name = "Section Type")]
    public SectionType SectionType { get; set; }

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

public class CreateMainPageSectionViewModel
{
    [Required(ErrorMessage = "Section type is required")]
    [Display(Name = "Section Type")]
    public SectionType SectionType { get; set; }

    [Required(ErrorMessage = "Title is required")]
    [StringLength(200, ErrorMessage = "Title must not exceed 200 characters")]
    [Display(Name = "Title")]
    public string Title { get; set; }

    [Required(ErrorMessage = "Content is required")]
    [Display(Name = "Content")]
    public string Content { get; set; }
}

public class EditMainPageSectionViewModel
{
    public int SectionId { get; set; }

    [Required(ErrorMessage = "Section type is required")]
    [Display(Name = "Section Type")]
    public SectionType SectionType { get; set; }

    [Required(ErrorMessage = "Title is required")]
    [StringLength(200, ErrorMessage = "Title must not exceed 200 characters")]
    [Display(Name = "Title")]
    public string Title { get; set; }

    [Required(ErrorMessage = "Content is required")]
    [Display(Name = "Content")]
    public string Content { get; set; }
}