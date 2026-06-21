namespace CompanySystem.Data.Entities;

using System;

public class MainPageSection
{
    public int SectionId { get; set; }

    public string SectionType { get; set; }

    public string Title { get; set; }

    public string Content { get; set; }

    public int UpdatedBy { get; set; }

    public DateTime UpdatedDate { get; set; }
}