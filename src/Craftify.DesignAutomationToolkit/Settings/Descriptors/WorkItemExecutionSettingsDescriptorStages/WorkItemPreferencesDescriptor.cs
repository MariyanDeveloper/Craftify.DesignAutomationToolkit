namespace Craftify.DesignAutomationToolkit.Settings.Descriptors.WorkItemExecutionSettingsDescriptorStages;

public class WorkItemPreferencesDescriptor
{
    internal WorkItemPreferences WorkItemPreferences { get; } = new();
    
    public WorkItemPreferencesDescriptor WithSettings(Action<IWorkItemExecutionDecider> configDescriptor)
    {
        var settingsDescriptor = WorkItemExecutionSettingsDescriptor.Create();
        configDescriptor(settingsDescriptor);
        WorkItemPreferences.WorkItemExecutionSettings = ((WorkItemExecutionSettingsDescriptor)settingsDescriptor).WorkItemExecutionSettings;
        return this;
    }
    
}