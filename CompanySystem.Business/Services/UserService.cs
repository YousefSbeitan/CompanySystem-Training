using CompanySystem.Business.DTOs;
using CompanySystem.Business.Interfaces;
using CompanySystem.Business.Mappers;
using CompanySystem.Data.Entities;
using CompanySystem.Data.Repositories.Interfaces;
using CompanySystem.Shared.Exceptions;
using CompanySystem.Shared.Helpers;
using CompanySystem.Shared.Requests;
using CompanySystem.Shared.Responses;

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


    public async Task<PagedResponse<UserDto>> GetAllAsync(
    PaginationFilterRequest request,
    string currentUserId,
    string currentUserRole)
    {
        try
        {
            var users =
                await _userRepository.FindAsync(
                    u => !u.IsDeleted);


            // Manager can see himself + his employees only
            if (currentUserRole == "Manager")
            {
                users =
                    users.Where(
                        u =>
                        u.UserId == currentUserId
                        ||
                        u.LeaderId == currentUserId);
            }


            if (!string.IsNullOrWhiteSpace(
                    request.Search))
            {
                users =
                    users.Where(
                        u =>
                        u.UserId.ToLower()
                            .Contains(
                                request.Search.ToLower())
                        ||
                        u.Username.ToLower()
                            .Contains(
                                request.Search.ToLower())
                        ||
                        u.PhoneNumber.Contains(
                            request.Search));
            }


            users =
                request.SortBy?.ToLower() switch
                {
                    "username" =>
                        request.IsDescending
                            ? users.OrderByDescending(
                                u => u.Username)
                            : users.OrderBy(
                                u => u.Username),


                    "salary" =>
                        request.IsDescending
                            ? users.OrderByDescending(
                                u => u.Salary)
                            : users.OrderBy(
                                u => u.Salary),


                    "startdate" =>
                        request.IsDescending
                            ? users.OrderByDescending(
                                u => u.StartDate)
                            : users.OrderBy(
                                u => u.StartDate),


                    "createddate" =>
                        request.IsDescending
                            ? users.OrderByDescending(
                                u => u.CreatedDate)
                            : users.OrderBy(
                                u => u.CreatedDate),


                    _ =>
                        users.OrderBy(
                            u => u.UserId)
                };


            var totalRecords =
                users.Count();


            var pagedUsers =
                users
                .Skip(
                    (request.PageNumber - 1)
                    *
                    request.PageSize)
                .Take(
                    request.PageSize)
                .Select(
                    UserMapper.ToDto)
                .ToList();


            return new PagedResponse<UserDto>(
                pagedUsers,
                request.PageNumber,
                request.PageSize,
                totalRecords);
        }
        catch (Exception ex)
        {
            throw new BusinessException(
                "Failed to retrieve users.",
                ex);
        }
    }


    public async Task<UserDto> GetByIdAsync(string userId)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(userId))
                throw new BusinessException(
                    "User id is required.");

            var user =
                await _userRepository.FirstOrDefaultAsync(
                    u => u.UserId == userId &&
                         !u.IsDeleted);


            if (user == null)
                throw new ResourceNotFoundException(
                    "User",
                    userId);



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
                "Failed to retrieve the user.", ex);
        }
    }


    public async Task<UserDto> CreateAsync(CreateUserDto dto)
    {
        try
        {
            if (dto == null)
                throw new BusinessException(
                    "User data is required.");


            if (string.IsNullOrWhiteSpace(dto.Username))
                throw new BusinessException(
                    "Username is required.");


            if (string.IsNullOrWhiteSpace(dto.Password))
                throw new BusinessException(
                    "Password is required.");


            if (string.IsNullOrWhiteSpace(dto.PhoneNumber))
                throw new BusinessException(
                    "Phone number is required.");


            if (dto.DepartmentId == null ||
                dto.DepartmentId <= 0)
                throw new BusinessException(
                    "Department is required.");


            if (dto.Salary < 0)
                throw new BusinessException(
                    "Salary cannot be negative.");


            dto.Username = dto.Username.Trim();
            dto.PhoneNumber = dto.PhoneNumber.Trim();


            var existingUser =
                await _userRepository.FirstOrDefaultAsync(
                    u =>
                    (
                        u.Username.ToLower()
                        == dto.Username.ToLower()
                        ||
                        u.PhoneNumber == dto.PhoneNumber

                    )
                    &&
                    !u.IsDeleted);


            if (existingUser != null)
                throw new BusinessException(
                    "Username or phone number already exists.");


            var department =
                await _departmentRepository.FirstOrDefaultAsync(
                    d => d.DepartmentId == dto.DepartmentId &&
                         !d.IsDeleted);


            if (department == null)
                throw new ResourceNotFoundException(
                    "Department",
                    dto.DepartmentId);


            var user =
                UserMapper.ToEntity(dto);

            user.PasswordHash =
                PasswordHasher.HashPassword(
                    dto.Password);

            user.UserId =
                UserIdGenerator.Generate(
                    dto.DepartmentId);


            if (dto.IsLeader)
            {
                user.LeaderId =
                    user.UserId;


                department.ManagerId =
                    user.UserId;


                _departmentRepository.Update(
                    department);
            }
            else
            {
                if (string.IsNullOrEmpty(
                    department.ManagerId))
                {

                    throw new BusinessException(
                        "This department does not have a leader yet.");
                }


                user.LeaderId =
                    department.ManagerId;
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


    public async Task<UserDto> UpdateAsync(
        EditUserDto dto)
    {
        try
        {
            if (dto == null)
                throw new BusinessException(
                    "User data is required.");


            if (string.IsNullOrWhiteSpace(dto.UserId))
                throw new BusinessException(
                    "User id is required.");


            if (string.IsNullOrWhiteSpace(dto.Username))
                throw new BusinessException(
                    "Username is required.");


            if (string.IsNullOrWhiteSpace(dto.PhoneNumber))
                throw new BusinessException(
                    "Phone number is required.");


            dto.Username = dto.Username.Trim();
            dto.PhoneNumber = dto.PhoneNumber.Trim();


            var user =
                await _userRepository.FirstOrDefaultAsync(
                    u => u.UserId == dto.UserId &&
                         !u.IsDeleted);


            if (user == null)
                throw new ResourceNotFoundException(
                    "User",
                    dto.UserId);


            var duplicateUser =
                await _userRepository.FirstOrDefaultAsync(
                    u =>
                    u.UserId != dto.UserId
                    &&
                    (
                        u.Username.ToLower()
                        == dto.Username.ToLower()
                        ||
                        u.PhoneNumber == dto.PhoneNumber
                    )
                    &&
                    !u.IsDeleted);


            if (duplicateUser != null)
                throw new BusinessException(
                    "Username or phone number already exists.");


            UserMapper.UpdateEntity(
                user,
                dto);


            if (dto.IsLeader)
            {
                user.LeaderId =
                    user.UserId;
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
                        dto.DepartmentId);


                if (string.IsNullOrEmpty(
                    department.ManagerId))
                    throw new BusinessException(
                        "This department does not have a leader yet.");

                user.LeaderId =
                    department.ManagerId;
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


    public async Task<bool> DeleteAsync(
        string userId)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(userId))
                throw new BusinessException(
                    "User id is required.");


            var user =
                await _userRepository.FirstOrDefaultAsync(
                    u => u.UserId == userId && !u.IsDeleted);


            if (user == null)
                throw new ResourceNotFoundException("User", userId);


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
        catch (BusinessException)
        {
            throw;
        }
        catch (Exception ex)
        {
            throw new BusinessException(
                "Failed to delete the user.", ex);
        }
    }

    public async Task<bool> CanManageUserAsync(
    string managerId,
    string userId)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(managerId) ||
                string.IsNullOrWhiteSpace(userId))
            {
                return false;
            }


            var user =
                await _userRepository.FirstOrDefaultAsync(
                    u => u.UserId == userId &&
                         !u.IsDeleted);


            if (user == null)
                return false;


            return user.LeaderId == managerId;
        }
        catch (Exception ex)
        {
            throw new BusinessException(
                "Failed to check user permission.",
                ex);
        }
    }
}