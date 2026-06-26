using CompanySystem.Data.Entities;
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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>()
    .Property(x => x.Salary)
    .HasPrecision(18, 2);
        base.OnModelCreating(modelBuilder);
    }

}