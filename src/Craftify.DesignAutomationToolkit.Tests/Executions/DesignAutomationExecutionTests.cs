// using Autodesk.Forge.DesignAutomation.Model;
// using Craftify.DesignAutomationToolkit.Activators.Executions;
// using Craftify.DesignAutomationToolkit.Activators.Handlers;
// using Craftify.DesignAutomationToolkit.Executions;
// using Craftify.DesignAutomationToolkit.Extensions.SearchResults;
// using Craftify.DesignAutomationToolkit.Factories;
// using Craftify.DesignAutomationToolkit.Interfaces;
// using Craftify.DesignAutomationToolkit.KeyGenerators;
// using Craftify.DesignAutomationToolkit.Measurements;
// using Craftify.DesignAutomationToolkit.Preferences;
// using Craftify.DesignAutomationToolkit.Providers;
// using Craftify.DesignAutomationToolkit.Publishers;
// using Craftify.DesignAutomationToolkit.Settings.Components.Extensions;
// using Craftify.DesignAutomationToolkit.Tests.Constants;
// using Craftify.DesignAutomationToolkit.Tests.Executions.SetupComponents;
// using Craftify.DesignAutomationToolkit.Tests.Helpers;
// using Craftify.DesignAutomationToolkit.Tests.Helpers.Activity;
// using Craftify.DesignAutomationToolkit.Tests.Helpers.AppBundle;
// using Craftify.DesignAutomationToolkit.Tests.Helpers.ObjectService;
// using Craftify.DesignAutomationToolkit.Tests.Helpers.WorkItemsApi;
// using Microsoft.Extensions.Logging.Abstractions;
//
// namespace Craftify.DesignAutomationToolkit.Tests.Executions;
//
// public class DesignAutomationExecutionTests
// {
//     [Fact]
//     public async Task Execute_WhenValidSettingsPassed_ReturnsSuccessfulExecutionResult()
//     {
//         var parametersPluginSetupComponent = new ParametersPluginConfigurationComponent();
//         var pluginPathSetupComponent = new PluginPathConfigurationComponent();
//         var workItemPreferencesSetupComponent = new WorkItemPreferenceConfigurationSetupComponent();
//         var chainedPluginSetup = parametersPluginSetupComponent
//             .ChainWith(pluginPathSetupComponent);
//         var objectServiceMock = MockBuilder.CreateObjectServiceMock(new ObjectServiceMockSettings()
//         {
//             InputUrl = ValidUrls.GoogleUrl,
//             OutputUrl = ValidUrls.ExampleUrl
//         });
//         var nicknameProviderMock = MockBuilder.CreateNicknameProviderMock();
//         var activityServiceMock = ActivityServiceMockBuilder
//             .Build(new ()
//             {
//                 ActivitySearchResult = new ActivitySearchResult(true, new Activity()
//                 {
//                     Id = "someId",
//                     Version = 1
//                 }),
//                 NewUpdatedVersion = 2
//             });
//         var appBundleServiceMock = AppBundleServiceMockBuilder
//             .Build(new()
//             {
//                 AppBundleSearchResult = new AppBundleSearchResult(true, new AppBundle()
//                 {
//                     Id = "id",
//                     Version = 1
//                 }),
//                 NewUpdatedVersion = 2
//             });
//         var workItemsApi = WorkItemsApiMockBuilder
//             .CreateWorkItemsApiWithInstantSuccessfulStatus().Object;
//         var nicknameProvider = nicknameProviderMock.Object;
//         var objectService = objectServiceMock.Object;
//         var activityService = activityServiceMock.Object;
//         var appBundleService = appBundleServiceMock.Object;
//         var timedExecutionMeasurement = new TimedExecutionResultMeasurement();
//         var settingHandlerFactory = new SettingHandlerFactory(new ISettingHandlerActivator[]
//         {
//             new InputHandlerActivator(objectService),
//             new OutputHandlerActivator(objectService)
//         });
//         var keyGenerator = new UniqueTrackingKeyGenerator();
//         var activityProvider = new ActivityProvider(nicknameProvider);
//         var workItemProvider = new WorkItemProvider(nicknameProvider, settingHandlerFactory);
//         var activityPublisher = new ActivityPublisher(activityService);
//         var appBundlePublisher = new AppBundlePublisher(appBundleService);
//         var workItemExecutionFactory = new WorkItemExecutionFactory(new IWorkItemExecutionActivator[]
//         {
//             new PollingWorkItemExecutionActivator(workItemsApi, timedExecutionMeasurement),
//             new CallbackWorkItemExecutionActivator(workItemsApi, keyGenerator, timedExecutionMeasurement)
//         });
//         var logger = new NullLogger<DesignAutomationExecution>();
//         var designAutomationExecution = new DesignAutomationExecution(
//             activityProvider,
//             workItemProvider,
//             activityPublisher,
//             appBundlePublisher,
//             workItemExecutionFactory,
//             timedExecutionMeasurement);
//         var timedDesignAutomationExecutionResult = await designAutomationExecution.Execute(setupDescriptor =>
//         {
//             setupDescriptor
//                 .ConfigurePlugin(chainedPluginSetup)
//                 .ConfigureWorkItemPreferences(workItemPreferencesSetupComponent)
//                 .ConfigureStoragePreferences(storage =>
//                 {
//                     storage.BucketIdPreference = new NamedIdPreference("bucketId");
//                 })
//                 .AssignActivityPreferenceProvider(new NamedAliasPreferenceProvider("activity_name", "activity_alias"))
//                 .AssignAppBundlePreferenceProvider(new NamedAliasPreferenceProvider("bundle_name", "bundle_alias"));
//         });
//         var isWorkItemExecutedSuccessfully = timedDesignAutomationExecutionResult.Result.IsSuccessful;
//         Assert.True(isWorkItemExecutedSuccessfully);
//     }
//
//     // [Fact]
//     // public async Task Execute_WhenValidSettingsPassed_ReturnsExpectedExecutionResult()
//     // {
//     //     var parametersPluginSetupComponent = new ParametersPluginSetupComponent();
//     //     var pluginPathSetupComponent = new PluginPathSetupComponent();
//     //     var workItemPreferencesSetupComponent = new WorkItemPreferenceSetupComponent();
//     //     var chainedPluginSetup = parametersPluginSetupComponent
//     //         .ChainWith(pluginPathSetupComponent);
//     //     var timedExecutionMeasurement = new TimedExecutionResultMeasurement();
//     //     var nicknameProviderMock = MockBuilder.CreateNicknameProviderMock();
//     //     var activityProvider = new ActivityProvider(nicknameProviderMock.Object);
//     //     var settingHandlerFactory = DependencyInjectionHelper
//     //         .GetSettingHandlerFactoryProvider(new ObjectServiceMockSettings()
//     //         {
//     //             InputUrl = ValidUrls.ExampleUrl,
//     //             OutputUrl = ValidUrls.GoogleUrl,
//     //         }).GetRequiredService<ISettingHandlerFactory>();
//     //     var workItemProvider = new WorkItemProvider(nicknameProviderMock.Object, settingHandlerFactory);
//     //     var mockActivityService = ActivityServiceMockBuilder
//     //         .Build(new ()
//     //         {
//     //             ActivitySearchResult = new ActivitySearchResult(true, new Activity()
//     //             {
//     //                 Id = "someId",
//     //                 Version = 1
//     //             }),
//     //             NewUpdatedVersion = 2
//     //         });
//     //     var mockAppBundleService = AppBundleServiceMockBuilder
//     //         .Build(new()
//     //         {
//     //             AppBundleSearchResult = new AppBundleSearchResult(true, new AppBundle()
//     //             {
//     //                 Id = "id",
//     //                 Version = 1
//     //             }),
//     //             NewUpdatedVersion = 2
//     //         });
//     //     var activityPublisher = new ActivityPublisher(mockActivityService.Object);
//     //     var appBundlePublisher = new AppBundlePublisher(mockAppBundleService.Object);
//     //     var workItemExecutionFactory = DependencyInjectionHelper
//     //         .GetWorkItemExecutionFactoryProvider()
//     //         .GetRequiredService<IWorkItemExecutionFactory>();
//     //     var designAutomationExecution = new DesignAutomationExecution(
//     //         activityProvider,
//     //         workItemProvider,
//     //         activityPublisher,
//     //         appBundlePublisher,
//     //         workItemExecutionFactory,
//     //         timedExecutionMeasurement);
//     //     var executionResult = await designAutomationExecution.Execute(setupDescriptor =>
//     //     {
//     //         setupDescriptor
//     //             .SetupPlugin(chainedPluginSetup)
//     //             .SetupWorkItemPreferences(workItemPreferencesSetupComponent)
//     //             .SetupStoragePreferences(storage =>
//     //             {
//     //                 storage.BucketIdPreference = new NamedIdPreference("bucketId");
//     //             })
//     //             .SetupActivityPreference(new NamedAliasPreferenceProvider("activity_name", "activity_alias"))
//     //             .SetupAppBundlePreference(new NamedAliasPreferenceProvider("bundle_name", "bundle_alias"));
//     //     });
//     //     var a = 5;
//     // }
//
//     // [Fact]
//     // public async Task Execute_WhenSignedUrlIsInvalid_ThrowsInvalidOperation()
//     // {
//     //     var parametersPluginSetupComponent = new ParametersPluginSetupComponent();
//     //     var pluginPathSetupComponent = new PluginPathSetupComponent();
//     //     var workItemPreferencesSetupComponent = new WorkItemPreferenceSetupComponent();
//     //     var chainedPluginSetup = parametersPluginSetupComponent
//     //         .ChainWith(pluginPathSetupComponent);
//     //     var timedExecutionMeasurement = new TimedExecutionResultMeasurement();
//     //     var nicknameProviderMock = MockBuilder.CreateNicknameProviderMock();
//     //     var activityProvider = new ActivityProvider(nicknameProviderMock.Object);
//     //     var settingHandlerFactory = DependencyInjectionHelper
//     //         .GetSettingHandlerFactoryProvider().GetRequiredService<ISettingHandlerFactory>();
//     //     var workItemProvider = new WorkItemProvider(nicknameProviderMock.Object, settingHandlerFactory);
//     //     var mockActivityService = ActivityServiceMockBuilder
//     //         .Build(new ()
//     //         {
//     //             ActivitySearchResult = new ActivitySearchResult(true, new Activity()
//     //             {
//     //                 Id = "someId",
//     //                 Version = 1
//     //             }),
//     //             NewUpdatedVersion = 2
//     //         });
//     //     var mockAppBundleService = AppBundleServiceMockBuilder
//     //         .Build(new()
//     //         {
//     //             AppBundleSearchResult = new AppBundleSearchResult(true, new AppBundle()
//     //             {
//     //                 Id = "id",
//     //                 Version = 1
//     //             }),
//     //             NewUpdatedVersion = 2
//     //         });
//     //     var activityPublisher = new ActivityPublisher(mockActivityService.Object);
//     //     var appBundlePublisher = new AppBundlePublisher(mockAppBundleService.Object);
//     //     var workItemExecutionFactory = DependencyInjectionHelper
//     //         .GetWorkItemExecutionFactoryProvider()
//     //         .GetRequiredService<IWorkItemExecutionFactory>();
//     //     var designAutomationExecution = new DesignAutomationExecution(
//     //         activityProvider,
//     //         workItemProvider,
//     //         activityPublisher,
//     //         appBundlePublisher,
//     //         workItemExecutionFactory,
//     //         timedExecutionMeasurement);
//     //     Assert.ThrowsAsync<InvalidOperationException>(async () => await designAutomationExecution.Execute(
//     //         setupDescriptor =>
//     //         {
//     //             setupDescriptor
//     //                 .SetupPlugin(chainedPluginSetup)
//     //                 .SetupWorkItemPreferences(workItemPreferencesSetupComponent)
//     //                 .SetupStoragePreferences(storage =>
//     //                 {
//     //                     storage.BucketIdPreference = new NamedIdPreference("bucketId");
//     //                 })
//     //                 .SetupActivityPreference(new NamedAliasPreferenceProvider("activity_name", "activity_alias"))
//     //                 .SetupAppBundlePreference(new NamedAliasPreferenceProvider("bundle_name", "bundle_alias"));
//     //         }));
//     // }
//     //
//     
//     
//     
//     // public void Execute_UsingOnFlySettingSetup()
//     // {
//     //     DesignAutomationExecution designAutomationExecution = default;
//     //     designAutomationExecution.Execute(settingDescriptor =>
//     //     {
//     //         settingDescriptor
//     //             .SetupPlugin(ConfigurePlugin)
//     //             .SetupStoragePreferences(ConfigureStoragePreferences)
//     //             .SetupAppBundlePreference(new NamedAliasPreferenceProvider("name", "alias"))
//     //             .SetupWorkItemPreferences(ConfigureWorkItemPreferences);
//     //     });
//     // }
//     //
//     // private static void ConfigureStoragePreferences(StoragePreferences storagePreferences)
//     // {
//     //     storagePreferences.BucketIdPreference = new NamedIdPreference("bucketId");
//     // }
//     //
//     // private void ConfigurePlugin(PluginDescriptor pluginDescriptor)
//     // {
//     //     pluginDescriptor.WithPath("pathOfPlugin.zip")
//     //         .DescribeParameters(settingsDescriptor =>
//     //         {
//     //             settingsDescriptor
//     //                 .Input(descriptor =>
//     //                 {
//     //                     descriptor
//     //                         .WithKey("rvtFile")
//     //                         .OfValue("path.rvt")
//     //                         .WithLocalName("local.rvt")
//     //                         .Required();
//     //                 })
//     //                 .Input(descriptor =>
//     //                 {
//     //                     descriptor
//     //                         .WithKey("revitParams")
//     //                         .OfValue(new Entity()
//     //                         {
//     //                             Name = "Mariyan"
//     //                         })
//     //                         .WithLocalName("params.json")
//     //                         .Required();
//     //                 })
//     //                 .Output(descriptor =>
//     //                 {
//     //                     descriptor
//     //                         .WithKey("result")
//     //                         .WithLocalName("output.rvt")
//     //                         .OfDescription("some description");
//     //                 });
//     //         });
//     // }
//     // private static void ConfigureWorkItemPreferences(WorkItemPreferencesDescriptor workItemDescriptor)
//     // {
//     //     workItemDescriptor
//     //         .WithIdPreference(new NamedIdPreference("someId"))
//     //         .WithSettings(settingsDescriptor =>
//     //         {
//     //             settingsDescriptor
//     //                 .ChoosePolling()
//     //                 .WithDelayBetweenPolling(3);
//     //         });
//     // }
// }