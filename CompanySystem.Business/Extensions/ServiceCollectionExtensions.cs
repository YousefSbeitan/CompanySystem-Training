using CompanySystem.Business.Interfaces;
using CompanySystem.Business.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CompanySystem.Business.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddBusinessServices(
        this IServiceCollection services)
    {
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IDepartmentService, DepartmentService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<INoteService, NoteService>();
        services.AddScoped<IMainPageSectionService, MainPageSectionService>();

        services.AddScoped<IPermissionService, PermissionService>();

        return services;
    }
}