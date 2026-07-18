using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CompanySystem.Web.Authorization;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class RequirePermissionAttribute : Attribute, IAuthorizationFilter
{
    private readonly string _permissionName;

    public RequirePermissionAttribute(string permissionName)
    {
        _permissionName = permissionName;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var user = context.HttpContext.User;

        if (!user.Identity?.IsAuthenticated ?? true)
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        var hasPermission = user.HasClaim("Permission", _permissionName);

        if (!hasPermission)
        {
            context.Result = new ForbidResult();
        }
    }
}
