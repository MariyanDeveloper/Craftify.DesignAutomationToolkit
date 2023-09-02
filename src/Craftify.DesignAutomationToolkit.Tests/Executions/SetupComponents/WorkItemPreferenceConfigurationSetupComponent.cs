using Craftify.DesignAutomationToolkit.Settings.Components.Interfaces;
using Craftify.DesignAutomationToolkit.Settings.Descriptors.WorkItemExecutionSettingsDescriptorStages;

namespace Craftify.DesignAutomationToolkit.Tests.Executions.SetupComponents;

public class WorkItemPreferenceConfigurationSetupComponent : IWorkItemPreferenceConfigurationComponent
{
    public void Configure(WorkItemPreferencesDescriptor workItemPreferencesDescriptor)
    {
        workItemPreferencesDescriptor
            .WithSettings(settingsDescriptor =>
            {
                settingsDescriptor
                    .ChoosePolling()
                    .WithDelayBetweenPolling(3)
                    .WithRunTimeoutMinutes(15);
            });
    }
}