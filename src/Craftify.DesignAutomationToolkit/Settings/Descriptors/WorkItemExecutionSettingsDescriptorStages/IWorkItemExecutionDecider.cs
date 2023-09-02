namespace Craftify.DesignAutomationToolkit.Settings.Descriptors.WorkItemExecutionSettingsDescriptorStages;

public interface IWorkItemExecutionDecider
{
    IPollingExecutionArgumentsAssignment ChoosePolling();
    ICallbackExecutionArgumentsAssignment ChooseCallback();
}