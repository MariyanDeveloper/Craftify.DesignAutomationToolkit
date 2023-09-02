using Craftify.DesignAutomationToolkit.Settings.Components.Interfaces;

namespace Craftify.DesignAutomationToolkit.Settings.Components.Extensions;

public static class PluginSetupComponentExtensions
{
    public static IPluginConfigurationComponent ChainWith(this IPluginConfigurationComponent from, IPluginConfigurationComponent next)
    {
        if (from is null) throw new ArgumentNullException(nameof(from));
        if (next is null) throw new ArgumentNullException(nameof(next));
        return new ChainedPluginConfigurationComponent(from, next);
    }
}