using CompanySystem.Business.DTOs;
using CompanySystem.Business.Interfaces;
using CompanySystem.Business.Mappers;
using CompanySystem.Data.Entities;
using CompanySystem.Data.Repositories.Interfaces;
using CompanySystem.Shared.Exceptions;

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
        try
        {
            var sections = await _repository.FindAsync(
                s => !s.IsDeleted);

            return sections.Select(
                MainPageSectionMapper.ToDto);
        }
        catch (Exception ex)
        {
            throw new BusinessException(
                "Failed to retrieve main page sections.", ex);
        }
    }


    public async Task<MainPageSectionDto?> GetByIdAsync(int sectionId)
    {
        try
        {
            var section =
                await _repository.FirstOrDefaultAsync(
                    s => s.SectionId == sectionId &&
                         !s.IsDeleted);

            if (section == null)
                throw new ResourceNotFoundException(
                    "Main Page Section",
                    sectionId);


            return MainPageSectionMapper.ToDto(section);
        }
        catch (ResourceNotFoundException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new BusinessException(
                "Failed to retrieve the main page section.", ex);
        }
    }


    public async Task<MainPageSectionDto> CreateAsync(
        CreateMainPageSectionDto dto)
    {
        try
        {
            var existingSection =
                await _repository.FirstOrDefaultAsync(
                    s =>
                    (
                        s.SectionType == dto.SectionType
                        ||
                        s.Title.ToLower()
                        == dto.Title.ToLower()
                    )
                    &&
                    !s.IsDeleted);


            if (existingSection != null)
                throw new BusinessException(
                    "Main page section already exists.");


            var section =
                MainPageSectionMapper.ToEntity(dto);


            section.CreatedBy = "System";


            await _repository.AddAsync(section);

            await _repository.SaveChangesAsync();


            return MainPageSectionMapper.ToDto(section);
        }
        catch (BusinessException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new BusinessException(
                "Failed to create the main page section.", ex);
        }
    }


    public async Task<MainPageSectionDto?> UpdateAsync(
        EditMainPageSectionDto dto)
    {
        try
        {
            var section =
                await _repository.FirstOrDefaultAsync(
                    s => s.SectionId == dto.SectionId &&
                         !s.IsDeleted);


            if (section == null)
                throw new ResourceNotFoundException(
                    "Main Page Section",
                    dto.SectionId);


            var existingSection =
                await _repository.FirstOrDefaultAsync(
                    s =>
                    s.SectionId != dto.SectionId
                    &&
                    (
                        s.SectionType == dto.SectionType
                        ||
                        s.Title.ToLower()
                        == dto.Title.ToLower()
                    )
                    &&
                    !s.IsDeleted);


            if (existingSection != null)
                throw new BusinessException(
                    "Main page section already exists.");


            MainPageSectionMapper.UpdateEntity(
                section,
                dto);


            section.UpdatedBy = "System";
            section.UpdatedDate = DateTime.UtcNow;


            _repository.Update(section);

            await _repository.SaveChangesAsync();


            return MainPageSectionMapper.ToDto(section);
        }
        catch (ResourceNotFoundException)
        {
            throw;
        }
        catch (BusinessException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new BusinessException(
                "Failed to update the main page section.", ex);
        }
    }


    public async Task<bool> DeleteAsync(int sectionId)
    {
        try
        {
            var section =
                await _repository.FirstOrDefaultAsync(
                    s => s.SectionId == sectionId &&
                         !s.IsDeleted);


            if (section == null)
                throw new ResourceNotFoundException(
                    "Main Page Section",
                    sectionId);


            section.IsDeleted = true;

            section.UpdatedBy = "System";
            section.UpdatedDate = DateTime.UtcNow;


            _repository.Update(section);

            await _repository.SaveChangesAsync();


            return true;
        }
        catch (ResourceNotFoundException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new BusinessException(
                "Failed to delete the main page section.", ex);
        }
    }
}