﻿using Craftify.DesignAutomationToolkit.Settings.Descriptors.WorkItemExecutionSettingsDescriptorStages;

namespace Craftify.DesignAutomationToolkit.Settings.Components.Interfaces;

public interface IWorkItemPreferenceConfigurationComponent
{
    void Configure(WorkItemPreferencesDescriptor workItemPreferencesDescriptor);
}