using TaskManagement.Application.Interfaces;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Enums;

namespace TaskManagement.Application.Handlers;

public class ProcurementTaskHandler : TaskTypeHandlerBase, ITaskTypeHandler
{
    private enum Status { Created = 1, SupplierOffersReceived = 2, PurchaseCompleted = 3 }

    public TaskType TaskType => TaskType.Procurement;
    public int FinalStatus => (int)Status.PurchaseCompleted;

    public void ValidateStatusData(TaskEntity task, int newStatus, Dictionary<string, string> statusData)
    {
        if (newStatus == (int)Status.SupplierOffersReceived)
        {
            RequireField(statusData, "priceQuote1");
            RequireField(statusData, "priceQuote2");
        }
        else if (newStatus == (int)Status.PurchaseCompleted)
        {
            RequireField(statusData, "receipt");
        }
    }

    public void ApplyStatusData(TaskEntity task, int newStatus, Dictionary<string, string> statusData)
    {
        if (newStatus == (int)Status.SupplierOffersReceived)
        {
            task.TypeData["priceQuote1"] = statusData["priceQuote1"];
            task.TypeData["priceQuote2"] = statusData["priceQuote2"];
        }
        else if (newStatus == (int)Status.PurchaseCompleted)
        {
            task.TypeData["receipt"] = statusData["receipt"];
        }
    }

    public void InitializeData(TaskEntity task) { }

    public object? GetResponseData(TaskEntity task) =>
        task.TypeData.Count == 0 ? null : task.TypeData;
}
