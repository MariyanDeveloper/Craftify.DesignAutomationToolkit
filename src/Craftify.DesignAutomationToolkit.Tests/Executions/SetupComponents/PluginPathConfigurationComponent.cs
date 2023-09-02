using Craftify.DesignAutomationToolkit.Settings.Components.Interfaces;
using Craftify.DesignAutomationToolkit.Settings.Descriptors;
using Craftify.DesignAutomationToolkit.Tests.Constants;

namespace Craftify.DesignAutomationToolkit.Tests.Executions.SetupComponents;

public class PluginPathConfigurationComponent : IPluginConfigurationComponent
{
    public void Configure(PluginDescriptor pluginDescriptor)
    {
        pluginDescriptor.WithPackagePath(ValidFilePaths.RevitProject);
    }
}