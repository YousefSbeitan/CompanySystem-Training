using CompanySystem.Data.Entities;
using CompanySystem.Data.Entities.Enums;
using Microsoft.EntityFrameworkCore;

namespace CompanySystem.Data.Context;

public class CompanySystemDbContext : DbContext
{
    public CompanySystemDbContext(
        DbContextOptions<CompanySystemDbContext> options)
        : base(options)
    {
    }


    public DbSet<User> Users { get; set; }

    public DbSet<Role> Roles { get; set; }

    public DbSet<Department> Departments { get; set; }

    public DbSet<Note> Notes { get; set; }

    public DbSet<MainPageSection> MainPageSections { get; set; }

    public DbSet<RefreshToken> RefreshTokens { get; set; }


    protected override void OnModelCreating(
        ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);


        modelBuilder.Entity<User>()
            .Property(x => x.Salary)
            .HasPrecision(18, 2);


        SeedData(modelBuilder);
    }


    private void SeedData(
        ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Role>().HasData(
            new Role
            {
                RoleId = 1,
                RoleName = "Admin",
                CreatedBy = "System",
                CreatedDate = new DateTime(2026, 1, 1),
                IsDeleted = false
            },
            new Role
            {
                RoleId = 2,
                RoleName = "Manager",
                CreatedBy = "System",
                CreatedDate = new DateTime(2026, 1, 1),
                IsDeleted = false
            },
            new Role
            {
                RoleId = 3,
                RoleName = "Employee",
                CreatedBy = "System",
                CreatedDate = new DateTime(2026, 1, 1),
                IsDeleted = false
            });


        modelBuilder.Entity<Department>().HasData(
            new Department
            {
                DepartmentId = 1,
                DepartmentName = "Human Resources",
                ManagerId = "ADMIN_001",
                CreatedBy = "System",
                CreatedDate = new DateTime(2026, 1, 1),
                IsDeleted = false
            },
            new Department
            {
                DepartmentId = 2,
                DepartmentName = "Information Technology",
                ManagerId = "ADMIN_001",
                CreatedBy = "System",
                CreatedDate = new DateTime(2026, 1, 1),
                IsDeleted = false
            });


        modelBuilder.Entity<User>().HasData(
            new User
            {
                UserId = "ADMIN_001",

                Username = "admin",

                // Password: Admin@123
                PasswordHash =
                    "$2a$12$rNiQUYl1Ap4ZrS/IuZeJOuXkio2fo8esv298m6LzCqr32AgTJbHq2",

                RoleId = 1,

                LeaderId = "ADMIN_001",

                DepartmentId = 1,

                PhoneNumber = "0000000000",

                StartDate =
                    new DateTime(
                        2026,
                        1,
                        1),

                Salary = 0,

                IsActive = true,

                CreatedBy = "System",

                CreatedDate =
                    new DateTime(
                        2026,
                        1,
                        1),

                IsDeleted = false
            });


        modelBuilder.Entity<MainPageSection>().HasData(
            new MainPageSection
            {
                SectionId = 1,
                SectionType = SectionType.AboutUs,
                Title = "About Us",
                Content = "Welcome to Company System.",
                CreatedBy = "System",
                CreatedDate = new DateTime(2026, 1, 1),
                IsDeleted = false
            },
            new MainPageSection
            {
                SectionId = 2,
                SectionType = SectionType.Services,
                Title = "Services",
                Content = "Our professional services.",
                CreatedBy = "System",
                CreatedDate = new DateTime(2026, 1, 1),
                IsDeleted = false
            });
    }
}