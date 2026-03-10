using Microsoft.EntityFrameworkCore;
using TaskManagement.Domain.Entities;

namespace TaskManagement.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<TaskEntity> Tasks => Set<TaskEntity>();
    public DbSet<ProcurementTaskData> ProcurementTaskData => Set<ProcurementTaskData>();
    public DbSet<DevelopmentTaskData> DevelopmentTaskData => Set<DevelopmentTaskData>();
    public DbSet<TaskStatusHistory> TaskStatusHistories => Set<TaskStatusHistory>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(e =>
        {
            e.ToTable("Users");
            e.HasKey(u => u.Id);
            e.Property(u => u.Name).IsRequired().HasMaxLength(200);
            e.Property(u => u.Email).IsRequired().HasMaxLength(200);
            e.HasIndex(u => u.Email).IsUnique();
        });

        modelBuilder.Entity<TaskEntity>(e =>
        {
            e.ToTable("Tasks");
            e.HasKey(t => t.Id);

            e.Property(t => t.Type).IsRequired();
            e.Property(t => t.Status).IsRequired();
            e.Property(t => t.IsClosed).IsRequired();
            e.Property(t => t.CreatedAt).IsRequired();

            e.HasOne(t => t.AssignedUser)
             .WithMany(u => u.AssignedTasks)
             .HasForeignKey(t => t.AssignedUserId)
             .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<ProcurementTaskData>(e =>
        {
            e.ToTable("ProcurementTaskData");
            e.HasKey(d => d.TaskId);

            e.HasOne(d => d.Task)
             .WithOne(t => t.ProcurementData)
             .HasForeignKey<ProcurementTaskData>(d => d.TaskId)
             .OnDelete(DeleteBehavior.Cascade);

            e.Property(d => d.PriceQuote1).HasMaxLength(500);
            e.Property(d => d.PriceQuote2).HasMaxLength(500);
            e.Property(d => d.Receipt).HasMaxLength(500);
        });

        modelBuilder.Entity<DevelopmentTaskData>(e =>
        {
            e.ToTable("DevelopmentTaskData");
            e.HasKey(d => d.TaskId);

            e.HasOne(d => d.Task)
             .WithOne(t => t.DevelopmentData)
             .HasForeignKey<DevelopmentTaskData>(d => d.TaskId)
             .OnDelete(DeleteBehavior.Cascade);

            e.Property(d => d.SpecificationText).HasMaxLength(4000);
            e.Property(d => d.BranchName).HasMaxLength(200);
            e.Property(d => d.VersionNumber).HasMaxLength(50);
        });

        modelBuilder.Entity<TaskStatusHistory>(e =>
        {
            e.ToTable("TaskStatusHistory");
            e.HasKey(h => h.Id);

            e.HasOne(h => h.Task)
             .WithMany(t => t.StatusHistory)
             .HasForeignKey(h => h.TaskId)
             .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(h => h.AssignedUser)
             .WithMany()
             .HasForeignKey(h => h.AssignedUserId)
             .OnDelete(DeleteBehavior.Restrict);

            e.Property(h => h.ChangedAt).IsRequired();
        });
    }
}
