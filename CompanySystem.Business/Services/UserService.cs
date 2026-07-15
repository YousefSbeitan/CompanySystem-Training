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

    public async Task<PagedResponse<UserDto>> GetAllAsync(PaginationFilterRequest request)
    {
        try
        {
            var users = await _userRepository.FindAsync(u => !u.IsDeleted);

            if (!string.IsNullOrWhiteSpace(request.Search))
            {
                var search = request.Search.Trim().ToLower();

                users = users.Where(u =>
                    u.UserId.ToLower().Contains(search) ||
                    u.Username.ToLower().Contains(search) ||
                    u.PhoneNumber.Contains(search));
            }

            users = request.SortBy?.ToLower() switch
            {
                "username" => request.IsDescending
                    ? users.OrderByDescending(u => u.Username)
                    : users.OrderBy(u => u.Username),

                "salary" => request.IsDescending
                    ? users.OrderByDescending(u => u.Salary)
                    : users.OrderBy(u => u.Salary),

                "startdate" => request.IsDescending
                    ? users.OrderByDescending(u => u.StartDate)
                    : users.OrderBy(u => u.StartDate),

                "createddate" => request.IsDescending
                    ? users.OrderByDescending(u => u.CreatedDate)
                    : users.OrderBy(u => u.CreatedDate),

                _ => users.OrderBy(u => u.UserId)
            };

            var totalRecords = users.Count();

            var pagedUsers = users
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(UserMapper.ToDto)
                .ToList();

            return new PagedResponse<UserDto>(pagedUsers, request.PageNumber, request.PageSize, totalRecords);
        }
        catch (Exception ex)
        {
            throw new BusinessException("Failed to retrieve users.", ex);
        }
    }

    public async Task<UserDto> GetByIdAsync(string userId)
    {
        if (string.IsNullOrWhiteSpace(userId))
            throw new BusinessException("User id is required.");

        try
        {
            var user = await GetUserAsync(userId);

            return UserMapper.ToDto(user);
        }
        catch (Exception ex) when (ex is not BusinessException && ex is not ResourceNotFoundException)
        {
            throw new BusinessException(
                "Failed to retrieve the user.", ex);
        }
    }

    public async Task<UserDto> CreateAsync(CreateUserDto dto)
    {
        if (dto == null)
            throw new BusinessException("User data is required.");

        ValidateUserData(dto.Username,dto.PhoneNumber,dto.DepartmentId,dto.Salary);

        if (string.IsNullOrWhiteSpace(dto.PasswordHash))
            throw new BusinessException("Password is required.");

        

        dto.Username = dto.Username.Trim();
        dto.PhoneNumber = dto.PhoneNumber.Trim();

        try
        {
            await EnsureUserIsUniqueAsync(dto.Username,dto.PhoneNumber);

            var department = await GetDepartmentAsync(dto.DepartmentId!.Value);

            var user = UserMapper.ToEntity(dto);

            user.UserId = UserIdGenerator.Generate(dto.DepartmentId);

            if (dto.IsLeader)
            {
                user.LeaderId = user.UserId;

                department.ManagerId = user.UserId;

                _departmentRepository.Update(department);
            }
            else
            {
                if (string.IsNullOrWhiteSpace(department.ManagerId))
                {
                    throw new BusinessException("This department does not have a leader yet.");
                }

                user.LeaderId = department.ManagerId;
            }

            user.CreatedBy = "System";

            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();

            return UserMapper.ToDto(user);
        }
        catch (Exception ex) when (ex is not BusinessException && ex is not ResourceNotFoundException)
        {
            throw new BusinessException("Failed to create the user.", ex);
        }
    }

    public async Task<UserDto> UpdateAsync(EditUserDto dto)
    {
        if (dto == null)
            throw new BusinessException("User data is required.");

        if (string.IsNullOrWhiteSpace(dto.UserId))
            throw new BusinessException("User id is required.");

        ValidateUserData(dto.Username, dto.PhoneNumber, dto.DepartmentId, dto.Salary);

        dto.Username = dto.Username.Trim();
        dto.PhoneNumber = dto.PhoneNumber.Trim();

        try
        {
            var user = await GetUserAsync(dto.UserId);

            await EnsureUserIsUniqueAsync(dto.Username,dto.PhoneNumber,dto.UserId);

            UserMapper.UpdateEntity(user, dto);

            if (dto.IsLeader)
            {
                user.LeaderId = user.UserId;
            }
            else
            {
                var department = await GetDepartmentAsync(dto.DepartmentId!.Value);

                if (string.IsNullOrWhiteSpace(department.ManagerId))
                    throw new BusinessException("This department does not have a leader yet.");

                user.LeaderId = department.ManagerId;
            }

            user.UpdatedBy = "System";
            user.UpdatedDate = DateTime.UtcNow;

            _userRepository.Update(user);

            await _userRepository.SaveChangesAsync();

            return UserMapper.ToDto(user);
        }
        catch (Exception ex) when (ex is not BusinessException && ex is not ResourceNotFoundException)
        {
            throw new BusinessException("Failed to update the user.",ex);
        }
    }

    public async Task<bool> DeleteAsync(string userId)
    {
        if (string.IsNullOrWhiteSpace(userId))
            throw new BusinessException("User id is required.");

        try
        {
            var user = await GetUserAsync(userId);

            user.IsDeleted = true;
            user.UpdatedBy = "System";
            user.UpdatedDate = DateTime.UtcNow;

            _userRepository.Update(user);

            await _userRepository.SaveChangesAsync();

            return true;
        }
        catch (Exception ex) when (ex is not BusinessException && ex is not ResourceNotFoundException)
        {
            throw new BusinessException("Failed to delete the user.",ex);
        }
    }

    private async Task EnsureUserIsUniqueAsync(string username,string phoneNumber,string? excludedUserId = null)
    {
        var existingUser =await _userRepository.FirstOrDefaultAsync(u =>!u.IsDeleted &&
                    (excludedUserId == null || u.UserId != excludedUserId) &&
                    (u.Username.ToLower() == username.ToLower() || u.PhoneNumber == phoneNumber));

        if (existingUser != null)
        {
            throw new BusinessException("Username or phone number already exists.");
        }
    }

    private async Task<Department> GetDepartmentAsync(
    int departmentId)
    {
        var department = await _departmentRepository.FirstOrDefaultAsync(d => d.DepartmentId == departmentId &&!d.IsDeleted);

        if (department == null)
        {
            throw new ResourceNotFoundException("Department",departmentId);
        }

        return department;
    }

    private async Task<User> GetUserAsync(string userId)
    {
        var user = await _userRepository.FirstOrDefaultAsync(u => u.UserId == userId &&!u.IsDeleted);

        if (user == null)
            throw new ResourceNotFoundException("User",userId);

        return user;
    }

    private static void ValidateUserData(string username,string phoneNumber,int? departmentId,decimal salary)
    {
        if (string.IsNullOrWhiteSpace(username))
            throw new BusinessException("Username is required.");

        if (string.IsNullOrWhiteSpace(phoneNumber))
            throw new BusinessException("Phone number is required.");

        if (departmentId == null || departmentId <= 0)
            throw new BusinessException("Department is required.");

        if (salary < 0)
            throw new BusinessException("Salary cannot be negative.");
    }
}