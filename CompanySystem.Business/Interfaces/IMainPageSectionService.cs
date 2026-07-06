using CompanySystem.Business.DTOs;

namespace CompanySystem.Business.Interfaces;

public interface IMainPageSectionService
{
    Task<IEnumerable<MainPageSectionDto>> GetAllAsync();

    Task<MainPageSectionDto?> GetByIdAsync(int sectionId);

    Task<MainPageSectionDto> CreateAsync(CreateMainPageSectionDto dto);

    Task<MainPageSectionDto?> UpdateAsync(EditMainPageSectionDto dto);

    Task<bool> DeleteAsync(int sectionId);
}