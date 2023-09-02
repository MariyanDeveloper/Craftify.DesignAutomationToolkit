using Craftify.DesignAutomationToolkit.Actions.Interfaces;
using Craftify.DesignAutomationToolkit.Executions.Results;
using Craftify.DesignAutomationToolkit.Extensions;
using Craftify.DesignAutomationToolkit.Interfaces;
using Craftify.DesignAutomationToolkit.Models;
using Craftify.DesignAutomationToolkit.Publishers;
using Craftify.DesignAutomationToolkit.Settings;
using Craftify.DesignAutomationToolkit.Settings.Descriptors;
using Craftify.ObjectStorageServiceToolkit.Interfaces;
using Microsoft.Extensions.Logging;

namespace Craftify.DesignAutomationToolkit.Executions;

public class DesignAutomationExecution : IDesignAutomationExecution
{
    private readonly IActivityProvider _activityProvider;
    private readonly IWorkItemProvider _workItemProvider;
    private readonly IActivityPublisher _activityPublisher;
    private readonly IAppBundlePublisher _appBundlePublisher;
    private readonly IWorkItemExecutionFactory _workItemExecutionFactory;
    private readonly ITimedExecutionResultMeasurement _executionResultMeasurement;
    private readonly IBucketService _bucketService;
    private readonly ILogger<DesignAutomationExecution> _logger;
    private readonly IEnumerable<IWorkItemExecutedAction>? _workItemExecutedActions;
    private readonly List<PublishedArgument> _publishedArguments = new();

