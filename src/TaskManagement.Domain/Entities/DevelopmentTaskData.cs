namespace TaskManagement.Domain.Entities;

public class DevelopmentTaskData
{
    public int TaskId { get; set; }
    public TaskEntity Task { get; set; } = null!;
    public string? SpecificationText { get; set; }
    public string? BranchName { get; set; }
    public string? VersionNumber { get; set; }
}
