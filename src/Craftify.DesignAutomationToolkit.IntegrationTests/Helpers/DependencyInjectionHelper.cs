using Craftify.AutodeskAuthenticationToolkit;
using Craftify.DesignAutomationToolkit.Actions.Interfaces;
using Craftify.DesignAutomationToolkit.Console;
using Craftify.DesignAutomationToolkit.IntegrationTests.Constants;
using Craftify.DesignAutomationToolkit.Interfaces;
using Craftify.DesignAutomationToolkit.MicrosoftDependencyInjection;
using Craftify.DesignAutomationToolkit.Models;
using Craftify.DesignAutomationToolkit.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Craftify.DesignAutomationToolkit.IntegrationTests.Helpers;

public class DependencyInjectionHelper
{
    public static IServiceProvider GetServiceProvider()
    {
        var builder = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory) // Use the project's base directory as the base path
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true) // Add the appsettings.json file
            .AddUserSecrets<DesignAutomationExecutionTests>()
            .AddEnvironmentVariables();
        var configuration = builder.Build();

        var services = new ServiceCollection();
        services.AddSingleton<IConfiguration>(configuration);

        services.Configure<ReportFileConfiguration>(
            configuration.GetSection(ConfigurationSections.ReportFileSettings));

        services.Configure<LocalFileConfiguration>(
            configuration.GetSection(ConfigurationSections.LocalFileConfigurations));
                
        services.AddTransient<LocalFileConfiguration>(provider =>
            provider.GetRequiredService<IOptions<LocalFileConfiguration>>().Value);

        services.AddTransient<ReportFileConfiguration>(provider =>
            provider.GetRequiredService<IOptions<ReportFileConfiguration>>().Value);
        
        services.AddHttpClient<IWorkItemExecutedAction, ReportWorkItemExecutedAction>();
        services.AddTransient<IWorkItemExecutedAction, DownloadFileLocallyAction>();
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
        return services.BuildServiceProvider();
    }
}