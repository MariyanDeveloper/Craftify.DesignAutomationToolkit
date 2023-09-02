using Craftify.DesignAutomationToolkit.Constants;
using Craftify.DesignAutomationToolkit.Settings.Descriptors.WorkItemExecutionSettingsDescriptorStages;

namespace Craftify.DesignAutomationToolkit.Settings.Descriptors;

public class WorkItemExecutionSettingsDescriptor : IWorkItemExecutionDecider, IPollingExecutionArgumentsAssignment, ICallbackExecutionArgumentsAssignment
{
    internal WorkItemExecutionSettings WorkItemExecutionSettings { get; } = new()
    {
        Arguments = new()
        {
            [ArgumentNames.Delay] = 5,
            [ArgumentNames.RunTimeoutMinutes] = 10,
            [ArgumentNames.BaseCallbackUrl] = string.Empty,
        }
    };
    private WorkItemExecutionSettingsDescriptor(){}
    public static IWorkItemExecutionDecider Create() => new WorkItemExecutionSettingsDescriptor();

    public IPollingExecutionArgumentsAssignment ChoosePolling()
    {
        WorkItemExecutionSettings.WorkItemExecutionPreference = WorkItemExecutions.Polling;
        return this;
    }

    public ICallbackExecutionArgumentsAssignment ChooseCallback()
    {
        WorkItemExecutionSettings.WorkItemExecutionPreference = WorkItemExecutions.Callback;
        return this;
    }
    public IPollingExecutionArgumentsAssignment WithDelayBetweenPolling(int delayInSeconds)
    {
        WorkItemExecutionSettings.Arguments[ArgumentNames.Delay] = delayInSeconds;
        return this;
    }

    public IPollingExecutionArgumentsAssignment WithRunTimeoutMinutes(int runTimeoutMinutes)
    {
        WorkItemExecutionSettings.Arguments[ArgumentNames.RunTimeoutMinutes] = runTimeoutMinutes;
        return this;
    }

    public ICallbackExecutionArgumentsAssignment WithCallbackBaseUrl(string callbackBaseUrl)
    {
        WorkItemExecutionSettings.Arguments[ArgumentNames.BaseCallbackUrl] = callbackBaseUrl;
        return this;
    }
}