using Autodesk.Forge.DesignAutomation.Model;
using Craftify.DesignAutomationToolkit.Executions.Results;

namespace Craftify.DesignAutomationToolkit.Interfaces;

public interface IWorkItemExecution
{
    Task<TimedExecutionResult<WorkItemResult>> Execute(WorkItem workItem, CancellationToken cancellationToken);
}