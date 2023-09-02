using Craftify.DesignAutomationToolkit.Activators.Handlers;
using Craftify.DesignAutomationToolkit.Factories;
using Craftify.DesignAutomationToolkit.Handlers;
using Craftify.DesignAutomationToolkit.Interfaces;
using Craftify.DesignAutomationToolkit.Settings;
using Craftify.DesignAutomationToolkit.Tests.Constants;
using Craftify.DesignAutomationToolkit.Tests.Helpers;
using Craftify.DesignAutomationToolkit.Tests.Helpers.ObjectService;
using Craftify.ObjectStorageServiceToolkit.Interfaces;

namespace Craftify.DesignAutomationToolkit.Tests.Factories;

public class SettingHandlerFactoryTests
{
    private readonly IObjectService _objectService;
    private readonly ISettingHandlerFactory _factoryWithAllActivators;

    public SettingHandlerFactoryTests()
    {
        var objectServiceMock = MockBuilder.CreateObjectServiceMock(new ObjectServiceMockSettings()
        {
            InputUrl = ValidUrls.GoogleUrl,
            OutputUrl = ValidUrls.ExampleUrl
        });
        _objectService = objectServiceMock.Object;
        _factoryWithAllActivators = new SettingHandlerFactory(new ISettingHandlerActivator[]
        {
            new InputHandlerActivator(_objectService),
            new OutputHandlerActivator(_objectService)
        });
 
    }
    [Fact]
    public void Create_WhenInputTypeIsPassed_ReturnsInputSettingHandler()
    {
        var factory = _factoryWithAllActivators;
        var inputHandler = factory.Create(typeof(InputSetting));
        
        Assert.IsType<InputArgumentSettingHandler>(inputHandler);
    }
    
    [Fact]
    public void Create_WhenOutputTypeIsPassed_ReturnsOutputSettingHandler()
    {
        var factory = _factoryWithAllActivators;

        var outputHandler = factory.Create(typeof(OutputSetting));
        
        Assert.IsType<OutputArgumentSettingHandler>(outputHandler);
    }
    
    [Fact]
    public void Create_WhenNotAssignableTypeIsPassed_ThrowsArgumentException()
    {
        var factory = _factoryWithAllActivators;

        Assert.Throws<ArgumentException>(() => factory.Create(typeof(string)));
        
    }
    
    [Fact]
    public void Create_WhenNullIsPassed_ThrowsArgumentNullException()
    {
        var factory = _factoryWithAllActivators;

        Assert.Throws<ArgumentNullException>(() => factory.Create(null));
        
    }
    
    [Fact]
    public void Create_WhenThereIsNoActivatorForType_InvalidOperationException()
    {

        var factory = new SettingHandlerFactory(new ISettingHandlerActivator[]
        {
            new InputHandlerActivator(_objectService)
        });
        
        Assert.Throws<InvalidOperationException>(() => factory.Create(typeof(OutputSetting)));
    }
    
}