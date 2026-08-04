using CompanySystem.Web.Authorization;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CompanySystem.Web.Controllers;

public class AuthPageController : Controller
{
    [HttpGet]
    [AllowAnonymous]
    [Route("Auth/Login")]
    public IActionResult Login()
    {
        return View("~/Views/Auth/Login.cshtml");
    }


    [HttpGet]
    [RequirePermission("Users.Create")]
    [Route("Auth/Register")]
    public IActionResult Register()
    {
        // Register.cshtml is not used; user creation is handled by User/Create
        return RedirectToAction("Create", "User");
    }


    [HttpGet]
    [AllowAnonymous]
    [Route("Auth/AccessDenied")]
    public IActionResult AccessDenied()
    {
        return View("~/Views/Auth/AccessDenied.cshtml");
    }
}