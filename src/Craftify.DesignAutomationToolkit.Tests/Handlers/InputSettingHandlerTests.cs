using Autodesk.Forge.DesignAutomation.Model;
using Craftify.DesignAutomationToolkit.Handlers;
using Craftify.DesignAutomationToolkit.Settings;
using Craftify.DesignAutomationToolkit.Tests.Constants;
using Craftify.DesignAutomationToolkit.Tests.Helpers;
using Craftify.DesignAutomationToolkit.Tests.Helpers.ObjectService;

namespace Craftify.DesignAutomationToolkit.Tests.Handlers;

public class InputSettingHandlerTests
{
    private static ObjectServiceMockSettings _objectMockSettingsWithValidUrl = new ()
    {
        InputUrl = ValidUrls.ExampleUrl
    };
    
    [Fact]
    public async Task Handle_WhenValueIsNumber_ReturnsExpectedArgument()
    {
        var objectServiceMock = MockBuilder.CreateObjectServiceMock();
        var inputSetting = new InputSetting()
        {
            LocalName = "someLocal",
            Value = 25
        };
        var inputSettingHandler = new InputArgumentSettingHandler(objectServiceMock.Object);
        var result = await inputSettingHandler
            .Handle(inputSetting, new HandlerStorageDetails(""));
        var argument = result.Argument as XrefTreeArgument;
        
        var expectedUrl = "data:application/json,25";
        var expectedVerb = Verb.Get;

        var isValid = argument.Url == expectedUrl &&
                      argument.Verb == expectedVerb;
        Assert.True(isValid);
    }
    
    
    [Fact]
    public async Task Handle_WhenValueIsObject_ReturnsExpectedArgument()
    {
        var objectServiceMock = MockBuilder.CreateObjectServiceMock();
        var inputSetting = new InputSetting()
        {
            LocalName = "someLocal",
            Value = new
            {
                Height = 20,
                Width = 10
            }
        };
        var inputSettingHandler = new InputArgumentSettingHandler(objectServiceMock.Object);
        var result = await inputSettingHandler
            .Handle(inputSetting, new HandlerStorageDetails(""));
        
        var argument = result.Argument as XrefTreeArgument;
        var expectedUrl = """
                data:application/json,{"Height":20,"Width":10}
                """;
        var expectedVerb = Verb.Get;

        var isValid = argument.Url == expectedUrl &&
                      argument.Verb == expectedVerb;
        Assert.True(isValid);
    }
    
    [Fact]
    public async Task Handle_WhenValueIsFilePathWithInvalidSignedUrl_ReturnsExpectedArgument()
    {
        var invalidUrl = "invalidUrl";
        var objectServiceMock = MockBuilder.CreateObjectServiceMock();
        var validFilePath = ValidFilePaths.RevitProject;
        var inputSetting = new InputSetting()
        {
            LocalName = "someLocal",
            Value = validFilePath
        };
        var inputSettingHandler = new InputArgumentSettingHandler(objectServiceMock.Object);
        var result = await inputSettingHandler
            .Handle(inputSetting, new HandlerStorageDetails("bucketId"));
        
        var argument = result.Argument as XrefTreeArgument;
        var expectedUrl = $"""
                          data:application/json,"{validFilePath}"
                          """;
        var expectedVerb = Verb.Get;
        
        var isValid = argument.Url == expectedUrl &&
                      argument.Verb == expectedVerb;
        Assert.True(isValid);
    }
    
    [Fact]
    public async Task Handle_WhenValueIsFilePathWithValidSignedUrl_ReturnsExpectedArgument()
    {
        var validUrl = ValidUrls.ExampleUrl;
        var objectServiceMock = MockBuilder.CreateObjectServiceMock(_objectMockSettingsWithValidUrl);
        var validFilePath = ValidFilePaths.RevitProject;
        var inputSetting = new InputSetting()
        {
            LocalName = "someLocal",
            Value = validFilePath
        };
        var inputSettingHandler = new InputArgumentSettingHandler(objectServiceMock.Object);
        var result = await inputSettingHandler
            .Handle(inputSetting, new HandlerStorageDetails(""));
        var argument = result.Argument as XrefTreeArgument;
        
        var expectedUrl = validUrl;
        var expectedVerb = Verb.Get;

        var isValid = argument.Url == expectedUrl &&
                      argument.Verb == expectedVerb;
        Assert.True(isValid);
    }
    
    [Fact]
    public async Task Handle_WhenValueIsNull_ReturnsExpectedArgument()
    {
        var objectServiceMock = MockBuilder.CreateObjectServiceMock();
        var inputSetting = new InputSetting()
        {
            LocalName = "someLocal",
            Value = null,
        };
        var inputSettingHandler = new InputArgumentSettingHandler(objectServiceMock.Object);

        Assert.ThrowsAsync<NullReferenceException>(async () => await inputSettingHandler
            .Handle(inputSetting, new HandlerStorageDetails("")));
    }
}