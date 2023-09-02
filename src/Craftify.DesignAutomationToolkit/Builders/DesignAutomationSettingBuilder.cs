using Craftify.DesignAutomationToolkit.Interfaces;
using Craftify.DesignAutomationToolkit.Settings;
using Craftify.DesignAutomationToolkit.Settings.Components.Interfaces;
using Craftify.DesignAutomationToolkit.Settings.Descriptors;
using Craftify.DesignAutomationToolkit.Settings.Descriptors.WorkItemExecutionSettingsDescriptorStages;

namespace Craftify.DesignAutomationToolkit.Builders;

public class DesignAutomationSettingBuilder
{
    private readonly DesignAutomationExecutionSetting _designAutomationExecutionSetting = new();
    private DesignAutomationSettingBuilder(){}
    public static DesignAutomationSettingBuilder Create() => new ();

    public DesignAutomationSettingBuilder ConfigurePlugin(IPluginConfigurationComponent pluginConfigurationComponent)
    {
        var pluginDescriptor = new PluginDescriptor();
        pluginConfigurationComponent.Configure(pluginDescriptor);
        _designAutomationExecutionSetting.PluginSetting = pluginDescriptor.PluginSetting;
        return this;
    }

    public DesignAutomationSettingBuilder ConfigurePluginPlugin(Action<PluginDescriptor> configPlugin)
    {
        var pluginDescriptor = new PluginDescriptor();
        configPlugin(pluginDescriptor);
        _designAutomationExecutionSetting.PluginSetting = pluginDescriptor.PluginSetting;
        return this;
    }

    public DesignAutomationSettingBuilder ConfigureStoragePreferences(Action<StoragePreferences> configPreferences)
    {
        var storagePreferences = new StoragePreferences();
        configPreferences(storagePreferences);
        _designAutomationExecutionSetting.StoragePreferences = storagePreferences;
        return this;
    }

    public DesignAutomationSettingBuilder AssignAppBundlePreferenceProvider(INamedAliasPreferenceProvider appBundlePreferenceProvider)
    {
        _designAutomationExecutionSetting.AppBundlePreferenceProvider = appBundlePreferenceProvider ?? throw new ArgumentNullException(nameof(appBundlePreferenceProvider));
        return this;
    }

    public DesignAutomationSettingBuilder AssignActivityPreferenceProvider(INamedAliasPreferenceProvider activityPreferenceProvider)
    {
        _designAutomationExecutionSetting.ActivityPreferenceProvider = activityPreferenceProvider ?? throw new ArgumentNullException(nameof(activityPreferenceProvider));
        return this;
    }

    public DesignAutomationSettingBuilder ConfigureWorkItemPreferences(IWorkItemPreferenceConfigurationComponent workItemPreferenceConfigurationComponent)
    {
        var preferencesDescriptor = new WorkItemPreferencesDescriptor();
        workItemPreferenceConfigurationComponent.Configure(preferencesDescriptor);
        _designAutomationExecutionSetting.WorkItemPreferences = preferencesDescriptor.WorkItemPreferences;
        return this;
    }

    public DesignAutomationSettingBuilder ConfigureWorkItemPreferences(Action<WorkItemPreferencesDescriptor> configDescriptor)
    {
        var preferencesDescriptor = new WorkItemPreferencesDescriptor();
        configDescriptor(preferencesDescriptor);
        _designAutomationExecutionSetting.WorkItemPreferences = preferencesDescriptor.WorkItemPreferences;
        return this;
    }

    public DesignAutomationExecutionSetting Build()
    {
        return _designAutomationExecutionSetting;
    }
}
