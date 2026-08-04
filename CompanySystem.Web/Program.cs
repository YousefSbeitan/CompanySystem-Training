using CompanySystem.Business.Extensions;
using CompanySystem.Business.Interfaces;
using CompanySystem.Business.Services;
using CompanySystem.Business.Services.Security;
using CompanySystem.Data.Context;
using CompanySystem.Data.Repositories.Implementations;
using CompanySystem.Data.Repositories.Interfaces;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddControllersWithViews();


// Register DbContext
builder.Services.AddDbContext<CompanySystemDbContext>(
    options =>
        options.UseSqlServer(
            builder.Configuration.GetConnectionString(
                "DefaultConnection")));


// Register Generic Repository
builder.Services.AddScoped(
    typeof(IGenericRepository<>),
    typeof(GenericRepository<>));


// Register Business Services
builder.Services.AddBusinessServices();

builder.Services.AddScoped<IAuthService, AuthService>();


// Register JWT Service
builder.Services.AddScoped<JwtService>();


// Composite Authentication: Cookie for MVC pages, JWT for API calls
builder.Services
    .AddAuthentication(
        options =>
        {
            options.DefaultAuthenticateScheme = "Composite";
            options.DefaultChallengeScheme = "Composite";
        })
    .AddPolicyScheme("Composite", "Composite",
        options =>
        {
            options.ForwardDefaultSelector = context =>
            {
                // API routes use JWT Bearer
                if (context.Request.Path.StartsWithSegments("/api"))
                    return JwtBearerDefaults.AuthenticationScheme;

                // MVC pages use Cookie
                return CookieAuthenticationDefaults.AuthenticationScheme;
            };
        })
    .AddCookie(
        options =>
        {
            options.LoginPath = "/Auth/Login";
            options.AccessDeniedPath = "/Auth/AccessDenied";
            options.Cookie.Name = "CompanySystem.Auth";
            options.Cookie.HttpOnly = true;
            options.Cookie.SameSite = SameSiteMode.Lax;
            options.ExpireTimeSpan = TimeSpan.FromHours(8);
            options.SlidingExpiration = true;
        })
    .AddJwtBearer(
        options =>
        {
            options.TokenValidationParameters =
                new TokenValidationParameters
                {
                    ValidateIssuer = true,

                    ValidateAudience = true,

                    ValidateLifetime = true,

                    ValidateIssuerSigningKey = true,


                    ValidIssuer =
                        builder.Configuration[
                            "Jwt:Issuer"],


                    ValidAudience =
                        builder.Configuration[
                            "Jwt:Audience"],


                    IssuerSigningKey =
                        new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(
                                builder.Configuration[
                                    "Jwt:Key"]!))
                };

            // Also accept JWT from cookie for MVC requests that carry it
            options.Events = new Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    var token = context.Request.Cookies["CompanySystem.Jwt"];
                    if (!string.IsNullOrEmpty(token))
                    {
                        context.Token = token;
                    }
                    return Task.CompletedTask;
                }
            };
        });

var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler(
        "/Home/Error");

    app.UseHsts();
}


app.UseHttpsRedirection();


app.UseStaticFiles();


app.UseRouting();


// Important order
app.UseAuthentication();

app.UseAuthorization();


app.MapControllerRoute(
    name: "default",
    pattern:
        "{controller=Home}/{action=Index}/{id?}");


app.Run();