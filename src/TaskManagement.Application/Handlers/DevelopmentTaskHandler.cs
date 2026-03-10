using TaskManagement.Application.Interfaces;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Enums;

namespace TaskManagement.Application.Handlers;

public class DevelopmentTaskHandler : TaskTypeHandlerBase, ITaskTypeHandler
{
    private enum Status { Created = 1, SpecificationCompleted = 2, DevelopmentCompleted = 3, DistributionCompleted = 4 }

    public TaskType TaskType => TaskType.Development;
    public int FinalStatus => (int)Status.DistributionCompleted;

    public void ValidateStatusData(TaskEntity task, int newStatus, Dictionary<string, string> statusData)
    {
        if (newStatus == (int)Status.SpecificationCompleted)
            RequireField(statusData, "specificationText");
        else if (newStatus == (int)Status.DevelopmentCompleted)
            RequireField(statusData, "branchName");
        else if (newStatus == (int)Status.DistributionCompleted)
            RequireField(statusData, "versionNumber");
    }

    public void ApplyStatusData(TaskEntity task, int newStatus, Dictionary<string, string> statusData)
    {
        var data = task.DevelopmentData!;

        if (newStatus == (int)Status.SpecificationCompleted)
            data.SpecificationText = statusData["specificationText"];
        else if (newStatus == (int)Status.DevelopmentCompleted)
            data.BranchName = statusData["branchName"];
        else if (newStatus == (int)Status.DistributionCompleted)
            data.VersionNumber = statusData["versionNumber"];
    }

    public void InitializeData(TaskEntity task)
    {
        task.DevelopmentData = new DevelopmentTaskData();
    }

    public object? GetResponseData(TaskEntity task) =>
        task.DevelopmentData is null ? null : new
        {
            task.DevelopmentData.SpecificationText,
            task.DevelopmentData.BranchName,
            task.DevelopmentData.VersionNumber
        };
}
