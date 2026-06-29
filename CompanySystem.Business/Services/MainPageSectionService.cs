using CompanySystem.Business.DTOs;
using CompanySystem.Business.Interfaces;
using CompanySystem.Business.Mappers;
using CompanySystem.Data.Entities;
using CompanySystem.Data.Repositories.Interfaces;

namespace CompanySystem.Business.Services;

public class MainPageSectionService : IMainPageSectionService
{
    private readonly IGenericRepository<MainPageSection> _repository;

    public MainPageSectionService(
        IGenericRepository<MainPageSection> repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<MainPageSectionDto>> GetAllAsync()
    {
        var sections = await _repository.FindAsync(
            s => !s.IsDeleted);

        return sections.Select(MainPageSectionMapper.ToDto);
    }

    public async Task<MainPageSectionDto?> GetByIdAsync(int sectionId)
    {
        var section = await _repository.FirstOrDefaultAsync(
            s => s.SectionId == sectionId &&
                 !s.IsDeleted);

        if (section == null)
            return null;

        return MainPageSectionMapper.ToDto(section);
    }

    public async Task<MainPageSectionDto> CreateAsync(CreateMainPageSectionDto dto)
    {
        var section = MainPageSectionMapper.ToEntity(dto);

        section.CreatedBy = "System";

        await _repository.AddAsync(section);

        await _repository.SaveChangesAsync();

        return MainPageSectionMapper.ToDto(section);
    }

    public async Task<MainPageSectionDto?> UpdateAsync(EditMainPageSectionDto dto)
    {
        var section = await _repository.FirstOrDefaultAsync(
            s => s.SectionId == dto.SectionId &&
                 !s.IsDeleted);

        if (section == null)
            return null;

        MainPageSectionMapper.UpdateEntity(section, dto);

        section.UpdatedBy = "System";
        section.UpdatedDate = DateTime.UtcNow;

        _repository.Update(section);

        await _repository.SaveChangesAsync();

        return MainPageSectionMapper.ToDto(section);
    }

    public async Task<bool> DeleteAsync(int sectionId)
    {
        var section = await _repository.FirstOrDefaultAsync(
            s => s.SectionId == sectionId &&
                 !s.IsDeleted);

        if (section == null)
            return false;

        section.IsDeleted = true;
        section.UpdatedBy = "System";
        section.UpdatedDate = DateTime.UtcNow;

        _repository.Update(section);

        await _repository.SaveChangesAsync();

        return true;
    }
}