using Autodesk.Forge.DesignAutomation.Model;
using Craftify.DesignAutomationToolkit.Extensions.SearchResults;
using Craftify.DesignAutomationToolkit.Interfaces;
using Craftify.DesignAutomationToolkit.Publishers.Options;
using Craftify.DesignAutomationToolkit.Publishers.Statuses;

namespace Craftify.DesignAutomationToolkit.Publishers;

public class ActivityPublisher : IActivityPublisher
{
    private readonly IActivityService _activityService;
    private readonly int _initialVersion = 1;

    public ActivityPublisher(IActivityService activityService)
    {
        _activityService = activityService ?? throw new ArgumentNullException(nameof(activityService));
    }
    public async Task<PublishedActivity> Publish(Activity activity, string alias, Action<PublishActivityOptions>? configOptions = null)
    {
        var options = CreateOptions(configOptions);
        var activitySearchResult = await LookUpActivity(activity, alias);
        if (activitySearchResult.IsFound is false)
        {
            return await CreateNewPublishedActivity(activity, alias);
        }
        var publishedActivity = await CreateUpdatedPublishedActivity(activity, alias);
        await CleanUpPreviousVersionIfRequested(
            activity,
            options,
            activitySearchResult.Activity.Version);
        return publishedActivity;
    }

    private async Task<PublishedActivity> CreateUpdatedPublishedActivity(Activity activity, string alias)
    {
        var newVersion = await _activityService.UpdateAsync(activity, alias);
        var publishedActivity = new PublishedActivity(
            activity.Id,
            alias,
            newVersion,
            ActivityStatus.Updated);
        return publishedActivity;
    }

    private async Task<PublishedActivity> CreateNewPublishedActivity(Activity activity, string alias)
    {
        await _activityService.CreateAliasedAsync(activity, alias);
        return new PublishedActivity(activity.Id, alias, _initialVersion, ActivityStatus.Created);
    }

    private async Task<ActivitySearchResult> LookUpActivity(Activity activity, string alias)
    {
        var activitySearchResult = await _activityService.TryGetAsync(
            activity.Id,
            alias);
        return activitySearchResult;
    }

    private static PublishActivityOptions CreateOptions(Action<PublishActivityOptions>? configOptions)
    {
        var options = new PublishActivityOptions();
        configOptions?.Invoke(options);
        return options;
    }

    private async Task CleanUpPreviousVersionIfRequested(
        Activity activity,
        PublishActivityOptions options,
        int? previousVersion)
    {
        if (options.CleanUpPreviousVersion is false)
        {
            return;
        }
        if (previousVersion is not null)
        {
            await _activityService.DeleteVersionAsync(activity.Id, previousVersion.Value);
        }
    }
}