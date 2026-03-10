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

    public ProcurementTaskData? ProcurementData { get; set; }
    public DevelopmentTaskData? DevelopmentData { get; set; }

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

        if (newStatus > Status + 1)
            throw new DomainException($"Cannot jump from status {Status} to {newStatus}.");
    }
}