    public DesignAutomationExecution(
        IActivityProvider activityProvider,
        IWorkItemProvider workItemProvider,
        IActivityPublisher activityPublisher,
        IAppBundlePublisher appBundlePublisher,
        IWorkItemExecutionFactory workItemExecutionFactory,
        ITimedExecutionResultMeasurement executionResultMeasurement,
        IBucketService bucketService,
        ILogger<DesignAutomationExecution> logger,
        IEnumerable<IWorkItemExecutedAction>? workItemExecutedActions = default)
    {
        _activityProvider = activityProvider ?? throw new ArgumentNullException(nameof(activityProvider));
        _workItemProvider = workItemProvider ?? throw new ArgumentNullException(nameof(workItemProvider));
        _activityPublisher = activityPublisher ?? throw new ArgumentNullException(nameof(activityPublisher));
        _appBundlePublisher = appBundlePublisher ?? throw new ArgumentNullException(nameof(appBundlePublisher));
        _workItemExecutionFactory = workItemExecutionFactory ??
                                    throw new ArgumentNullException(nameof(workItemExecutionFactory));
        _executionResultMeasurement = executionResultMeasurement ??
                                      throw new ArgumentNullException(nameof(executionResultMeasurement));
        _bucketService = bucketService ?? throw new ArgumentNullException(nameof(bucketService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _workItemExecutedActions = workItemExecutedActions;
    }

    public async Task<TimedExecutionResult<DesignAutomationExecutionResult>> Execute(
        DesignAutomationExecutionSetting designAutomationExecutionSetting)
    {
        _logger.LogStartingDesignAutomationExecution();
        var result = await _executionResultMeasurement.MeasureAsync(async () =>
            await ExecuteInternal(designAutomationExecutionSetting));
        _logger.LogExecutedDesignAutomation(result);
        return result;
    }
    public async Task<TimedExecutionResult<DesignAutomationExecutionResult>> Execute(
        Action<DesignAutomationSettingDescriptor> setupDescriptor)
    {
        _logger.LogStartingDesignAutomationExecution();
        var result = await _executionResultMeasurement.MeasureAsync(async () =>
        {
            _logger.LogInformation("Generating design automation settings.");
            var designAutomationSetting = CreateDesignAutomationSetting(setupDescriptor);
            _logger.LogInformation("Publishing app bundle.");
            return await ExecuteInternal(designAutomationSetting);
        });
        _logger.LogExecutedDesignAutomation(result);
        return result;
    }

    private async Task<DesignAutomationExecutionResult> ExecuteInternal(
        DesignAutomationExecutionSetting designAutomationExecutionSetting)
    {
        var appBundleExecutionResult = await ExecuteAppBundlePublishing(designAutomationExecutionSetting);

        _logger.LogPublishedAppBundle(appBundleExecutionResult);

        _logger.LogInformation("Publishing activity.");
        var activityExecutionResult = await ExecuteActivityPublishing(designAutomationExecutionSetting);

        _logger.LogPublishedActivity(activityExecutionResult);

        _logger.LogInformation("Ensuring bucket creation.");
        await EnsureBucketIsCreated(designAutomationExecutionSetting.StoragePreferences);
        _logger.LogInformation("Executing work item.");

        var workItemExecutionResult = await ExecuteWorkItem(designAutomationExecutionSetting);
        _logger.LogExecutedWorkItem(workItemExecutionResult);
        var workItemExecutedArgs = new WorkItemExecutedArgs(_publishedArguments, workItemExecutionResult);
        await ExecuteActions(workItemExecutedArgs);
        return new DesignAutomationExecutionResult(
            appBundleExecutionResult,
            activityExecutionResult,
            workItemExecutionResult);
    }

    private async Task ExecuteActions(WorkItemExecutedArgs workItemExecutedArgs)
    {
        if (_workItemExecutedActions is null)
        {
            _logger.LogWarning("No work item executed actions defined.");
            return;
        }

        _logger.LogInformation("Executing {ActionCount} work item executed actions.", _workItemExecutedActions.Count());
        foreach (var workItemExecutedAction in _workItemExecutedActions)
        {
            _logger.LogInformation("Executing action of type {ActionType}", workItemExecutedAction.GetType());
            await workItemExecutedAction.Handle(workItemExecutedArgs);
        }
    }

    private DesignAutomationExecutionSetting CreateDesignAutomationSetting(
        Action<DesignAutomationSettingDescriptor> setupDescriptor)
    {
        var descriptor = new DesignAutomationSettingDescriptor();
        setupDescriptor(descriptor);
        return descriptor.DesignAutomationExecutionSetting;
    }

    private async Task<TimedExecutionResult<PublishedAppBundle>> ExecuteAppBundlePublishing(
        DesignAutomationExecutionSetting designAutomationExecutionSetting)
    {
        var appBundlePreference = designAutomationExecutionSetting.AppBundlePreferenceProvider.Get();

        return await _executionResultMeasurement.MeasureAsync(() =>
            _appBundlePublisher.Publish(
                appBundlePreference.Name,
                appBundlePreference.Alias,
                designAutomationExecutionSetting.PluginSetting.Path));
    }

    private async Task<TimedExecutionResult<PublishedActivity>> ExecuteActivityPublishing(
        DesignAutomationExecutionSetting designAutomationExecutionSetting)
    {
        var activity = await _activityProvider.Get(designAutomationExecutionSetting);
        var activityPreference = designAutomationExecutionSetting.ActivityPreferenceProvider.Get();

        return await _executionResultMeasurement.MeasureAsync(() =>
            _activityPublisher.Publish(
                activity,
                activityPreference.Alias));
    }

    private async Task<TimedExecutionResult<WorkItemResult>> ExecuteWorkItem(
        DesignAutomationExecutionSetting designAutomationExecutionSetting)
    {
        var workItem = await _workItemProvider.Get(designAutomationExecutionSetting, handleResult =>
        {
            if (handleResult is not { FileSignedUrl: not null })
            {
                return;
            }
            var publishedArgument = new PublishedArgument(handleResult.Argument, handleResult.ArgumentType,
                handleResult.FileSignedUrl);
            _publishedArguments.Add(publishedArgument);
        });
        var executionSettings = designAutomationExecutionSetting.WorkItemPreferences.WorkItemExecutionSettings;
        var workItemExecution = _workItemExecutionFactory.Create(executionSettings);
        return await workItemExecution.Execute(workItem, CancellationToken.None);
    }

    private async Task EnsureBucketIsCreated(StoragePreferences storagePreferences)
    {
        var bucketId = storagePreferences
            .BucketIdPreference
            .Get();
        await _bucketService.GetOrCreate(bucketId);
    }
}