using Autodesk.Forge.DesignAutomation.Model;
using Craftify.DesignAutomationToolkit.Interfaces;
using Craftify.DesignAutomationToolkit.Providers;
using Craftify.DesignAutomationToolkit.Tests.Constants;
using Craftify.DesignAutomationToolkit.Tests.Fixtures;
using Craftify.DesignAutomationToolkit.Tests.Helpers;
using Craftify.DesignAutomationToolkit.Tests.Helpers.ObjectService;
using Craftify.DesignAutomationToolkit.Tests.Providers.EqualityComparers;
using Microsoft.Extensions.DependencyInjection;

namespace Craftify.DesignAutomationToolkit.Tests.Providers;

public class WorkItemProviderTests : IClassFixture<DesignAutomationSettingFixture>
{
    private readonly DesignAutomationSettingFixture _designAutomationSettingFixture;

    public WorkItemProviderTests(DesignAutomationSettingFixture designAutomationDesignAutomationSettingFixture)
    {
        _designAutomationSettingFixture = designAutomationDesignAutomationSettingFixture;
    }

    [Fact]
    public async Task Get_WithValidSettings_ReturnsExpectedActivity()
    {
        var serviceProvider = DependencyInjectionHelper.GetSettingHandlerFactoryProvider(new ObjectServiceMockSettings()
        {
            InputUrl = ValidUrls.ExampleUrl,
            OutputUrl = ValidUrls.GoogleUrl
        });
        var settingHandlerFactory = serviceProvider.GetRequiredService<ISettingHandlerFactory>();
        var nicknameProviderMock = MockBuilder.CreateNicknameProviderMock();
        var workItemProvider = new WorkItemProvider(
            nicknameProviderMock.Object,
            settingHandlerFactory);
        var designAutomationSetting = _designAutomationSettingFixture.DesignAutomationExecutionSetting;
        var workItem = await workItemProvider.Get(designAutomationSetting);
        var expectedParamFileUrl = """
                data:application/json,{"Height":20,"Width":10}
                """;

        var actualArguments = workItem.Arguments;
        var expectedArguments = new Dictionary<string, IArgument>()
        {
            ["rvtFile"] = new XrefTreeArgument()
            {
                Url = ValidUrls.ExampleUrl,
                Verb = Verb.Get
            },
            ["paramFile"] = new XrefTreeArgument()
            {
                Url = expectedParamFileUrl,
                Verb = Verb.Get
            },
            ["result"] = new XrefTreeArgument()
            {
                Url = ValidUrls.GoogleUrl,
                Verb = Verb.Put
            }
        };
        Assert.Equal(
            expectedArguments,
            actualArguments,
            new ArgumentDictionaryComparer(new ArgumentEqualityComparer()));
    }
}