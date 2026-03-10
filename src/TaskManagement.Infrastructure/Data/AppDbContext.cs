using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using TaskManagement.Domain.Entities;

namespace TaskManagement.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<TaskEntity> Tasks => Set<TaskEntity>();
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

            e.Property(t => t.TypeData)
             .HasConversion(
                 v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                 v => JsonSerializer.Deserialize<Dictionary<string, string>>(v, (JsonSerializerOptions?)null)
                     ?? new Dictionary<string, string>())
             .HasColumnType("nvarchar(max)")
             .Metadata.SetValueComparer(new ValueComparer<Dictionary<string, string>>(
                 (a, b) => JsonSerializer.Serialize(a, (JsonSerializerOptions?)null)
                        == JsonSerializer.Serialize(b, (JsonSerializerOptions?)null),
                 v => v.Aggregate(0, (hash, kvp) => HashCode.Combine(hash, kvp.Key.GetHashCode(), kvp.Value.GetHashCode())),
                 v => v.ToDictionary(kvp => kvp.Key, kvp => kvp.Value)));

            e.HasOne(t => t.AssignedUser)
             .WithMany(u => u.AssignedTasks)
             .HasForeignKey(t => t.AssignedUserId)
             .OnDelete(DeleteBehavior.Restrict);
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
