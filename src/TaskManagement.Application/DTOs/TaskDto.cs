using AutoMapper;
using TaskManagement.Domain.Entities;

namespace TaskManagement.Application.DTOs;

[AutoMap(typeof(TaskEntity))]
public class TaskDto
{
    public int Id { get; set; }
    public string Type { get; set; } = string.Empty;

    public int Status { get; set; }
    public bool IsClosed { get; set; }
    public int AssignedUserId { get; set; }
    public string AssignedUserName { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }
    public object? TypeData { get; set; }

    public List<StatusHistoryDto> StatusHistory { get; set; } = new();
}

[AutoMap(typeof(TaskStatusHistory))]
public class StatusHistoryDto
{
    public int? FromStatus { get; set; }
    public int ToStatus { get; set; }
    public int AssignedUserId { get; set; }
    public string AssignedUserName { get; set; } = string.Empty;
    public DateTime ChangedAt { get; set; }
}
