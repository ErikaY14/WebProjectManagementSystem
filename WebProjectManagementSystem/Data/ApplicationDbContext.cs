using Microsoft.EntityFrameworkCore;
using WebProjectManagementSystem.Models;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Project> Projects { get; set; }
    public DbSet<WebProjectManagementSystem.Models.Task> Tasks { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<ProjectUser> ProjectUsers { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Attachment> Attachments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Seed данни за Projects
        modelBuilder.Entity<Project>().HasData(
            new Project { Id = 1, Name = "Project Alpha", Description = "First project", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, StartDate = DateTime.Now, EndDate = DateTime.Now.AddMonths(3), Status = "Active" },
            new Project { Id = 2, Name = "Project Beta", Description = "Second project", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, StartDate = DateTime.Now.AddMonths(1), EndDate = DateTime.Now.AddMonths(6), Status = "Active" }
        );

        // Seed данни за Users
        modelBuilder.Entity<User>().HasData(
            new User { Id = 1, Username = "eryordanova", PasswordHash = "123456", FirstName = "Erika", LastName = "Yordanova", Email = "eryordanova@panda.com", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, RoleType = "Admin" },
            new User { Id = 2, Username = "vyordanova", PasswordHash = "1234567", FirstName = "Viktoria", LastName = "Yordanova", Email = "vyordanova@panda.com", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, RoleType = "User" },
            new User { Id = 3, Username = "akehaiova", PasswordHash = "Adi123", FirstName = "Adelina", LastName = "Kehaiova", Email = "akehaiova@panda.com", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now, RoleType = "User" }
        );

        // Seed данни за Tasks
        modelBuilder.Entity<WebProjectManagementSystem.Models.Task>().HasData(
            new WebProjectManagementSystem.Models.Task { Id = 1, Title = "InitialTask", Description = "First task", ProjectId = 1, AssignedTo = 1, Status = "New", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now },
            new WebProjectManagementSystem.Models.Task { Id = 2, Title = "InitialTask", Description = "Second task", ProjectId = 2, AssignedTo = 2, Status = "In Progress", CreatedAt = DateTime.Now, UpdatedAt = DateTime.Now }
        );
    }
}
