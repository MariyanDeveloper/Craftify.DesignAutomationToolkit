using Autodesk.Forge.DesignAutomation.Http;
using Autodesk.Forge.DesignAutomation.Model;

namespace Craftify.DesignAutomationToolkit.Extensions;

public static class WorkItemsApiExtensions
{
    public static async Task<WorkItemStatus> CreateWorkItemStatusAsync(this IWorkItemsApi workItemsApi, WorkItem workItem)
    {
        return (await workItemsApi.CreateWorkItemAsync(workItem)).Content;
    }
    public static async Task<WorkItemStatus> GetWorkItemStatusAsync(this IWorkItemsApi workItemsApi, string statusId)
    {
        return (await workItemsApi.GetWorkitemStatusAsync(statusId)).Content;
    }
}