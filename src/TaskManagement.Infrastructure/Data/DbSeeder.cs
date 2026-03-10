using Microsoft.EntityFrameworkCore;
using TaskManagement.Domain.Entities;

namespace TaskManagement.Infrastructure.Data;

public static class DbSeeder
{
    public static async Task SeedAsync(AppDbContext context)
    {
        if (await context.Users.AnyAsync())
            return;

        var users = new List<User>
        {
            new() { Name = "Noam Gherson",  Email = "noamg@example.com"  },
            new() { Name = "Ben Gherson",      Email = "beng@example.com"    },
            new() { Name = "Lior Gherson", Email = "liorg@example.com"  },
        };

        context.Users.AddRange(users);
        await context.SaveChangesAsync();
    }
}
