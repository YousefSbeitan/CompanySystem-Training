using CompanySystem.Data.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace CompanySystem.Business.DTOs;

public class MainPageSectionDto
{
    public int SectionId { get; set; }

    public SectionType SectionType { get; set; }

    public string Title { get; set; }

    public string Content { get; set; }

    public string CreatedBy { get; set; }

    public DateTime CreatedDate { get; set; }

    public string? UpdatedBy { get; set; }

    public DateTime? UpdatedDate { get; set; }
}

public class CreateMainPageSectionDto
{
    public SectionType SectionType { get; set; }

    public string Title { get; set; }

    public string Content { get; set; }
}

public class EditMainPageSectionDto
{
    public int SectionId { get; set; }

    public SectionType SectionType { get; set; }

    public string Title { get; set; }

    public string Content { get; set; }
}