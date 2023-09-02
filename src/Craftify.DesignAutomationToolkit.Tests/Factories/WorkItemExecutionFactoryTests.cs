using System.Reflection;
using Craftify.DesignAutomationToolkit.Activators.Executions;
using Craftify.DesignAutomationToolkit.Constants;
using Craftify.DesignAutomationToolkit.Executions;
using Craftify.DesignAutomationToolkit.Factories;
using Craftify.DesignAutomationToolkit.Interfaces;
using Craftify.DesignAutomationToolkit.KeyGenerators;
using Craftify.DesignAutomationToolkit.Measurements;
using Craftify.DesignAutomationToolkit.Settings;
using Craftify.DesignAutomationToolkit.Tests.Helpers.WorkItemsApi;

namespace Craftify.DesignAutomationToolkit.Tests.Factories;

public class WorkItemExecutionFactoryTests
{
    private static readonly FieldInfo GetDelayInSecondsFiledInfo =
        typeof(PollingWorkItemExecution).GetField("_delayInSeconds", BindingFlags.NonPublic | BindingFlags.Instance);
    
    private static readonly FieldInfo GetBaseCallbackUrlFiledInfo =
        typeof(CallbackWorkItemExecution).GetField("_callbackBaseUrl", BindingFlags.NonPublic | BindingFlags.Instance);
    
    [Fact]
    public void Create_WhenPollingIsRequested_ReturnsExpectedInstanceWithDelay()
    {
        var timedExecutionMeasurement = new TimedExecutionResultMeasurement();
        var workItemsApi = WorkItemsApiMockBuilder
            .CreateWorkItemsApiWithScenarioOnThirdAttemptSuccessfulSuccess().Object;
        var keyGenerator = new UniqueTrackingKeyGenerator();
        var workItemExecutionFactory = new WorkItemExecutionFactory(new IWorkItemExecutionActivator[]
        {
            new PollingWorkItemExecutionActivator(workItemsApi, timedExecutionMeasurement),
            new CallbackWorkItemExecutionActivator(workItemsApi, keyGenerator, timedExecutionMeasurement)
        });
        var workItemExecution = workItemExecutionFactory.Create(new WorkItemExecutionSettings()
        {
            WorkItemExecutionPreference = WorkItemExecutions.Polling,
            Arguments = new Dictionary<string, object>()
            {
                [ArgumentNames.Delay] = 7
            }
        });
        var expectedDelay = 7;
        var expectedType = typeof(PollingWorkItemExecution);
        var actualDelay = (int)GetDelayInSecondsFiledInfo.GetValue(workItemExecution);
        var isValid = workItemExecution.GetType() == expectedType &&
                      actualDelay == expectedDelay;
        
        Assert.True(isValid);

    }
    
    [Fact]
    public void Create_WhenCallbackIsRequested_ReturnsExpectedInstanceWithCallbackUrl()
    {
        var timedExecutionMeasurement = new TimedExecutionResultMeasurement();
        var workItemsApi = WorkItemsApiMockBuilder
            .CreateWorkItemsApiWithScenarioOnThirdAttemptSuccessfulSuccess().Object;
        var keyGenerator = new UniqueTrackingKeyGenerator();
        var workItemExecutionFactory = new WorkItemExecutionFactory(new IWorkItemExecutionActivator[]
        {
            new PollingWorkItemExecutionActivator(workItemsApi, timedExecutionMeasurement),
            new CallbackWorkItemExecutionActivator(workItemsApi, keyGenerator, timedExecutionMeasurement)
        });
        var workItemExecution = workItemExecutionFactory.Create(new WorkItemExecutionSettings()
        {
            WorkItemExecutionPreference = WorkItemExecutions.Callback,
            Arguments = new Dictionary<string, object>()
            {
                [ArgumentNames.BaseCallbackUrl] = "baseCallbackUrl"
            }
        });
        var expectedCallbackUrl = "baseCallbackUrl";
        var expectedType = typeof(CallbackWorkItemExecution);
        var actualCallbackUrl = (string)GetBaseCallbackUrlFiledInfo.GetValue(workItemExecution);
        var isValid = workItemExecution.GetType() == expectedType &&
                      actualCallbackUrl == expectedCallbackUrl;
        
        Assert.True(isValid);

    }
}