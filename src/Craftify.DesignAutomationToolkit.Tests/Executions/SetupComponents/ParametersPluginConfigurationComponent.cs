using Craftify.DesignAutomationToolkit.Settings.Components.Interfaces;
using Craftify.DesignAutomationToolkit.Settings.Descriptors;
using Craftify.DesignAutomationToolkit.Tests.Settings;

namespace Craftify.DesignAutomationToolkit.Tests.Executions.SetupComponents;

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
                            .OfValue("path.rvt")
                            .WithLocalName("local.rvt")
                            .Required();
                    })
                    .Input(descriptor =>
                    {
                        descriptor
                            .WithKey("revitParams")
                            .OfValue(new Entity()
                            {
                                Name = "Mariyan"
                            })
                            .WithLocalName("params.json")
                            .Required();
                    })
                    .Output(descriptor =>
                    {
                        descriptor
                            .WithKey("result")
                            .WithLocalName("output.rvt")
                            .OfDescription("some description");
                    });;
            });
    }
}