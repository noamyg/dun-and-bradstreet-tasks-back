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
        var data = task.ProcurementData!;

        if (newStatus == (int)Status.SupplierOffersReceived)
        {
            data.PriceQuote1 = statusData["priceQuote1"];
            data.PriceQuote2 = statusData["priceQuote2"];
        }
        else if (newStatus == (int)Status.PurchaseCompleted)
        {
            data.Receipt = statusData["receipt"];
        }
    }

    public void InitializeData(TaskEntity task)
    {
        task.ProcurementData = new ProcurementTaskData();
    }

    public object? GetResponseData(TaskEntity task) =>
        task.ProcurementData is null ? null : new
        {
            task.ProcurementData.PriceQuote1,
            task.ProcurementData.PriceQuote2,
            task.ProcurementData.Receipt
        };
}
