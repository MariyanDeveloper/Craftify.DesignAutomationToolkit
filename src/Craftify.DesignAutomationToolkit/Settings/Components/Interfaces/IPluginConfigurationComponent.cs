using Craftify.DesignAutomationToolkit.Settings.Descriptors;

namespace Craftify.DesignAutomationToolkit.Settings.Components.Interfaces;

public interface IPluginConfigurationComponent
{
    void Configure(PluginDescriptor pluginDescriptor);
}