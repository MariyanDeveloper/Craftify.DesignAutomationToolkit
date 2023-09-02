using Autodesk.Forge.DesignAutomation.Model;
using Craftify.DesignAutomationToolkit.Executions;
using Craftify.DesignAutomationToolkit.Measurements;
using Craftify.DesignAutomationToolkit.Tests.Helpers.WorkItemsApi;

namespace Craftify.DesignAutomationToolkit.Tests.Executions;

public class PollingWorkItemExecutionTests
{
    [Fact]
    public async Task Execute_OnThirdTimeSuccessStatus_ReturnsSuccessfulStatus()
    {
        var workItemsApi = WorkItemsApiMockBuilder
            .CreateWorkItemsApiWithScenarioOnThirdAttemptSuccessfulSuccess().Object;
        var timedExecutionMeasurement = new TimedExecutionResultMeasurement();
        var pollingWorkItemExecution = new PollingWorkItemExecution(
            workItemsApi,
            timedExecutionMeasurement,
            1);
        var workItem = new WorkItem();
        var executionResult = await pollingWorkItemExecution.Execute(workItem, default);
        
        Assert.True(executionResult.Result.WorkItemStatus.Status == Status.Success);
    }
    
    [Fact]
    public async Task Execute_WithFiveSecondsCancellationToken_ReturnsCancelledStatus()
    {
        var workItemsApi = WorkItemsApiMockBuilder
            .CreateWorkItemsApiWithScenarioOnThirdAttemptSuccessfulSuccess().Object;
        var timedExecutionMeasurement = new TimedExecutionResultMeasurement();
        var pollingWorkItemExecution = new PollingWorkItemExecution(
            workItemsApi,
            timedExecutionMeasurement,
            2);
        var workItem = new WorkItem();
        var cancellationToken = new CancellationTokenSource(TimeSpan.FromSeconds(5)).Token;
        var executionResult = await pollingWorkItemExecution.Execute(workItem, cancellationToken);
        
        Assert.True(executionResult.Result.WorkItemStatus.Status == Status.Cancelled);
    }
}