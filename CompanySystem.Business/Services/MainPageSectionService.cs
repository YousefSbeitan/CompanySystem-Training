using CompanySystem.Business.DTOs;
using CompanySystem.Business.Interfaces;
using CompanySystem.Business.Mappers;
using CompanySystem.Data.Entities;
using CompanySystem.Data.Repositories.Interfaces;
using CompanySystem.Shared.Exceptions;
using CompanySystem.Shared.Requests;
using CompanySystem.Shared.Responses;

namespace CompanySystem.Business.Services;

public class MainPageSectionService : IMainPageSectionService
{
    private readonly IGenericRepository<MainPageSection> _repository;

    public MainPageSectionService(
        IGenericRepository<MainPageSection> repository)
    {
        _repository = repository;
    }


    public async Task<PagedResponse<MainPageSectionDto>> GetAllAsync(
        PaginationFilterRequest request)
    {
        try
        {
            var sections =
                await _repository.FindAsync(
                    s => !s.IsDeleted);


            // Filtering
            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                sections = sections.Where(
                    s =>
                    s.Title.ToLower()
                        .Contains(request.Search.ToLower())
                    ||
                    s.Content.ToLower()
                        .Contains(request.Search.ToLower())
                    ||
                    s.SectionType.ToString()
                        .ToLower()
                        .Contains(request.Search.ToLower()));
            }


            // Sorting
            sections = request.SortBy?.ToLower() switch
            {
                "title" => request.IsDescending
                    ? sections.OrderByDescending(s => s.Title)
                    : sections.OrderBy(s => s.Title),


                "sectiontype" => request.IsDescending
                    ? sections.OrderByDescending(s => s.SectionType)
                    : sections.OrderBy(s => s.SectionType),


                "updateddate" => request.IsDescending
                    ? sections.OrderByDescending(s => s.UpdatedDate)
                    : sections.OrderBy(s => s.UpdatedDate),


                "createddate" => request.IsDescending
                    ? sections.OrderByDescending(s => s.CreatedDate)
                    : sections.OrderBy(s => s.CreatedDate),


                _ => sections.OrderBy(s => s.SectionId)
            };


            var totalRecords =
                sections.Count();


            var pagedSections =
                sections
                .Skip(
                    (request.PageNumber - 1)
                    * request.PageSize)
                .Take(request.PageSize)
                .Select(MainPageSectionMapper.ToDto)
                .ToList();


            return new PagedResponse<MainPageSectionDto>(
                pagedSections,
                request.PageNumber,
                request.PageSize,
                totalRecords);
        }
        catch (Exception ex)
        {
            throw new BusinessException(
                "Failed to retrieve main page sections.", ex);
        }
    }


    public async Task<MainPageSectionDto?> GetByIdAsync(
        int sectionId)
    {
        try
        {
            if (sectionId <= 0)
                throw new BusinessException(
                    "Invalid section id.");


            var section =
                await _repository.FirstOrDefaultAsync(
                    s => s.SectionId == sectionId &&
                         !s.IsDeleted);


            if (section == null)
                throw new ResourceNotFoundException(
                    "Main Page Section",
                    sectionId);


            return MainPageSectionMapper.ToDto(
                section);
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
                "Failed to retrieve the main page section.", ex);
        }
    }


    public async Task<MainPageSectionDto> CreateAsync(
        CreateMainPageSectionDto dto)
    {
        try
        {
            if (dto == null)
                throw new BusinessException(
                    "Section data is required.");


            if (string.IsNullOrWhiteSpace(dto.Title))
                throw new BusinessException(
                    "Title is required.");


            if (string.IsNullOrWhiteSpace(dto.Content))
                throw new BusinessException(
                    "Content is required.");


            dto.Title =
                dto.Title.Trim();

            dto.Content =
                dto.Content.Trim();


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
                MainPageSectionMapper.ToEntity(
                    dto);


            section.CreatedBy =
                "System";


            await _repository.AddAsync(
                section);


            await _repository.SaveChangesAsync();


            return MainPageSectionMapper.ToDto(
                section);
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
            if (dto == null)
                throw new BusinessException(
                    "Section data is required.");


            if (dto.SectionId <= 0)
                throw new BusinessException(
                    "Invalid section id.");


            if (string.IsNullOrWhiteSpace(dto.Title))
                throw new BusinessException(
                    "Title is required.");


            if (string.IsNullOrWhiteSpace(dto.Content))
                throw new BusinessException(
                    "Content is required.");


            dto.Title =
                dto.Title.Trim();

            dto.Content =
                dto.Content.Trim();


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


            section.UpdatedBy =
                "System";

            section.UpdatedDate =
                DateTime.UtcNow;


            _repository.Update(
                section);


            await _repository.SaveChangesAsync();


            return MainPageSectionMapper.ToDto(
                section);
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


    public async Task<bool> DeleteAsync(
        int sectionId)
    {
        try
        {
            if (sectionId <= 0)
                throw new BusinessException(
                    "Invalid section id.");


            var section =
                await _repository.FirstOrDefaultAsync(
                    s => s.SectionId == sectionId &&
                         !s.IsDeleted);


            if (section == null)
                throw new ResourceNotFoundException(
                    "Main Page Section",
                    sectionId);


            section.IsDeleted = true;

            section.UpdatedBy =
                "System";

            section.UpdatedDate =
                DateTime.UtcNow;


            _repository.Update(
                section);


            await _repository.SaveChangesAsync();


            return true;
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
                "Failed to delete the main page section.", ex);
        }
    }
}