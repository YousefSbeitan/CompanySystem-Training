using CompanySystem.Business.DTOs;
using CompanySystem.Business.Interfaces;
using CompanySystem.Business.Mappers;
using CompanySystem.Data.Entities;
using CompanySystem.Data.Repositories.Interfaces;

namespace CompanySystem.Business.Services;

public class NoteService : INoteService
{
    private readonly IGenericRepository<Note> _repository;

    public NoteService(
        IGenericRepository<Note> repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<NoteDto>> GetAllAsync()
    {
        var notes = await _repository.FindAsync(
            n => !n.IsDeleted);

        return notes.Select(NoteMapper.ToDto);
    }

    public async Task<NoteDto?> GetByIdAsync(int noteId)
    {
        var note = await _repository.FirstOrDefaultAsync(
            n => n.NoteId == noteId &&
                 !n.IsDeleted);

        if (note == null)
            return null;

        return NoteMapper.ToDto(note);
    }

    public async Task<NoteDto> CreateAsync(CreateNoteDto dto)
    {
        var note = NoteMapper.ToEntity(dto);

        note.CreatedBy = "System";

        await _repository.AddAsync(note);

        await _repository.SaveChangesAsync();

        return NoteMapper.ToDto(note);
    }

    public async Task<NoteDto?> UpdateAsync(EditNoteDto dto)
    {
        var note = await _repository.FirstOrDefaultAsync(
            n => n.NoteId == dto.NoteId &&
                 !n.IsDeleted);

        if (note == null)
            return null;

        NoteMapper.UpdateEntity(note, dto);

        note.UpdatedBy = "System";
        note.UpdatedDate = DateTime.UtcNow;

        _repository.Update(note);

        await _repository.SaveChangesAsync();

        return NoteMapper.ToDto(note);
    }

    public async Task<bool> DeleteAsync(int noteId)
    {
        var note = await _repository.FirstOrDefaultAsync(
            n => n.NoteId == noteId &&
                 !n.IsDeleted);

        if (note == null)
            return false;

        note.IsDeleted = true;
        note.UpdatedBy = "System";
        note.UpdatedDate = DateTime.UtcNow;

        _repository.Update(note);

        await _repository.SaveChangesAsync();

        return true;
    }
}