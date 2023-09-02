using Craftify.DesignAutomationToolkit.Activators.Handlers;
using Craftify.DesignAutomationToolkit.Factories;
using Craftify.DesignAutomationToolkit.Handlers;
using Craftify.DesignAutomationToolkit.Interfaces;
using Craftify.DesignAutomationToolkit.Tests.Helpers.ObjectService;
using Craftify.ObjectStorageServiceToolkit.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Craftify.DesignAutomationToolkit.Tests.Helpers;

public static class DependencyInjectionHelper
{
    private static IServiceProvider? _handlerFactoryProvider;
    private static IServiceProvider? _workItemExecutionFactoryProvider;
    public static IServiceProvider GetSettingHandlerFactoryProvider(ObjectServiceMockSettings? objectServiceMockSettings = null)
    {
        if (_handlerFactoryProvider is not null)
        {
            return _handlerFactoryProvider;
        }
        var objectServiceMock = MockBuilder.CreateObjectServiceMock(objectServiceMockSettings);
        var services = new ServiceCollection();
        services.AddTransient<InputArgumentSettingHandler>();
        services.AddTransient<OutputArgumentSettingHandler>();
        services.AddTransient<ISettingHandlerActivator, InputHandlerActivator>();
        services.AddTransient<ISettingHandlerActivator, OutputHandlerActivator>();
        services.AddTransient<ISettingHandlerFactory, SettingHandlerFactory>();
        services.AddSingleton<IObjectService>(_ => objectServiceMock.Object);
        _handlerFactoryProvider = services.BuildServiceProvider();
        return _handlerFactoryProvider;
    }

    // public static IServiceProvider GetWorkItemExecutionFactoryProvider()
    // {
    //     if (_workItemExecutionFactoryProvider is not null)
    //     {
    //         return _workItemExecutionFactoryProvider;
    //     }
    //     var services = new ServiceCollection();
    //     var workItemsApiMock = MockBuilder.CreateWorkItemsApiMockOnThirdTimeSuccessful();
    //     services.AddTransient<ITimedExecutionResultMeasurement, TimedExecutionResultMeasurement>();
    //     services.AddTransient<IKeyGenerator, UniqueTrackingKeyGenerator>();
    //     services.AddTransient<IWorkItemsApi>(_ => workItemsApiMock.Object);
    //     services.AddTransient<IWorkItemExecutionActivator, CallbackWorkItemExecutionActivator>();
    //     services.AddTransient<IWorkItemExecutionActivator, PollingWorkItemExecutionActivator>();
    //     services.AddTransient<IWorkItemExecutionFactory, WorkItemExecutionFactory>();
    //     _workItemExecutionFactoryProvider = services.BuildServiceProvider();
    //     return _workItemExecutionFactoryProvider;
    //
    // }
}