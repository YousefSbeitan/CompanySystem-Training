using CompanySystem.Business.DTOs;
using CompanySystem.Shared.Requests;
using CompanySystem.Shared.Responses;

namespace CompanySystem.Business.Interfaces;

public interface IMainPageSectionService
{
    Task<PagedResponse<MainPageSectionDto>> GetAllAsync(
        PaginationFilterRequest request);


    Task<MainPageSectionDto?> GetByIdAsync(
        int sectionId);


    Task<MainPageSectionDto> CreateAsync(
        CreateMainPageSectionDto dto);


    Task<MainPageSectionDto?> UpdateAsync(
        EditMainPageSectionDto dto);


    Task<bool> DeleteAsync(
        int sectionId);
}