using CompanySystem.Business.DTOs;
using CompanySystem.Business.Interfaces;
using CompanySystem.Business.Mappers;
using CompanySystem.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace CompanySystem.Business.Services;

public class NoteService : INoteService
{
    private readonly CompanySystemDbContext _context;

    public NoteService(CompanySystemDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<NoteDto>> GetAllAsync()
    {
        var notes = await _context.Notes
            .Where(n => !n.IsDeleted)
            .ToListAsync();

        return notes.Select(NoteMapper.ToDto);
    }

    public async Task<NoteDto?> GetByIdAsync(int noteId)
    {
        var note = await _context.Notes
            .FirstOrDefaultAsync(n =>
                n.NoteId == noteId &&
                !n.IsDeleted);

        if (note == null)
            return null;

        return NoteMapper.ToDto(note);
    }

    public async Task<NoteDto> CreateAsync(CreateNoteDto dto)
    {
        var note = NoteMapper.ToEntity(dto);

        note.CreatedBy = "System";

        _context.Notes.Add(note);

        await _context.SaveChangesAsync();

        return NoteMapper.ToDto(note);
    }

    public async Task<NoteDto?> UpdateAsync(EditNoteDto dto)
    {
        var note = await _context.Notes
            .FirstOrDefaultAsync(n =>
                n.NoteId == dto.NoteId &&
                !n.IsDeleted);

        if (note == null)
            return null;

        NoteMapper.UpdateEntity(note, dto);

        note.UpdatedBy = "System";
        note.UpdatedDate = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return NoteMapper.ToDto(note);
    }

    public async Task<bool> DeleteAsync(int noteId)
    {
        var note = await _context.Notes
            .FirstOrDefaultAsync(n =>
                n.NoteId == noteId &&
                !n.IsDeleted);

        if (note == null)
            return false;

        note.IsDeleted = true;
        note.UpdatedBy = "System";
        note.UpdatedDate = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return true;
    }
}