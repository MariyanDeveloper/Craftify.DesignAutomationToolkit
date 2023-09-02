namespace Craftify.DesignAutomationToolkit.Settings.Descriptors.WorkItemExecutionSettingsDescriptorStages;

public interface IPollingExecutionArgumentsAssignment
{
    IPollingExecutionArgumentsAssignment WithDelayBetweenPolling(int delayInSeconds);
    IPollingExecutionArgumentsAssignment WithRunTimeoutMinutes(int runTimeoutMinutes);
}