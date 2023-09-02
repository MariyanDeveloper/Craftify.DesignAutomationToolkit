using Autodesk.Forge.DesignAutomation.Http;
using Craftify.DesignAutomationToolkit.Constants;
using Craftify.DesignAutomationToolkit.Executions;
using Craftify.DesignAutomationToolkit.Interfaces;
using Craftify.DesignAutomationToolkit.Settings;
using Craftify.DesignAutomationToolkit.Settings.Extensions;

namespace Craftify.DesignAutomationToolkit.Activators.Executions;

public class CallbackWorkItemExecutionActivator : IWorkItemExecutionActivator
{
    private readonly IWorkItemsApi _workItemsApi;
    private readonly IKeyGenerator _keyGenerator;
    private readonly ITimedExecutionResultMeasurement _executionResultMeasurement;
    public string Type => WorkItemExecutions.Callback;

    public CallbackWorkItemExecutionActivator(
        IWorkItemsApi workItemsApi,
        IKeyGenerator keyGenerator, ITimedExecutionResultMeasurement executionResultMeasurement)
    {
        _workItemsApi = workItemsApi ?? throw new ArgumentNullException(nameof(workItemsApi));
        _keyGenerator = keyGenerator ?? throw new ArgumentNullException(nameof(keyGenerator));
        _executionResultMeasurement = executionResultMeasurement ?? throw new ArgumentNullException(nameof(executionResultMeasurement));
    }
    public IWorkItemExecution Create(WorkItemExecutionSettings executionSettings)
    {
        return new CallbackWorkItemExecution(
            _workItemsApi,
            _keyGenerator,
            _executionResultMeasurement,
            executionSettings.ExtractBaseCallbackUrl());
    }
}