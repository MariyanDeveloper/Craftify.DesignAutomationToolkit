using Craftify.DesignAutomationToolkit.Builders;

namespace Craftify.DesignAutomationToolkit.Settings.Descriptors;

public class PluginDescriptor
{
    internal PluginSetting PluginSetting { get; set; } = new();

    public PluginDescriptor WithPackagePath(string packagedPluginPath)
    {
        PluginSetting.Path = packagedPluginPath ?? throw new ArgumentNullException(nameof(packagedPluginPath));
        return this;
    }
    public PluginDescriptor DescribeParameters(Action<SettingsBuilder> settingsDescriptor)
    {
        var settingsBuilder = new SettingsBuilder();
        settingsDescriptor(settingsBuilder);
        PluginSetting.ParameterSettings = settingsBuilder.Build();
        return this;
    }

    public PluginDescriptor OfProduct(ProductSpecification product)
    {
        PluginSetting.ProductSpecification = product;
        return this;
    }

    
}