namespace CompanySystem.Data.Entities;

public class MainPageSection : TrackingEntity
{
    public int SectionId { get; set; }

    public string SectionType { get; set; }

    public string Title { get; set; }

    public string Content { get; set; }
}