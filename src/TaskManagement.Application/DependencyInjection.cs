using Microsoft.Extensions.DependencyInjection;
using TaskManagement.Application.Handlers;
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
        services.AddScoped<ITaskTypeHandler, ProcurementTaskHandler>();
        services.AddScoped<ITaskTypeHandler, DevelopmentTaskHandler>();

        return services;
    }
}
