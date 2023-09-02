using Craftify.DesignAutomationToolkit.Settings;

namespace Craftify.DesignAutomationToolkit.Interfaces;

public interface IWorkItemExecutionActivator
{
    string Type { get; }
    IWorkItemExecution Create(WorkItemExecutionSettings executionSettings);
}