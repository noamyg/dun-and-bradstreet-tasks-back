using Microsoft.Extensions.DependencyInjection;
using TaskManagement.Application.Interfaces;
using TaskManagement.Application.Services;

namespace TaskManagement.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(typeof(DependencyInjection).Assembly);
        services.AddScoped<ITaskService, TaskService>();
        services.AddScoped<IUserService, UserService>();

        var handlerTypes = typeof(DependencyInjection).Assembly.GetTypes()
            .Where(t => t is { IsAbstract: false, IsInterface: false }
                        && typeof(ITaskTypeHandler).IsAssignableFrom(t));

        foreach (var type in handlerTypes)
            services.AddScoped(typeof(ITaskTypeHandler), type);

        return services;
    }
}
