using CompanySystem.Business.DTOs;
using CompanySystem.Business.Interfaces;
using CompanySystem.Business.Mappers;
using CompanySystem.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace CompanySystem.Business.Services;

public class UserService : IUserService
{
    private readonly CompanySystemDbContext _context;

    public UserService(CompanySystemDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<UserDto>> GetAllAsync()
    {
        var users = await _context.Users
            .Where(u => !u.IsDeleted)
            .ToListAsync();

        return users.Select(UserMapper.ToDto);
    }

    public async Task<UserDto?> GetByIdAsync(string userId)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u =>
                u.UserId == userId &&
                !u.IsDeleted);

        if (user == null)
            return null;

        return UserMapper.ToDto(user);
    }

    public async Task<UserDto> CreateAsync(CreateUserDto dto)
    {
        var user = UserMapper.ToEntity(dto);

        user.CreatedBy = "System";

        _context.Users.Add(user);

        await _context.SaveChangesAsync();

        return UserMapper.ToDto(user);
    }

    public async Task<UserDto?> UpdateAsync(EditUserDto dto)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u =>
                u.UserId == dto.UserId &&
                !u.IsDeleted);

        if (user == null)
            return null;

        UserMapper.UpdateEntity(user, dto);

        user.UpdatedBy = "System";
        user.UpdatedDate = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return UserMapper.ToDto(user);
    }

    public async Task<bool> DeleteAsync(string userId)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u =>
                u.UserId == userId &&
                !u.IsDeleted);

        if (user == null)
            return false;

        user.IsDeleted = true;
        user.UpdatedBy = "System";
        user.UpdatedDate = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return true;
    }
}