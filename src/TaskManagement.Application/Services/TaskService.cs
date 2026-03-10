using AutoMapper;
using TaskManagement.Application.DTOs;
using TaskManagement.Application.Interfaces;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Enums;
using TaskManagement.Domain.Exceptions;

namespace TaskManagement.Application.Services;

public class TaskService : ITaskService
{
    private readonly ITaskRepository _tasks;
    private readonly IUserService _userService;
    private readonly IEnumerable<ITaskTypeHandler> _handlers;
    private readonly IMapper _mapper;

    public TaskService(
        ITaskRepository tasks,
        IUserService userService,
        IEnumerable<ITaskTypeHandler> handlers,
        IMapper mapper)
    {
        _tasks = tasks;
        _userService = userService;
        _handlers = handlers;
        _mapper = mapper;
    }

    public async Task<TaskDto> CreateTaskAsync(CreateTaskDto request, CancellationToken ct = default)
    {
        var user = await _userService.RequireUserAsync(request.AssignedUserId, ct);
        var handler = ResolveHandler(request.Type);

        var task = new TaskEntity
        {
            Type = request.Type,
            Status = 1,
            IsClosed = false,
            AssignedUserId = user.Id,
            CreatedAt = DateTime.UtcNow
        };

        handler.InitializeData(task);

        task.StatusHistory.Add(new TaskStatusHistory
        {
            FromStatus = null,
            ToStatus = 1,
            AssignedUserId = user.Id,
            ChangedAt = task.CreatedAt
        });

        await _tasks.AddAsync(task, ct);
        await _tasks.SaveChangesAsync(ct);

        var created = await _tasks.GetByIdAsync(task.Id, ct)
            ?? throw new InvalidOperationException("Task was not persisted correctly.");

        return MapToResponse(created, handler);
    }

    public async Task<TaskDto> ChangeStatusAsync(int taskId, ChangeStatusDto request, CancellationToken ct = default)
    {
        var task = await RequireTaskAsync(taskId, ct);
        var handler = ResolveHandler(task.Type);
        var nextUser = await _userService.RequireUserAsync(request.NextAssignedUserId, ct);
        task.EnsureNotClosed();

        task.ValidateStatusTransition(request.NewStatus);

        handler.ValidateStatusData(task, request.NewStatus, request.StatusData);

        handler.ApplyStatusData(task, request.NewStatus, request.StatusData);

        var previousStatus = task.Status;
        task.Status = request.NewStatus;
        task.AssignedUserId = nextUser.Id;

        task.StatusHistory.Add(new TaskStatusHistory
        {
            FromStatus = previousStatus,
            ToStatus = request.NewStatus,
            AssignedUserId = nextUser.Id,
            ChangedAt = DateTime.UtcNow
        });

        await _tasks.SaveChangesAsync(ct);

        return MapToResponse(task, handler);
    }

    public async Task<TaskDto> CloseTaskAsync(int taskId, CancellationToken ct = default)
    {
        var task = await RequireTaskAsync(taskId, ct);
        var handler = ResolveHandler(task.Type);

        task.EnsureNotClosed();

        if (task.Status != handler.FinalStatus)
            throw new DomainException(
                $"A {task.Type} task can only be closed at status {handler.FinalStatus} " +
                $"(current status: {task.Status}).");

        task.IsClosed = true;

        await _tasks.SaveChangesAsync(ct);

        return MapToResponse(task, handler);
    }

    public async Task<List<TaskDto>> GetUserTasksAsync(int userId, CancellationToken ct = default)
    {
        await _userService.RequireUserAsync(userId, ct);

        var tasks = await _tasks.GetByAssignedUserIdAsync(userId, ct);

        return tasks
            .Select(t => MapToResponse(t, ResolveHandler(t.Type)))
            .ToList();
    }

    private ITaskTypeHandler ResolveHandler(TaskType type) =>
        _handlers.FirstOrDefault(h => h.TaskType == type)
            ?? throw new NotSupportedException($"Unsupported task type: {type}");

    private async Task<TaskEntity> RequireTaskAsync(int taskId, CancellationToken ct)
    {
        var task = await _tasks.GetByIdAsync(taskId, ct);
        if (task is null)
            throw new DomainException($"Task {taskId} not found.");
        return task;
    }

    private TaskDto MapToResponse(TaskEntity task, ITaskTypeHandler handler)
    {
        var response = _mapper.Map<TaskDto>(task);
        response.TypeData = handler.GetResponseData(task);
        response.StatusHistory = response.StatusHistory.OrderBy(h => h.ChangedAt).ToList();
        return response;
    }
}
