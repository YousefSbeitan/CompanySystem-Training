using CompanySystem.Business.DTOs;
using CompanySystem.Web.ViewModels;

namespace CompanySystem.Web.Mappers;

public static class MainPageSectionViewModelMapper
{
    public static MainPageSectionViewModel ToViewModel(MainPageSectionDto dto)
    {
        return new MainPageSectionViewModel
        {
            SectionId = dto.SectionId,
            SectionType = dto.SectionType,
            Title = dto.Title,
            Content = dto.Content,
            CreatedBy = dto.CreatedBy,
            CreatedDate = dto.CreatedDate,
            UpdatedBy = dto.UpdatedBy,
            UpdatedDate = dto.UpdatedDate
        };
    }

    public static CreateMainPageSectionDto ToCreateDto(CreateMainPageSectionViewModel viewModel)
    {
        return new CreateMainPageSectionDto
        {
            SectionType = viewModel.SectionType,
            Title = viewModel.Title,
            Content = viewModel.Content
        };
    }

    public static EditMainPageSectionDto ToEditDto(EditMainPageSectionViewModel viewModel)
    {
        return new EditMainPageSectionDto
        {
            SectionId = viewModel.SectionId,
            SectionType = viewModel.SectionType,
            Title = viewModel.Title,
            Content = viewModel.Content
        };
    }

    public static EditMainPageSectionViewModel ToEditViewModel(MainPageSectionDto dto)
    {
        return new EditMainPageSectionViewModel
        {
            SectionId = dto.SectionId,
            SectionType = dto.SectionType,
            Title = dto.Title,
            Content = dto.Content
        };
    }
}