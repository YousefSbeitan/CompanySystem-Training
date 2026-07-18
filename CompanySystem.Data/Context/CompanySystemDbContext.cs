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

    public DbSet<Permission> Permissions { get; set; }

    public DbSet<UserPermission> UserPermissions { get; set; }


    protected override void OnModelCreating(
        ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);


        modelBuilder.Entity<User>()
            .Property(x => x.Salary)
            .HasPrecision(18, 2);


        modelBuilder.Entity<UserPermission>()
            .HasOne<Permission>()
            .WithMany()
            .HasForeignKey(x => x.PermissionId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<UserPermission>()
            .HasOne<User>()
            .WithMany()
            .HasForeignKey(x => x.UserId)
            .OnDelete(DeleteBehavior.Restrict);


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
                    "$2a$12$0abb8Tyz65y2IVbOZh8eReGMQNUXrk9L4.BbE5uNjWIp6dmXzml7q",

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


        modelBuilder.Entity<Permission>().HasData(
            new Permission
            {
                PermissionId = 1,
                PermissionName = "Departments.View",
                Description = "View departments",
                CreatedBy = "System",
                CreatedDate = new DateTime(2026, 1, 1),
                IsDeleted = false
            },
            new Permission
            {
                PermissionId = 2,
                PermissionName = "Departments.Create",
                Description = "Create departments",
                CreatedBy = "System",
                CreatedDate = new DateTime(2026, 1, 1),
                IsDeleted = false
            },
            new Permission
            {
                PermissionId = 3,
                PermissionName = "Departments.Edit",
                Description = "Edit departments",
                CreatedBy = "System",
                CreatedDate = new DateTime(2026, 1, 1),
                IsDeleted = false
            },
            new Permission
            {
                PermissionId = 4,
                PermissionName = "Departments.Delete",
                Description = "Delete departments",
                CreatedBy = "System",
                CreatedDate = new DateTime(2026, 1, 1),
                IsDeleted = false
            },
            new Permission
            {
                PermissionId = 5,
                PermissionName = "Users.View",
                Description = "View users",
                CreatedBy = "System",
                CreatedDate = new DateTime(2026, 1, 1),
                IsDeleted = false
            },
            new Permission
            {
                PermissionId = 6,
                PermissionName = "Users.Create",
                Description = "Create users",
                CreatedBy = "System",
                CreatedDate = new DateTime(2026, 1, 1),
                IsDeleted = false
            },
            new Permission
            {
                PermissionId = 7,
                PermissionName = "Users.Edit",
                Description = "Edit users",
                CreatedBy = "System",
                CreatedDate = new DateTime(2026, 1, 1),
                IsDeleted = false
            },
            new Permission
            {
                PermissionId = 8,
                PermissionName = "Users.Delete",
                Description = "Delete users",
                CreatedBy = "System",
                CreatedDate = new DateTime(2026, 1, 1),
                IsDeleted = false
            },
            new Permission
            {
                PermissionId = 9,
                PermissionName = "Roles.View",
                Description = "View roles",
                CreatedBy = "System",
                CreatedDate = new DateTime(2026, 1, 1),
                IsDeleted = false
            },
            new Permission
            {
                PermissionId = 10,
                PermissionName = "Roles.Create",
                Description = "Create roles",
                CreatedBy = "System",
                CreatedDate = new DateTime(2026, 1, 1),
                IsDeleted = false
            },
            new Permission
            {
                PermissionId = 11,
                PermissionName = "Roles.Edit",
                Description = "Edit roles",
                CreatedBy = "System",
                CreatedDate = new DateTime(2026, 1, 1),
                IsDeleted = false
            },
            new Permission
            {
                PermissionId = 12,
                PermissionName = "Roles.Delete",
                Description = "Delete roles",
                CreatedBy = "System",
                CreatedDate = new DateTime(2026, 1, 1),
                IsDeleted = false
            },
            new Permission
            {
                PermissionId = 13,
                PermissionName = "Notes.View",
                Description = "View notes",
                CreatedBy = "System",
                CreatedDate = new DateTime(2026, 1, 1),
                IsDeleted = false
            },
            new Permission
            {
                PermissionId = 14,
                PermissionName = "Notes.Create",
                Description = "Create notes",
                CreatedBy = "System",
                CreatedDate = new DateTime(2026, 1, 1),
                IsDeleted = false
            },
            new Permission
            {
                PermissionId = 15,
                PermissionName = "Notes.Edit",
                Description = "Edit notes",
                CreatedBy = "System",
                CreatedDate = new DateTime(2026, 1, 1),
                IsDeleted = false
            },
            new Permission
            {
                PermissionId = 16,
                PermissionName = "Notes.Delete",
                Description = "Delete notes",
                CreatedBy = "System",
                CreatedDate = new DateTime(2026, 1, 1),
                IsDeleted = false
            },
            new Permission
            {
                PermissionId = 17,
                PermissionName = "MainPageSections.View",
                Description = "View main page sections",
                CreatedBy = "System",
                CreatedDate = new DateTime(2026, 1, 1),
                IsDeleted = false
            },
            new Permission
            {
                PermissionId = 18,
                PermissionName = "MainPageSections.Create",
                Description = "Create main page sections",
                CreatedBy = "System",
                CreatedDate = new DateTime(2026, 1, 1),
                IsDeleted = false
            },
            new Permission
            {
                PermissionId = 19,
                PermissionName = "MainPageSections.Edit",
                Description = "Edit main page sections",
                CreatedBy = "System",
                CreatedDate = new DateTime(2026, 1, 1),
                IsDeleted = false
            },
            new Permission
            {
                PermissionId = 20,
                PermissionName = "MainPageSections.Delete",
                Description = "Delete main page sections",
                CreatedBy = "System",
                CreatedDate = new DateTime(2026, 1, 1),
                IsDeleted = false
            },
            new Permission
            {
                PermissionId = 21,
                PermissionName = "Permissions.View",
                Description = "View permissions",
                CreatedBy = "System",
                CreatedDate = new DateTime(2026, 1, 1),
                IsDeleted = false
            },
            new Permission
            {
                PermissionId = 22,
                PermissionName = "Permissions.Create",
                Description = "Create permissions",
                CreatedBy = "System",
                CreatedDate = new DateTime(2026, 1, 1),
                IsDeleted = false
            },
            new Permission
            {
                PermissionId = 23,
                PermissionName = "Permissions.Edit",
                Description = "Edit permissions",
                CreatedBy = "System",
                CreatedDate = new DateTime(2026, 1, 1),
                IsDeleted = false
            },
            new Permission
            {
                PermissionId = 24,
                PermissionName = "Permissions.Delete",
                Description = "Delete permissions",
                CreatedBy = "System",
                CreatedDate = new DateTime(2026, 1, 1),
                IsDeleted = false
            },
            new Permission
            {
                PermissionId = 25,
                PermissionName = "Dashboard.View",
                Description = "View dashboard",
                CreatedBy = "System",
                CreatedDate = new DateTime(2026, 1, 1),
                IsDeleted = false
            });


        // Admin user gets all permissions
        modelBuilder.Entity<UserPermission>().HasData(
            new UserPermission
            {
                UserPermissionId = 1,
                UserId = "ADMIN_001",
                PermissionId = 1,
                CreatedBy = "System",
                CreatedDate = new DateTime(2026, 1, 1),
                IsDeleted = false
            },
            new UserPermission
            {
                UserPermissionId = 2,
                UserId = "ADMIN_001",
                PermissionId = 2,
                CreatedBy = "System",
                CreatedDate = new DateTime(2026, 1, 1),
                IsDeleted = false
            },
            new UserPermission
            {
                UserPermissionId = 3,
                UserId = "ADMIN_001",
                PermissionId = 3,
                CreatedBy = "System",
                CreatedDate = new DateTime(2026, 1, 1),
                IsDeleted = false
            },
            new UserPermission
            {
                UserPermissionId = 4,
                UserId = "ADMIN_001",
                PermissionId = 4,
                CreatedBy = "System",
                CreatedDate = new DateTime(2026, 1, 1),
                IsDeleted = false
            },
            new UserPermission
            {
                UserPermissionId = 5,
                UserId = "ADMIN_001",
                PermissionId = 5,
                CreatedBy = "System",
                CreatedDate = new DateTime(2026, 1, 1),
                IsDeleted = false
            },
            new UserPermission
            {
                UserPermissionId = 6,
                UserId = "ADMIN_001",
                PermissionId = 6,
                CreatedBy = "System",
                CreatedDate = new DateTime(2026, 1, 1),
                IsDeleted = false
            },
            new UserPermission
            {
                UserPermissionId = 7,
                UserId = "ADMIN_001",
                PermissionId = 7,
                CreatedBy = "System",
                CreatedDate = new DateTime(2026, 1, 1),
                IsDeleted = false
            },
            new UserPermission
            {
                UserPermissionId = 8,
                UserId = "ADMIN_001",
                PermissionId = 8,
                CreatedBy = "System",
                CreatedDate = new DateTime(2026, 1, 1),
                IsDeleted = false
            },
            new UserPermission
            {
                UserPermissionId = 9,
                UserId = "ADMIN_001",
                PermissionId = 9,
                CreatedBy = "System",
                CreatedDate = new DateTime(2026, 1, 1),
                IsDeleted = false
            },
            new UserPermission
            {
                UserPermissionId = 10,
                UserId = "ADMIN_001",
                PermissionId = 10,
                CreatedBy = "System",
                CreatedDate = new DateTime(2026, 1, 1),
                IsDeleted = false
            },
            new UserPermission
            {
                UserPermissionId = 11,
                UserId = "ADMIN_001",
                PermissionId = 11,
                CreatedBy = "System",
                CreatedDate = new DateTime(2026, 1, 1),
                IsDeleted = false
            },
            new UserPermission
            {
                UserPermissionId = 12,
                UserId = "ADMIN_001",
                PermissionId = 12,
                CreatedBy = "System",
                CreatedDate = new DateTime(2026, 1, 1),
                IsDeleted = false
            },
            new UserPermission
            {
                UserPermissionId = 13,
                UserId = "ADMIN_001",
                PermissionId = 13,
                CreatedBy = "System",
                CreatedDate = new DateTime(2026, 1, 1),
                IsDeleted = false
            },
            new UserPermission
            {
                UserPermissionId = 14,
                UserId = "ADMIN_001",
                PermissionId = 14,
                CreatedBy = "System",
                CreatedDate = new DateTime(2026, 1, 1),
                IsDeleted = false
            },
            new UserPermission
            {
                UserPermissionId = 15,
                UserId = "ADMIN_001",
                PermissionId = 15,
                CreatedBy = "System",
                CreatedDate = new DateTime(2026, 1, 1),
                IsDeleted = false
            },
            new UserPermission
            {
                UserPermissionId = 16,
                UserId = "ADMIN_001",
                PermissionId = 16,
                CreatedBy = "System",
                CreatedDate = new DateTime(2026, 1, 1),
                IsDeleted = false
            },
            new UserPermission
            {
                UserPermissionId = 17,
                UserId = "ADMIN_001",
                PermissionId = 17,
                CreatedBy = "System",
                CreatedDate = new DateTime(2026, 1, 1),
                IsDeleted = false
            },
            new UserPermission
            {
                UserPermissionId = 18,
                UserId = "ADMIN_001",
                PermissionId = 18,
                CreatedBy = "System",
                CreatedDate = new DateTime(2026, 1, 1),
                IsDeleted = false
            },
            new UserPermission
            {
                UserPermissionId = 19,
                UserId = "ADMIN_001",
                PermissionId = 19,
                CreatedBy = "System",
                CreatedDate = new DateTime(2026, 1, 1),
                IsDeleted = false
            },
            new UserPermission
            {
                UserPermissionId = 20,
                UserId = "ADMIN_001",
                PermissionId = 20,
                CreatedBy = "System",
                CreatedDate = new DateTime(2026, 1, 1),
                IsDeleted = false
            },
            new UserPermission
            {
                UserPermissionId = 21,
                UserId = "ADMIN_001",
                PermissionId = 21,
                CreatedBy = "System",
                CreatedDate = new DateTime(2026, 1, 1),
                IsDeleted = false
            },
            new UserPermission
            {
                UserPermissionId = 22,
                UserId = "ADMIN_001",
                PermissionId = 22,
                CreatedBy = "System",
                CreatedDate = new DateTime(2026, 1, 1),
                IsDeleted = false
            },
            new UserPermission
            {
                UserPermissionId = 23,
                UserId = "ADMIN_001",
                PermissionId = 23,
                CreatedBy = "System",
                CreatedDate = new DateTime(2026, 1, 1),
                IsDeleted = false
            },
            new UserPermission
            {
                UserPermissionId = 24,
                UserId = "ADMIN_001",
                PermissionId = 24,
                CreatedBy = "System",
                CreatedDate = new DateTime(2026, 1, 1),
                IsDeleted = false
            },
            new UserPermission
            {
                UserPermissionId = 25,
                UserId = "ADMIN_001",
                PermissionId = 25,
                CreatedBy = "System",
                CreatedDate = new DateTime(2026, 1, 1),
                IsDeleted = false
            });
    }
}