namespace TaskManagement.Domain.Entities;

public class TaskStatusHistory
{
    public int Id { get; set; }

    public int TaskId { get; set; }
    public TaskEntity Task { get; set; } = null!;

    public int? FromStatus { get; set; }

    public int ToStatus { get; set; }

    public int AssignedUserId { get; set; }
    public User AssignedUser { get; set; } = null!;

    public DateTime ChangedAt { get; set; }
}
