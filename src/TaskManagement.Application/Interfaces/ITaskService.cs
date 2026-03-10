using TaskManagement.Application.DTOs;

namespace TaskManagement.Application.Interfaces;

public interface ITaskService
{
    Task<TaskDto> CreateTaskAsync(CreateTaskDto request, CancellationToken ct = default);
    Task<TaskDto> ChangeStatusAsync(int taskId, ChangeStatusDto request, CancellationToken ct = default);
    Task<TaskDto> CloseTaskAsync(int taskId, CancellationToken ct = default);
    Task<List<TaskDto>> GetUserTasksAsync(int userId, CancellationToken ct = default);
}
