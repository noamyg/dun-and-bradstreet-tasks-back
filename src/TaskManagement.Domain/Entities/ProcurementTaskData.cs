namespace TaskManagement.Domain.Entities;

public class ProcurementTaskData
{
    public int TaskId { get; set; }
    public TaskEntity Task { get; set; } = null!;

    public string? PriceQuote1 { get; set; }
    public string? PriceQuote2 { get; set; }
    public string? Receipt { get; set; }
}
