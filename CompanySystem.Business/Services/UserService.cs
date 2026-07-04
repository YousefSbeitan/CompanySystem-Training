using CompanySystem.Business.DTOs;
using CompanySystem.Business.Interfaces;
using CompanySystem.Business.Mappers;
using CompanySystem.Data.Entities;
using CompanySystem.Data.Repositories.Interfaces;
using CompanySystem.Shared.Exceptions;
using CompanySystem.Shared.Helpers;

namespace CompanySystem.Business.Services;

public class UserService : IUserService
{
    private readonly IGenericRepository<User> _userRepository;
    private readonly IGenericRepository<Department> _departmentRepository;

    public UserService(
        IGenericRepository<User> userRepository,
        IGenericRepository<Department> departmentRepository)
    {
        _userRepository = userRepository;
        _departmentRepository = departmentRepository;
    }


    public async Task<IEnumerable<UserDto>> GetAllAsync()
    {
        try
        {
            var users = await _userRepository.FindAsync(
                u => !u.IsDeleted);

            return users.Select(UserMapper.ToDto);
        }
        catch (Exception ex)
        {
            throw new BusinessException(
                "Failed to retrieve users.", ex);
        }
    }


    public async Task<UserDto> GetByIdAsync(string userId)
    {
        try
        {
            var user = await _userRepository.FirstOrDefaultAsync(
                u => u.UserId == userId &&
                     !u.IsDeleted);

            if (user == null)
                throw new ResourceNotFoundException(
                    "User", userId);

            return UserMapper.ToDto(user);
        }
        catch (ResourceNotFoundException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new BusinessException(
                "Failed to retrieve the user.", ex);
        }
    }


    public async Task<UserDto> CreateAsync(CreateUserDto dto)
    {
        try
        {
            var department =
                await _departmentRepository.FirstOrDefaultAsync(
                    d => d.DepartmentId == dto.DepartmentId &&
                         !d.IsDeleted);

            if (department == null)
                throw new ResourceNotFoundException(
                    "Department",
                    dto.DepartmentId!);


            var user = UserMapper.ToEntity(dto);


            // Generate UserId
            user.UserId =
                UserIdGenerator.Generate(dto.DepartmentId);


            // If this user is department leader
            if (dto.IsLeader)
            {
                user.LeaderId = user.UserId;

                department.ManagerId = user.UserId;

                _departmentRepository.Update(department);
            }
            else
            {
                if (string.IsNullOrEmpty(department.ManagerId))
                    throw new BusinessException(
                        "This department does not have a leader yet.");

                user.LeaderId = department.ManagerId;
            }


            user.CreatedBy = "System";


            await _userRepository.AddAsync(user);

            await _userRepository.SaveChangesAsync();

            return UserMapper.ToDto(user);
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
                "Failed to create the user.", ex);
        }
    }


    public async Task<UserDto> UpdateAsync(EditUserDto dto)
    {
        try
        {
            var user =
                await _userRepository.FirstOrDefaultAsync(
                    u => u.UserId == dto.UserId &&
                         !u.IsDeleted);

            if (user == null)
                throw new ResourceNotFoundException(
                    "User", dto.UserId);


            UserMapper.UpdateEntity(user, dto);


            if (dto.IsLeader)
            {
                user.LeaderId = user.UserId;
            }
            else
            {
                var department =
                    await _departmentRepository.FirstOrDefaultAsync(
                        d => d.DepartmentId == dto.DepartmentId &&
                             !d.IsDeleted);

                if (department == null)
                    throw new ResourceNotFoundException(
                        "Department",
                        dto.DepartmentId!);

                if (string.IsNullOrEmpty(department.ManagerId))
                    throw new BusinessException(
                        "This department does not have a leader yet.");

                user.LeaderId = department.ManagerId;
            }


            user.UpdatedBy = "System";
            user.UpdatedDate = DateTime.UtcNow;


            _userRepository.Update(user);

            await _userRepository.SaveChangesAsync();


            return UserMapper.ToDto(user);
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
                "Failed to update the user.", ex);
        }
    }


    public async Task<bool> DeleteAsync(string userId)
    {
        try
        {
            var user =
                await _userRepository.FirstOrDefaultAsync(
                    u => u.UserId == userId &&
                         !u.IsDeleted);

            if (user == null)
                throw new ResourceNotFoundException(
                    "User", userId);


            user.IsDeleted = true;
            user.UpdatedBy = "System";
            user.UpdatedDate = DateTime.UtcNow;


            _userRepository.Update(user);

            await _userRepository.SaveChangesAsync();


            return true;
        }
        catch (ResourceNotFoundException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new BusinessException(
                "Failed to delete the user.", ex);
        }
    }
}