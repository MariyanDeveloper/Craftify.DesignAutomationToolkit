// See https://aka.ms/new-console-template for more information

using Craftify.AutodeskAuthenticationToolkit;
using Craftify.DesignAutomationToolkit.Actions.Interfaces;
using Craftify.DesignAutomationToolkit.Console;
using Craftify.DesignAutomationToolkit.Console.Constants;
using Craftify.DesignAutomationToolkit.Console.SetupComponents;
using Craftify.DesignAutomationToolkit.Interfaces;
using Craftify.DesignAutomationToolkit.MicrosoftDependencyInjection;
using Craftify.DesignAutomationToolkit.Models;
using Craftify.DesignAutomationToolkit.Preferences;
using Craftify.DesignAutomationToolkit.Providers;
using Craftify.DesignAutomationToolkit.Services;
using Craftify.DesignAutomationToolkit.Settings.Components.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

AssignEnvironmentToDevelopment();
var host = Host.CreateDefaultBuilder()
    .ConfigureServices((context, services) =>
    {
        var configuration = context.Configuration; 
        services.Configure<ReportFileConfiguration>(
            configuration.GetSection("ReportFileSettings"));

        services.Configure<LocalFileConfiguration>(
            configuration.GetSection("LocalFileConfigurations"));
                
        services.AddTransient<LocalFileConfiguration>(provider =>
            provider.GetRequiredService<IOptions<LocalFileConfiguration>>().Value);

        services.AddTransient<ReportFileConfiguration>(provider =>
            provider.GetRequiredService<IOptions<ReportFileConfiguration>>().Value);
        
        services.AddHttpClient<IWorkItemExecutedAction, ReportWorkItemExecutedAction>();
        services.AddHttpClient<IFileDownloadService, FileDownloadService>();
        services.AddDesignAutomationToolkit(new Lazy<AuthCredentials>(() =>
        {
            var clientId = configuration[AutodeskPlatformService.ClientId];
            var clientSecret = configuration[AutodeskPlatformService.ClientSecret];
            if (clientId is null || clientSecret is null)
            {
                throw new InvalidOperationException($"You must provide client id and client secret");
            }
            return new AuthCredentials(clientId, clientSecret);
        }));
    })
    .Build();
var parametersPluginSetupComponent = new ParametersPluginConfigurationComponent();
var pluginPathSetupComponent = new PluginPathConfigurationComponent();
var chainedPluginSetupComponent = parametersPluginSetupComponent
    .ChainWith(pluginPathSetupComponent);
var workItemPreferencesSetupComponent = new WorkItemPreferenceConfigurationSetupComponent();
var designAutomationExecution = host.Services.GetRequiredService<IDesignAutomationExecution>();
var executionResult = await designAutomationExecution.Execute(setupDescriptor =>
    setupDescriptor
        .ConfigurePlugin(chainedPluginSetupComponent)
        .ConfigureWorkItemPreferences(workItemPreferencesSetupComponent)
        .ConfigureStoragePreferences(storage =>
        {
            storage.BucketIdPreference = new NamedIdPreference("mariyan_dev_test_bucket_for_execution");
        })
        .AssignActivityPreferenceProvider(new NamedAliasPreferenceProvider("mariyan_dev_activity", "mariyanDevActivityAlias"))
        .AssignAppBundlePreferenceProvider(new NamedAliasPreferenceProvider("mariyan_dev_appBundle", "mariyanDevAppBundleAlias")));
// var b = 5;
Console.WriteLine("Finished");
void AssignEnvironmentToDevelopment()
{
    Environment.SetEnvironmentVariable("DOTNET_ENVIRONMENT", "Development");
}