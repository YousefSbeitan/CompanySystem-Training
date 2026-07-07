using CompanySystem.Business.DTOs;
using CompanySystem.Data.Entities;

namespace CompanySystem.Business.Mappers;

public static class MainPageSectionMapper
{
    public static MainPageSectionDto ToDto(MainPageSection section)
    {
        return new MainPageSectionDto
        {
            SectionId = section.SectionId,
            SectionType = section.SectionType,
            Title = section.Title,
            Content = section.Content,
            CreatedBy = section.CreatedBy,
            CreatedDate = section.CreatedDate,
            UpdatedBy = section.UpdatedBy,
            UpdatedDate = section.UpdatedDate
        };
    }

    public static MainPageSection ToEntity(CreateMainPageSectionDto dto)
    {
        return new MainPageSection
        {
            SectionType = dto.SectionType,
            Title = dto.Title,
            Content = dto.Content
        };
    }

    public static void UpdateEntity(
        MainPageSection section,
        EditMainPageSectionDto dto)
    {
        section.SectionType = dto.SectionType;
        section.Title = dto.Title;
        section.Content = dto.Content;
    }
}