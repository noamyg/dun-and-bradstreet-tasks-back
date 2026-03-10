using TaskManagement.Domain.Enums;
using TaskManagement.Domain.Exceptions;

namespace TaskManagement.Domain.Entities;

public class TaskEntity
{
    public int Id { get; set; }
    public TaskType Type { get; set; }
    public int Status { get; set; }
    public bool IsClosed { get; set; }
    public int AssignedUserId { get; set; }
    public User AssignedUser { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public Dictionary<string, string> TypeData { get; set; } = new();

    public ICollection<TaskStatusHistory> StatusHistory { get; set; } = new List<TaskStatusHistory>();

    public void EnsureNotClosed()
    {
        if (IsClosed)
            throw new DomainException("Task is already closed.");
    }

    public void ValidateStatusTransition(int newStatus)
    {
        if (newStatus < 1)
            throw new DomainException("Status must be a positive integer.");

        if (newStatus == Status)
            throw new DomainException($"Task is already at status {Status}.");

        var distance = Math.Abs(newStatus - Status);
        if (distance > 1)
            throw new DomainException($"Cannot jump from status {Status} to {newStatus}. Only single-step transitions are allowed.");
    }
}
