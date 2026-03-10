using Microsoft.AspNetCore.Mvc;
using TaskManagement.Application.DTOs;
using TaskManagement.Application.Interfaces;

namespace TaskManagement.API.Controllers;

[ApiController]
[Route("api/tasks")]
public class TasksController : ControllerBase
{
    private readonly ITaskService _taskService;

    public TasksController(ITaskService taskService) => _taskService = taskService;

    [HttpPost]
    [ProducesResponseType(typeof(TaskDto), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create(
        [FromBody] CreateTaskDto request,
        CancellationToken ct)
    {
        var response = await _taskService.CreateTaskAsync(request, ct);
        return CreatedAtAction(nameof(GetUserTasks), new { userId = response.AssignedUserId }, response);
    }

    [HttpPatch("{taskId:int}/status")]
    [ProducesResponseType(typeof(TaskDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ChangeStatus(
        int taskId,
        [FromBody] ChangeStatusDto request,
        CancellationToken ct)
    {
        var response = await _taskService.ChangeStatusAsync(taskId, request, ct);
        return Ok(response);
    }

    [HttpPost("{taskId:int}/close")]
    public async Task<IActionResult> Close(int taskId, CancellationToken ct)
    {
        var response = await _taskService.CloseTaskAsync(taskId, ct);
        return Ok(response);
    }

    [HttpGet("assigned-to/{userId:int}")]
    public async Task<IActionResult> GetUserTasks(int userId)
    {
        var tasks = await _taskService.GetUserTasksAsync(userId);
        return Ok(tasks);
    }
}
