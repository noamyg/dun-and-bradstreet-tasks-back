using TaskManagement.Domain.Exceptions;

namespace TaskManagement.Application.Handlers;

public abstract class TaskTypeHandlerBase
{
    protected static void RequireField(Dictionary<string, string> data, string key)
    {
        if (!data.TryGetValue(key, out var value) || string.IsNullOrWhiteSpace(value))
            throw new DomainException($"'{key}' is required.");
    }
}
