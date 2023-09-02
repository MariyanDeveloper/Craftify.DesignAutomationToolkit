using Craftify.DesignAutomationToolkit.Builders;

namespace Craftify.DesignAutomationToolkit.Tests.Settings;

public class Entity
{
    public string Name { get; set; }
}
public class SettingsBuilderTests
{
    [Fact]
    public void Build_WhenInputAndOutputCreated_ReturnsExpectedAmountOfSettings()
    {
        var settings = new SettingsBuilder()
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
            })
            .Build();

        var expectedCount = 3;
        Assert.True(settings.Count == expectedCount);
    }
}