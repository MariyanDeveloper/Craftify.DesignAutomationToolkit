using Craftify.DesignAutomationToolkit.Interfaces;
using Craftify.DesignAutomationToolkit.Settings.Components.Interfaces;
using Craftify.DesignAutomationToolkit.Settings.Descriptors.WorkItemExecutionSettingsDescriptorStages;

namespace Craftify.DesignAutomationToolkit.Settings.Descriptors;

public class DesignAutomationSettingDescriptor
{
    internal DesignAutomationExecutionSetting DesignAutomationExecutionSetting { get; } = new();
    public DesignAutomationSettingDescriptor ConfigurePlugin(IPluginConfigurationComponent pluginConfigurationComponent)
    {
        var pluginDescriptor = new PluginDescriptor();
        pluginConfigurationComponent.Configure(pluginDescriptor);
        DesignAutomationExecutionSetting.PluginSetting = pluginDescriptor.PluginSetting;
        return this;
    }
    public DesignAutomationSettingDescriptor ConfigurePluginPlugin(Action<PluginDescriptor> configPlugin)
    {
        var pluginDescriptor = new PluginDescriptor();
        configPlugin(pluginDescriptor);
        DesignAutomationExecutionSetting.PluginSetting = pluginDescriptor.PluginSetting;
        return this;
    }

    public DesignAutomationSettingDescriptor ConfigureStoragePreferences(Action<StoragePreferences> configPreferences)
    {
        var storagePreferences = new StoragePreferences();
        configPreferences(storagePreferences);
        DesignAutomationExecutionSetting.StoragePreferences = storagePreferences;
        return this;
    }

    public DesignAutomationSettingDescriptor AssignAppBundlePreferenceProvider(INamedAliasPreferenceProvider appBundlePreferenceProvider)
    {
        DesignAutomationExecutionSetting.AppBundlePreferenceProvider = appBundlePreferenceProvider ?? throw new ArgumentNullException(nameof(appBundlePreferenceProvider));
        return this;
    }
    public DesignAutomationSettingDescriptor AssignActivityPreferenceProvider(INamedAliasPreferenceProvider activityPreferenceProvider)
    {
        DesignAutomationExecutionSetting.ActivityPreferenceProvider = activityPreferenceProvider ?? throw new ArgumentNullException(nameof(activityPreferenceProvider));
        return this;
    }
    public DesignAutomationSettingDescriptor ConfigureWorkItemPreferences(IWorkItemPreferenceConfigurationComponent workItemPreferenceConfigurationComponent)
    {
        var preferencesDescriptor = new WorkItemPreferencesDescriptor();
        workItemPreferenceConfigurationComponent.Configure(preferencesDescriptor);
        DesignAutomationExecutionSetting.WorkItemPreferences = preferencesDescriptor.WorkItemPreferences;
        return this;
    }
    public DesignAutomationSettingDescriptor ConfigureWorkItemPreferences(Action<WorkItemPreferencesDescriptor> configDescriptor)
    {
        var preferencesDescriptor = new WorkItemPreferencesDescriptor();
        configDescriptor(preferencesDescriptor);
        DesignAutomationExecutionSetting.WorkItemPreferences = preferencesDescriptor.WorkItemPreferences;
        return this;
    }
    
}