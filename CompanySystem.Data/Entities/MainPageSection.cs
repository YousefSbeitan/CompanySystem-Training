using System.ComponentModel.DataAnnotations;

namespace CompanySystem.Data.Entities;

public class MainPageSection : TrackingEntity
{
    [Key]
    public int SectionId { get; set; }

    public string SectionType { get; set; }

    public string Title { get; set; }

    public string Content { get; set; }
}