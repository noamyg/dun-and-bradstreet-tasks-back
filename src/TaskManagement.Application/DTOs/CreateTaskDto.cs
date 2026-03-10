using TaskManagement.Domain.Enums;

namespace TaskManagement.Application.DTOs;

public class CreateTaskDto
{
    public TaskType Type { get; set; }
    public int AssignedUserId { get; set; }
}
