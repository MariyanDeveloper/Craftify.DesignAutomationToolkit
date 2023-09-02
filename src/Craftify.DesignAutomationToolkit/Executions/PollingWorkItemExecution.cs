using Autodesk.Forge.DesignAutomation.Http;
using Autodesk.Forge.DesignAutomation.Model;
using Craftify.DesignAutomationToolkit.Executions.Results;
using Craftify.DesignAutomationToolkit.Extensions;
using Craftify.DesignAutomationToolkit.Interfaces;

namespace Craftify.DesignAutomationToolkit.Executions;

public class PollingWorkItemExecution : IWorkItemExecution
{
    private readonly IWorkItemsApi _workItemsApi;
    private readonly ITimedExecutionResultMeasurement _executionResultMeasurement;
    private readonly int _delayInSeconds;
    private readonly int _defaultRunTimeoutMinutes = 10;
    private readonly int _runTimeoutMinutes;

    public PollingWorkItemExecution(
        IWorkItemsApi workItemsApi,
        ITimedExecutionResultMeasurement executionResultMeasurement,
        int delayInSeconds,
        int? runTimeoutMinutes = null)
    {
        _workItemsApi = workItemsApi ?? throw new ArgumentNullException(nameof(workItemsApi));
        _executionResultMeasurement = executionResultMeasurement ?? throw new ArgumentNullException(nameof(executionResultMeasurement));
        _delayInSeconds = delayInSeconds;
        _runTimeoutMinutes = runTimeoutMinutes ?? _defaultRunTimeoutMinutes;
    }
    public async Task<TimedExecutionResult<WorkItemResult>> Execute(WorkItem workItem,
        CancellationToken cancellationToken)
    {
        if (cancellationToken == CancellationToken.None)
        {
            var delayTimeSpan = TimeSpan.FromMinutes(_runTimeoutMinutes); 
            cancellationToken = new CancellationTokenSource(delayTimeSpan).Token;
        }

        return await _executionResultMeasurement.MeasureAsync(async () =>
        {
            var workItemStatus = await _workItemsApi.CreateWorkItemStatusAsync(workItem);
            while (workItemStatus.IsNotFinished())
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    await _workItemsApi.DeleteWorkItemAsync(workItem.Id);
                    //TODO maybe introduce a delay for a mitigation against potential inconsistencies or latencies
                    workItemStatus.Status = Status.Cancelled;
                    break;
                }
                try
                {
                    await Task.Delay(TimeSpan.FromSeconds(_delayInSeconds), cancellationToken);
                }
                catch (TaskCanceledException) { }
                workItemStatus = await _workItemsApi.GetWorkItemStatusAsync(workItemStatus.Id);
            }
            return new WorkItemResult(workItem.Id, workItem.ActivityId, workItemStatus);
        });
    }
}