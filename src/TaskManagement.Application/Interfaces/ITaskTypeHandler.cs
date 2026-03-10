using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Enums;

namespace TaskManagement.Application.Interfaces;

public interface ITaskTypeHandler
{
    TaskType TaskType { get; }

    int FinalStatus { get; }

    void ValidateStatusData(TaskEntity task, int newStatus, Dictionary<string, string> statusData);

    void ApplyStatusData(TaskEntity task, int newStatus, Dictionary<string, string> statusData);

    void InitializeData(TaskEntity task);
    
    object? GetResponseData(TaskEntity task);
}
