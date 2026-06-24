using System.ComponentModel.DataAnnotations;

namespace CompanySystem.Business.DTOs;

public class MainPageSectionDto
{
    public int SectionId { get; set; }

    public string SectionType { get; set; }

    public string Title { get; set; }

    public string Content { get; set; }

    public string CreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }
}

public class CreateMainPageSectionDto
{
    [Required(ErrorMessage = "Section type is required")]
    [StringLength(100, ErrorMessage = "Section type must not exceed 100 characters")]
    public string SectionType { get; set; }

    [Required(ErrorMessage = "Title is required")]
    [StringLength(200, ErrorMessage = "Title must not exceed 200 characters")]
    public string Title { get; set; }

    [Required(ErrorMessage = "Content is required")]
    public string Content { get; set; }
}

public class EditMainPageSectionDto
{
    [Required]
    public int SectionId { get; set; }

    [Required(ErrorMessage = "Section type is required")]
    [StringLength(100, ErrorMessage = "Section type must not exceed 100 characters")]
    public string SectionType { get; set; }

    [Required(ErrorMessage = "Title is required")]
    [StringLength(200, ErrorMessage = "Title must not exceed 200 characters")]
    public string Title { get; set; }

    [Required(ErrorMessage = "Content is required")]
    public string Content { get; set; }
}