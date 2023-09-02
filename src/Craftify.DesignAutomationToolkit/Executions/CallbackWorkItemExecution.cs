using System.Collections.Concurrent;
using Autodesk.Forge.DesignAutomation.Http;
using Autodesk.Forge.DesignAutomation.Model;
using Craftify.DesignAutomation.ModelBuilders.Builders;
using Craftify.DesignAutomation.Shared.Constants;
using Craftify.DesignAutomationToolkit.Executions.Results;
using Craftify.DesignAutomationToolkit.Extensions;
using Craftify.DesignAutomationToolkit.Interfaces;

namespace Craftify.DesignAutomationToolkit.Executions;

public class CallbackWorkItemExecution : IWorkItemExecution
{
    private readonly IWorkItemsApi _workItemsApi;
    private readonly IKeyGenerator _keyGenerator;
    private readonly ITimedExecutionResultMeasurement _executionResultMeasurement;
    private readonly string _callbackBaseUrl;
    private readonly ConcurrentDictionary<string, TaskCompletionSource<WorkItemStatus>> _workItemTracker = new();

    public CallbackWorkItemExecution(
        IWorkItemsApi workItemsApi,
        IKeyGenerator keyGenerator,
        ITimedExecutionResultMeasurement executionResultMeasurement,
        string callbackBaseUrl)
    {
        _workItemsApi = workItemsApi ?? throw new ArgumentNullException(nameof(workItemsApi));
        _keyGenerator = keyGenerator ?? throw new ArgumentNullException(nameof(keyGenerator));
        _executionResultMeasurement = executionResultMeasurement ?? throw new ArgumentNullException(nameof(executionResultMeasurement));
        _callbackBaseUrl = callbackBaseUrl ?? throw new ArgumentNullException(nameof(callbackBaseUrl));
    }
    public async Task<TimedExecutionResult<WorkItemResult>> Execute(WorkItem workItem,
        CancellationToken cancellationToken)
    {
        var trackingKey = _keyGenerator.Generate();
        var completionSource = _workItemTracker.GetOrAdd(
            trackingKey,
            new TaskCompletionSource<WorkItemStatus>());
        var callbackUrl = $"{_callbackBaseUrl}{trackingKey}";
        var argument = new ArgumentBuilder()
            .BuildCallback(callbackUrl);
        workItem.Arguments.Add(SpecialOutputArguments.OnComplete, argument);
        return await _executionResultMeasurement.MeasureAsync(async () =>
        {
            try
            {
                var _ = await _workItemsApi.CreateWorkItemStatusAsync(workItem);
            }
            catch (Exception e)
            {
                _workItemTracker.TryRemove(trackingKey, out _);
                throw;
            }
            var completedWorkItemStatus = await completionSource.Task;
            return new WorkItemResult(workItem.Id, workItem.ActivityId, completedWorkItemStatus);
        });

    }
    
}