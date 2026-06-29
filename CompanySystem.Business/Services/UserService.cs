using CompanySystem.Business.DTOs;
using CompanySystem.Business.Interfaces;
using CompanySystem.Business.Mappers;
using CompanySystem.Data.Entities;
using CompanySystem.Data.Repositories.Interfaces;

namespace CompanySystem.Business.Services;

public class UserService : IUserService
{
    private readonly IGenericRepository<User> _repository;

    public UserService(
        IGenericRepository<User> repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<UserDto>> GetAllAsync()
    {
        var users = await _repository.FindAsync(
            u => !u.IsDeleted);

        return users.Select(UserMapper.ToDto);
    }

    public async Task<UserDto?> GetByIdAsync(string userId)
    {
        var user = await _repository.FirstOrDefaultAsync(
            u => u.UserId == userId &&
                 !u.IsDeleted);

        if (user == null)
            return null;

        return UserMapper.ToDto(user);
    }

    public async Task<UserDto> CreateAsync(CreateUserDto dto)
    {
        var user = UserMapper.ToEntity(dto);

        user.CreatedBy = "System";

        await _repository.AddAsync(user);

        await _repository.SaveChangesAsync();

        return UserMapper.ToDto(user);
    }

    public async Task<UserDto?> UpdateAsync(EditUserDto dto)
    {
        var user = await _repository.FirstOrDefaultAsync(
            u => u.UserId == dto.UserId &&
                 !u.IsDeleted);

        if (user == null)
            return null;

        UserMapper.UpdateEntity(user, dto);

        user.UpdatedBy = "System";
        user.UpdatedDate = DateTime.UtcNow;

        _repository.Update(user);

        await _repository.SaveChangesAsync();

        return UserMapper.ToDto(user);
    }

    public async Task<bool> DeleteAsync(string userId)
    {
        var user = await _repository.FirstOrDefaultAsync(
            u => u.UserId == userId &&
                 !u.IsDeleted);

        if (user == null)
            return false;

        user.IsDeleted = true;
        user.UpdatedBy = "System";
        user.UpdatedDate = DateTime.UtcNow;

        _repository.Update(user);

        await _repository.SaveChangesAsync();

        return true;
    }
}