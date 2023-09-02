using Craftify.DesignAutomationToolkit.Settings.Components.Interfaces;
using Craftify.DesignAutomationToolkit.Settings.Descriptors;

namespace Craftify.DesignAutomationToolkit.Console.SetupComponents;

public class PluginPathConfigurationComponent : IPluginConfigurationComponent
{
    public void Configure(PluginDescriptor pluginDescriptor)
    {
        pluginDescriptor.WithPackagePath("AppBundles/DeleteWalls.zip");
    }
}