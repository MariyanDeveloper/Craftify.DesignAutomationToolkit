using Craftify.DesignAutomationToolkit.Settings;

namespace Craftify.DesignAutomationToolkit.Interfaces;

public interface IWorkItemExecutionFactory
{
    IWorkItemExecution Create(WorkItemExecutionSettings executionSettings);
}