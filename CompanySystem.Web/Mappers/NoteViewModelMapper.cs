using CompanySystem.Business.DTOs;
using CompanySystem.Web.ViewModels;

namespace CompanySystem.Web.Mappers;

public static class NoteViewModelMapper
{
    public static NoteViewModel ToViewModel(NoteDto dto)
    {
        return new NoteViewModel
        {
            NoteId = dto.NoteId,
            UserId = dto.UserId,
            NoteType = dto.NoteType,
            Title = dto.Title,
            Content = dto.Content,
            CreatedBy = dto.CreatedBy,
            CreatedDate = dto.CreatedDate,
            UpdatedBy = dto.UpdatedBy,
            UpdatedDate = dto.UpdatedDate
        };
    }

    public static CreateNoteDto ToCreateDto(CreateNoteViewModel viewModel)
    {
        return new CreateNoteDto
        {
            UserId = viewModel.UserId,
            NoteType = viewModel.NoteType,
            Title = viewModel.Title,
            Content = viewModel.Content
        };
    }

    public static EditNoteDto ToEditDto(EditNoteViewModel viewModel)
    {
        return new EditNoteDto
        {
            NoteId = viewModel.NoteId,
            NoteType = viewModel.NoteType,
            Title = viewModel.Title,
            Content = viewModel.Content
        };
    }

    public static EditNoteViewModel ToEditViewModel(NoteDto dto)
    {
        return new EditNoteViewModel
        {
            NoteId = dto.NoteId,
            NoteType = dto.NoteType,
            Title = dto.Title,
            Content = dto.Content
        };
    }
}