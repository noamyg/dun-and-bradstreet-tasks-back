using TaskManagement.Domain.Entities;

namespace TaskManagement.Application.Interfaces;

public interface ITaskRepository
{
    Task<TaskEntity?> GetByIdAsync(int id, CancellationToken ct = default);

    Task<List<TaskEntity>> GetByAssignedUserIdAsync(int userId, CancellationToken ct = default);

    Task AddAsync(TaskEntity task, CancellationToken ct = default);

    Task SaveChangesAsync(CancellationToken ct = default);
}
