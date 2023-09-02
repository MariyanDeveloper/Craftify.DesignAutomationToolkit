using Autodesk.Forge.DesignAutomation.Model;
using Craftify.DesignAutomationToolkit.Builders;
using Craftify.DesignAutomationToolkit.Console.SetupComponents;
using Craftify.DesignAutomationToolkit.IntegrationTests.Helpers;
using Craftify.DesignAutomationToolkit.Interfaces;
using Craftify.DesignAutomationToolkit.Preferences;
using Craftify.DesignAutomationToolkit.Providers;
using Craftify.DesignAutomationToolkit.Settings.Components.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Craftify.DesignAutomationToolkit.IntegrationTests;

public class DesignAutomationExecutionTests
{
    public DesignAutomationExecutionTests()
    {
        Environment.SetEnvironmentVariable("DOTNET_ENVIRONMENT", "Development");
    }
    
    [Fact]
    public async Task Execute_WithScenario_OneInputRvt_OneOutputRvt_UsingSettingBuilder_DeletionWallsPlugin_ReturnsSuccessfulStatus()
    {
        var serviceProvider = DependencyInjectionHelper.GetServiceProvider();
        var parametersPluginSetupComponent = new ParametersPluginConfigurationComponent();
        var pluginPathSetupComponent = new PluginPathConfigurationComponent();
        var chainedPluginSetupComponent = parametersPluginSetupComponent
            .ChainWith(pluginPathSetupComponent);
        var workItemPreferencesSetupComponent = new WorkItemPreferenceConfigurationSetupComponent();
        var executionSettings = DesignAutomationSettingBuilder
            .Create()
            .ConfigurePlugin(chainedPluginSetupComponent)
            .ConfigureWorkItemPreferences(workItemPreferencesSetupComponent)
            .ConfigureStoragePreferences(storage =>
            {
                storage.BucketIdPreference = new NamedIdPreference("mariyan_dev_test_bucket_for_execution");
            })
            .AssignActivityPreferenceProvider(new NamedAliasPreferenceProvider("mariyan_dev_activity",
                "mariyanDevActivityAlias"))
            .AssignAppBundlePreferenceProvider(new NamedAliasPreferenceProvider("mariyan_dev_appBundle",
                "mariyanDevAppBundleAlias"))
            .Build();
        var execution = serviceProvider.GetRequiredService<IDesignAutomationExecution>();
        var executionResult = await execution.Execute(executionSettings);
        
        var expectedStatus = Status.Success;
        var actualStatus = executionResult.Result.WorkItemExecutionResult.Result.WorkItemStatus.Status;
        
        Assert.True(expectedStatus == actualStatus);
    }
    
    [Fact]
    public async Task Execute_WithScenario_OneInputRvt_OneOutputRvt_UsingSettingDescriptor_DeletionWallsPlugin_ReturnsSuccessfulStatus()
    {
        var serviceProvider = DependencyInjectionHelper.GetServiceProvider();
        var parametersPluginSetupComponent = new ParametersPluginConfigurationComponent();
        var pluginPathSetupComponent = new PluginPathConfigurationComponent();
        var chainedPluginSetupComponent = parametersPluginSetupComponent
            .ChainWith(pluginPathSetupComponent);
        var workItemPreferencesSetupComponent = new WorkItemPreferenceConfigurationSetupComponent();

        var execution = serviceProvider.GetRequiredService<IDesignAutomationExecution>();
        var executionResult = await execution.Execute(setupDescriptor =>
            setupDescriptor
                .ConfigurePlugin(chainedPluginSetupComponent)
                .ConfigureWorkItemPreferences(workItemPreferencesSetupComponent)
                .ConfigureStoragePreferences(storage =>
                {
                    storage.BucketIdPreference = new NamedIdPreference("mariyan_dev_test_bucket_for_execution");
                })
                .AssignActivityPreferenceProvider(new NamedAliasPreferenceProvider("mariyan_dev_activity", "mariyanDevActivityAlias"))
                .AssignAppBundlePreferenceProvider(new NamedAliasPreferenceProvider("mariyan_dev_appBundle", "mariyanDevAppBundleAlias")));
        var expectedStatus = Status.Success;
        var actualStatus = executionResult.Result.WorkItemExecutionResult.Result.WorkItemStatus.Status;
        
        Assert.True(expectedStatus == actualStatus);
    }
}