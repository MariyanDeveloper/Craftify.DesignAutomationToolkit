using Autodesk.Forge.DesignAutomation.Http;
using Craftify.DesignAutomationToolkit.Constants;
using Craftify.DesignAutomationToolkit.Executions;
using Craftify.DesignAutomationToolkit.Interfaces;
using Craftify.DesignAutomationToolkit.Settings;
using Craftify.DesignAutomationToolkit.Settings.Extensions;

namespace Craftify.DesignAutomationToolkit.Activators.Executions;

public class PollingWorkItemExecutionActivator : IWorkItemExecutionActivator
{
    private readonly IWorkItemsApi _workItemsApi;
    private readonly ITimedExecutionResultMeasurement _executionResultMeasurement;
    public string Type => WorkItemExecutions.Polling;

    public PollingWorkItemExecutionActivator(
        IWorkItemsApi workItemsApi,
        ITimedExecutionResultMeasurement executionResultMeasurement)
    {
        _workItemsApi = workItemsApi ?? throw new ArgumentNullException(nameof(workItemsApi));
        _executionResultMeasurement = executionResultMeasurement ?? throw new ArgumentNullException(nameof(executionResultMeasurement));
    }
    public IWorkItemExecution Create(WorkItemExecutionSettings executionSettings)
    {
        return new PollingWorkItemExecution(
                _workItemsApi,
                _executionResultMeasurement,
                executionSettings.ExtractDelay());
    }
}