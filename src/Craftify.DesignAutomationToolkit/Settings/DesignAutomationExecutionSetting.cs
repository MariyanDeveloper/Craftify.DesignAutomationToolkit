using Craftify.DesignAutomationToolkit.Interfaces;

namespace Craftify.DesignAutomationToolkit.Settings;

public class DesignAutomationExecutionSetting
{
    public PluginSetting PluginSetting { get; set; }
    public StoragePreferences StoragePreferences { get; set; }
    public WorkItemPreferences WorkItemPreferences { get; set; }
    public INamedAliasPreferenceProvider AppBundlePreferenceProvider { get; set; }
    public INamedAliasPreferenceProvider ActivityPreferenceProvider { get; set; }
}