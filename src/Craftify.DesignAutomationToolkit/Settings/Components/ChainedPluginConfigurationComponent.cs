using Craftify.DesignAutomationToolkit.Settings.Components.Interfaces;
using Craftify.DesignAutomationToolkit.Settings.Descriptors;

namespace Craftify.DesignAutomationToolkit.Settings.Components;

public class ChainedPluginConfigurationComponent : IPluginConfigurationComponent
{
    private readonly IPluginConfigurationComponent _fromComponent;
    private readonly IPluginConfigurationComponent _toComponent;

    public ChainedPluginConfigurationComponent(IPluginConfigurationComponent fromComponent, IPluginConfigurationComponent toComponent)
    {
        _fromComponent = fromComponent ?? throw new ArgumentNullException(nameof(fromComponent));
        _toComponent = toComponent ?? throw new ArgumentNullException(nameof(toComponent));
    }
    public void Configure(PluginDescriptor pluginDescriptor)
    {
        _fromComponent.Configure(pluginDescriptor);
        _toComponent.Configure(pluginDescriptor);
    }
}