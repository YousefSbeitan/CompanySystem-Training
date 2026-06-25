using CompanySystem.Business.DTOs;
using CompanySystem.Business.Interfaces;
using CompanySystem.Business.Mappers;
using CompanySystem.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace CompanySystem.Business.Services;

public class MainPageSectionService : IMainPageSectionService
{
    private readonly CompanySystemDbContext _context;

    public MainPageSectionService(CompanySystemDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<MainPageSectionDto>> GetAllAsync()
    {
        var sections = await _context.MainPageSections
            .Where(s => !s.IsDeleted)
            .ToListAsync();

        return sections.Select(MainPageSectionMapper.ToDto);
    }

    public async Task<MainPageSectionDto?> GetByIdAsync(int sectionId)
    {
        var section = await _context.MainPageSections
            .FirstOrDefaultAsync(s =>
                s.SectionId == sectionId &&
                !s.IsDeleted);

        if (section == null)
            return null;

        return MainPageSectionMapper.ToDto(section);
    }

    public async Task<MainPageSectionDto> CreateAsync(CreateMainPageSectionDto dto)
    {
        var section = MainPageSectionMapper.ToEntity(dto);

        section.CreatedBy = "System";

        _context.MainPageSections.Add(section);

        await _context.SaveChangesAsync();

        return MainPageSectionMapper.ToDto(section);
    }

    public async Task<MainPageSectionDto?> UpdateAsync(EditMainPageSectionDto dto)
    {
        var section = await _context.MainPageSections
            .FirstOrDefaultAsync(s =>
                s.SectionId == dto.SectionId &&
                !s.IsDeleted);

        if (section == null)
            return null;

        MainPageSectionMapper.UpdateEntity(section, dto);

        section.UpdatedBy = "System";
        section.UpdatedDate = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return MainPageSectionMapper.ToDto(section);
    }

    public async Task<bool> DeleteAsync(int sectionId)
    {
        var section = await _context.MainPageSections
            .FirstOrDefaultAsync(s =>
                s.SectionId == sectionId &&
                !s.IsDeleted);

        if (section == null)
            return false;

        section.IsDeleted = true;
        section.UpdatedBy = "System";
        section.UpdatedDate = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return true;
    }
}