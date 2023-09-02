using Autodesk.Forge.DesignAutomation.Model;
using Craftify.DesignAutomationToolkit.Handlers;
using Craftify.DesignAutomationToolkit.Settings;

namespace Craftify.DesignAutomationToolkit.Interfaces;

public interface IWorkItemProvider
{
    Task<WorkItem> Get(DesignAutomationExecutionSetting designAutomationExecutionSetting, Action<HandlerResult>? onArgumentResultHandled = default);
}