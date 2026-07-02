using CompanySystem.Business.DTOs;
using CompanySystem.Business.Interfaces;
using CompanySystem.Business.Mappers;
using CompanySystem.Data.Entities;
using CompanySystem.Data.Repositories.Interfaces;
using CompanySystem.Shared.Exceptions;

namespace CompanySystem.Business.Services;

public class NoteService : INoteService
{
    private readonly IGenericRepository<Note> _repository;

    public NoteService(IGenericRepository<Note> repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<NoteDto>> GetAllAsync()
    {
        try
        {
            var notes = await _repository.FindAsync(n => !n.IsDeleted);

            return notes.Select(NoteMapper.ToDto);
        }
        catch (Exception ex)
        {
            throw new BusinessException("Failed to retrieve notes.", ex);
        }
    }

    public async Task<NoteDto?> GetByIdAsync(int noteId)
    {
        try
        {
            var note = await _repository.FirstOrDefaultAsync(
                n => n.NoteId == noteId &&
                     !n.IsDeleted);

            if (note == null)
                throw new ResourceNotFoundException("Note", noteId);

            return NoteMapper.ToDto(note);
        }
        catch (ResourceNotFoundException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new BusinessException("Failed to retrieve the note.", ex);
        }
    }

    public async Task<NoteDto> CreateAsync(CreateNoteDto dto)
    {
        try
        {
            var note = NoteMapper.ToEntity(dto);

            note.CreatedBy = "System";

            await _repository.AddAsync(note);

            await _repository.SaveChangesAsync();

            return NoteMapper.ToDto(note);
        }
        catch (Exception ex)
        {
            throw new BusinessException("Failed to create the note.", ex);
        }
    }

    public async Task<NoteDto?> UpdateAsync(EditNoteDto dto)
    {
        try
        {
            var note = await _repository.FirstOrDefaultAsync(
                n => n.NoteId == dto.NoteId &&
                     !n.IsDeleted);

            if (note == null)
                throw new ResourceNotFoundException("Note", dto.NoteId);

            NoteMapper.UpdateEntity(note, dto);

            note.UpdatedBy = "System";
            note.UpdatedDate = DateTime.UtcNow;

            _repository.Update(note);

            await _repository.SaveChangesAsync();

            return NoteMapper.ToDto(note);
        }
        catch (ResourceNotFoundException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new BusinessException("Failed to update the note.", ex);
        }
    }

    public async Task<bool> DeleteAsync(int noteId)
    {
        try
        {
            var note = await _repository.FirstOrDefaultAsync(
                n => n.NoteId == noteId &&
                     !n.IsDeleted);

            if (note == null)
                throw new ResourceNotFoundException("Note", noteId);

            note.IsDeleted = true;
            note.UpdatedBy = "System";
            note.UpdatedDate = DateTime.UtcNow;

            _repository.Update(note);

            await _repository.SaveChangesAsync();

            return true;
        }
        catch (ResourceNotFoundException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new BusinessException("Failed to delete the note.", ex);
        }
    }
}