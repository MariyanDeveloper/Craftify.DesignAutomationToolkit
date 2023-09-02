using Craftify.DesignAutomationToolkit.Settings.Components.Interfaces;
using Craftify.DesignAutomationToolkit.Settings.Descriptors;

namespace Craftify.DesignAutomationToolkit.Console.SetupComponents;

public class ParametersPluginConfigurationComponent : IPluginConfigurationComponent
{
    public void Configure(PluginDescriptor pluginDescriptor)
    {
        pluginDescriptor
            .DescribeParameters(settingDescriptor =>
            {
                settingDescriptor
                    .Input(descriptor =>
                    {
                        descriptor
                            .WithKey("rvtFile")
                            .OfValue("SampleFiles/DeleteWalls2023.rvt")
                            .WithLocalName("local.rvt")
                            .Required();
                    })
                    .Output(descriptor =>
                    {
                        descriptor
                            .WithKey("result")
                            .WithLocalName("result.rvt")
                            .OfDescription("some description");
                    });;
            });
    }
}