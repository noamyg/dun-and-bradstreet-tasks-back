using Microsoft.EntityFrameworkCore;
using TaskManagement.Application.Interfaces;
using TaskManagement.Domain.Entities;
using TaskManagement.Infrastructure.Data;

namespace TaskManagement.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext db) => _context = db;

    public async Task<User?> GetByIdAsync(int id, CancellationToken ct = default) =>
        await _context.Users.FindAsync([id], ct);

    public async Task<List<User>> GetAllAsync(CancellationToken ct = default) =>
        await _context.Users.ToListAsync(ct);
}
