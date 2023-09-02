using Autodesk.Forge.DesignAutomation.Model;

namespace Craftify.DesignAutomationToolkit.Executions.Results;

public record WorkItemResult(string Id, string ActivityId, WorkItemStatus WorkItemStatus)
{
    public bool IsSuccessful => this.WorkItemStatus.Status == Status.Success;
};