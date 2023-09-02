using Autodesk.Forge.Core;
using Autodesk.Forge.DesignAutomation;
using Autodesk.Forge.DesignAutomation.Http;
using Craftify.AutodeskAuthenticationToolkit;
using Craftify.AutodeskAuthenticationToolkit.MicrosoftDependencyInjection;
using Craftify.DesignAutomationToolkit.Activators.Executions;
using Craftify.DesignAutomationToolkit.Activators.Handlers;
using Craftify.DesignAutomationToolkit.Executions;
using Craftify.DesignAutomationToolkit.Factories;
using Craftify.DesignAutomationToolkit.Interfaces;
using Craftify.DesignAutomationToolkit.KeyGenerators;
using Craftify.DesignAutomationToolkit.Measurements;
using Craftify.DesignAutomationToolkit.Providers;
using Craftify.DesignAutomationToolkit.Publishers;
using Craftify.DesignAutomationToolkit.Services;
using Craftify.ObjectStorageServiceToolkit.MicrosoftDependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Craftify.DesignAutomationToolkit.MicrosoftDependencyInjection;

public static class DependencyInjection
{
    public static void AddDesignAutomationToolkit(
        this IServiceCollection serviceCollection,
        Lazy<AuthCredentials> authCredentials)
    {
        serviceCollection.AddTransient<AuthCredentials>(_ => authCredentials.Value);
        serviceCollection.AddTransient<ForgeConfiguration>(_ => new ForgeConfiguration()
        {
            ClientId = authCredentials.Value.ClientId,
            ClientSecret = authCredentials.Value.ClientSecret
        });
        serviceCollection.AddAutodeskAuthenticationToolkit(authCredentials);
        serviceCollection.AddObjectStorageServiceToolkit();
        serviceCollection.AddForgeService();
        serviceCollection.AddDesignAutomationApis();
        serviceCollection.AddServices();
        serviceCollection.AddProviders();
        serviceCollection.AddFactories();
        serviceCollection.AddActivators();
        serviceCollection.AddExecutions();
        serviceCollection.AddTransient<ITimedExecutionResultMeasurement, TimedExecutionResultMeasurement>();
        serviceCollection.AddTransient<IKeyGenerator, UniqueTrackingKeyGenerator>();
        serviceCollection.AddPublishers();
    }

    private static void AddExecutions(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddTransient<IDesignAutomationExecution, DesignAutomationExecution>();
    }
    private static void AddProviders(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddTransient<INicknameProvider, NicknameProvider>();
        serviceCollection.AddTransient<IActivityProvider, ActivityProvider>();
        serviceCollection.AddTransient<IWorkItemProvider, WorkItemProvider>();
    }
    
    private static void AddFactories(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddTransient<ISettingHandlerFactory, SettingHandlerFactory>();
        serviceCollection.AddTransient<IWorkItemExecutionFactory, WorkItemExecutionFactory>();
    }

    private static void AddActivators(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSettingHandlerActivators();
        serviceCollection.AddWorkItemExecutionActivators();
    }
    private static void AddWorkItemExecutionActivators(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddTransient<IWorkItemExecutionActivator, PollingWorkItemExecutionActivator>();
        serviceCollection.AddTransient<IWorkItemExecutionActivator, CallbackWorkItemExecutionActivator>();
    }
    private static void AddSettingHandlerActivators(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddTransient<ISettingHandlerActivator, InputHandlerActivator>();
        serviceCollection.AddTransient<ISettingHandlerActivator, OutputHandlerActivator>();
    }
    private static void AddServices(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddTransient<IActivityService, ActivityService>();
        serviceCollection.AddTransient<IAppBundleService, AppBundleService>();
    }
    private static void AddDesignAutomationApis(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddTransient<DesignAutomationClient>();
        serviceCollection.AddTransient<IWorkItemsApi, WorkItemsApi>();
        serviceCollection.AddTransient<IActivitiesApi, ActivitiesApi>();
        serviceCollection.AddTransient<IAppBundlesApi, AppBundlesApi>();
        serviceCollection.AddTransient<IForgeAppsApi, ForgeAppsApi>();
        serviceCollection.AddTransient<DesignAutomationClient>();
    }
    private static void AddPublishers(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddTransient<IActivityPublisher, ActivityPublisher>();
        serviceCollection.AddTransient<IAppBundlePublisher, AppBundlePublisher>();
    }

    private static void AddForgeService(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddHttpClient<ForgeService>(_ => { })
            .ConfigurePrimaryHttpMessageHandler(provider =>
            {
                var forgeConfiguration = Options.Create(provider.GetRequiredService<ForgeConfiguration>());
                return new ForgeHandler(forgeConfiguration)
                {
                    InnerHandler = new HttpClientHandler()
                };
            });
    }
}