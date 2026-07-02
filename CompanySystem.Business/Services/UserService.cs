using CompanySystem.Business.DTOs;
using CompanySystem.Business.Interfaces;
using CompanySystem.Business.Mappers;
using CompanySystem.Data.Entities;
using CompanySystem.Data.Repositories.Interfaces;
using CompanySystem.Shared.Exceptions;

namespace CompanySystem.Business.Services;

public class UserService : IUserService
{
    private readonly IGenericRepository<User> _repository;

    public UserService(IGenericRepository<User> repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<UserDto>> GetAllAsync()
    {
        try
        {
            var users = await _repository.FindAsync(u => !u.IsDeleted);

            return users.Select(UserMapper.ToDto);
        }
        catch (Exception ex)
        {
            throw new BusinessException("Failed to retrieve users.", ex);
        }
    }

    public async Task<UserDto?> GetByIdAsync(string userId)
    {
        try
        {
            var user = await _repository.FirstOrDefaultAsync(
                u => u.UserId == userId &&
                     !u.IsDeleted);

            if (user == null)
                throw new ResourceNotFoundException("User", userId);

            return UserMapper.ToDto(user);
        }
        catch (ResourceNotFoundException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new BusinessException("Failed to retrieve the user.", ex);
        }
    }

    public async Task<UserDto> CreateAsync(CreateUserDto dto)
    {
        try
        {
            var user = UserMapper.ToEntity(dto);

            user.CreatedBy = "System";

            await _repository.AddAsync(user);

            await _repository.SaveChangesAsync();

            return UserMapper.ToDto(user);
        }
        catch (Exception ex)
        {
            throw new BusinessException("Failed to create the user.", ex);
        }
    }

    public async Task<UserDto?> UpdateAsync(EditUserDto dto)
    {
        try
        {
            var user = await _repository.FirstOrDefaultAsync(
                u => u.UserId == dto.UserId &&
                     !u.IsDeleted);

            if (user == null)
                throw new ResourceNotFoundException("User", dto.UserId);

            UserMapper.UpdateEntity(user, dto);

            user.UpdatedBy = "System";
            user.UpdatedDate = DateTime.UtcNow;

            _repository.Update(user);

            await _repository.SaveChangesAsync();

            return UserMapper.ToDto(user);
        }
        catch (ResourceNotFoundException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new BusinessException("Failed to update the user.", ex);
        }
    }

    public async Task<bool> DeleteAsync(string userId)
    {
        try
        {
            var user = await _repository.FirstOrDefaultAsync(
                u => u.UserId == userId &&
                     !u.IsDeleted);

            if (user == null)
                throw new ResourceNotFoundException("User", userId);

            user.IsDeleted = true;
            user.UpdatedBy = "System";
            user.UpdatedDate = DateTime.UtcNow;

            _repository.Update(user);

            await _repository.SaveChangesAsync();

            return true;
        }
        catch (ResourceNotFoundException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new BusinessException("Failed to delete the user.", ex);
        }
    }
}