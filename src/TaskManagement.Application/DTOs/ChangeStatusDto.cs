namespace TaskManagement.Application.DTOs;

public class ChangeStatusDto
{
    public int NewStatus { get; set; }
    public int NextAssignedUserId { get; set; }
    public Dictionary<string, string> StatusData { get; set; } = new();
}
