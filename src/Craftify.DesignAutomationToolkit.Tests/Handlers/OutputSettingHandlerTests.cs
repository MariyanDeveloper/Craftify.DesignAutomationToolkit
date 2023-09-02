using Autodesk.Forge.DesignAutomation.Model;
using Craftify.DesignAutomationToolkit.Handlers;
using Craftify.DesignAutomationToolkit.Settings;
using Craftify.DesignAutomationToolkit.Tests.Constants;
using Craftify.DesignAutomationToolkit.Tests.Helpers;
using Craftify.DesignAutomationToolkit.Tests.Helpers.ObjectService;

namespace Craftify.DesignAutomationToolkit.Tests.Handlers;

public class OutputSettingHandlerTests
{
    private static ObjectServiceMockSettings _objectMockSettingsWithValidUrl = new ()
    {
        OutputUrl = ValidUrls.GoogleUrl
    };
    private static ObjectServiceMockSettings _objectMockSettingsWithInvalidUrl = new ()
    {
        OutputUrl = "Invalid"
    };
    
    [Fact]
    public async Task Handle_WhenUrlIsValid_ReturnsExpectedResult()
    {
        var objectServiceMock = MockBuilder.CreateObjectServiceMock(_objectMockSettingsWithValidUrl);
        var localName = "result.rvt";
        var outputSetting = new OutputSetting()
        {
            LocalName = localName,
        };
        var outputSettingHandler = new OutputArgumentSettingHandler(objectServiceMock.Object);
        var result = await outputSettingHandler.Handle(outputSetting, new HandlerStorageDetails("bucketId"));
        var argument = result.Argument as XrefTreeArgument;
        
        var expectedVerb = Verb.Put;
        var expectedLocalName = localName;
        var expectedUrl = ValidUrls.GoogleUrl;

        var isValid = argument.Verb == expectedVerb &&
                      argument.Url == expectedUrl;
        Assert.True(isValid);

    }
    
    [Fact]
    public async Task Handle_WhenUrlIsInvalid_ReturnsExpectedResult()
    {
        var objectServiceMock = MockBuilder.CreateObjectServiceMock(_objectMockSettingsWithInvalidUrl);
        var outputSetting = new OutputSetting()
        {
            LocalName = "result.rvt",
        };
        var outputSettingHandler = new OutputArgumentSettingHandler(objectServiceMock.Object);
        Assert.ThrowsAsync<InvalidOperationException>(async () =>
            await outputSettingHandler.Handle(outputSetting, new HandlerStorageDetails("bucketId")));
    }
}