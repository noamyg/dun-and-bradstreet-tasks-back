using Microsoft.EntityFrameworkCore;
using TaskManagement.Application.Interfaces;
using TaskManagement.Domain.Entities;
using TaskManagement.Infrastructure.Data;

namespace TaskManagement.Infrastructure.Repositories;

public class TaskRepository : ITaskRepository
{
    private readonly AppDbContext _db;

    public TaskRepository(AppDbContext db) => _db = db;

    public async Task<TaskEntity?> GetByIdAsync(int id, CancellationToken ct = default) =>
        await TasksWithAllIncludes()
            .FirstOrDefaultAsync(t => t.Id == id, ct);

    public async Task<List<TaskEntity>> GetByAssignedUserIdAsync(int userId, CancellationToken ct = default) =>
        await TasksWithAllIncludes()
            .Where(t => t.AssignedUserId == userId)
            .ToListAsync(ct);

    public async Task AddAsync(TaskEntity task, CancellationToken ct = default) =>
        await _db.Tasks.AddAsync(task, ct);

    public async Task SaveChangesAsync(CancellationToken ct = default) =>
        await _db.SaveChangesAsync(ct);

    private IQueryable<TaskEntity> TasksWithAllIncludes() =>
        _db.Tasks
            .Include(t => t.AssignedUser)
            .Include(t => t.ProcurementData)
            .Include(t => t.DevelopmentData)
            .Include(t => t.StatusHistory)
                .ThenInclude(h => h.AssignedUser);
}
