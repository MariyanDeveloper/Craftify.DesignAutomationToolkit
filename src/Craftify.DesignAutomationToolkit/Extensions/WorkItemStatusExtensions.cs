using Autodesk.Forge.DesignAutomation.Model;

namespace Craftify.DesignAutomationToolkit.Extensions;

public static class WorkItemStatusExtensions
{
    public static bool IsNotFinished(this WorkItemStatus workItemStatus)
    {
        return workItemStatus.Status is Status.Pending or Status.Inprogress;
    }
}