using Autodesk.Forge.DesignAutomation;
using Autodesk.Forge.DesignAutomation.Http;
using Autodesk.Forge.DesignAutomation.Model;
using Craftify.DesignAutomationToolkit.Extensions;
using Craftify.DesignAutomationToolkit.Extensions.SearchResults;
using Craftify.DesignAutomationToolkit.Interfaces;
using Craftify.DesignAutomationToolkit.Services.Responses;

namespace Craftify.DesignAutomationToolkit.Services;

public class ActivityService : IActivityService
{
    private readonly IForgeAppsApi _forgeAppsApi;
    private readonly DesignAutomationClient _designAutomationClient;
    private readonly IActivitiesApi _activitiesApi;

    public ActivityService(
        IForgeAppsApi forgeAppsApi,
        DesignAutomationClient designAutomationClient,
        IActivitiesApi activitiesApi)
    {
        _forgeAppsApi = forgeAppsApi ?? throw new ArgumentNullException(nameof(forgeAppsApi));
        _designAutomationClient = designAutomationClient ?? throw new ArgumentNullException(nameof(designAutomationClient));
        _activitiesApi = activitiesApi ?? throw new ArgumentNullException(nameof(activitiesApi));
    }
    public async Task<ActivitySearchResult> TryGetAsync(string activityId, string activityAlias)
    {
        var appNickname = await _forgeAppsApi.GetNicknameAsync();
        var qualifiedName = new QualifiedName(
            appNickname,
            activityId,
            activityAlias).ToString();
        try
        {
            var activity = await _activitiesApi.GetAsync(qualifiedName);
            return new ActivitySearchResult(true, activity);
        }
        catch (HttpRequestException e) when(e.IsNotFound())
        {
            return new ActivitySearchResult(false);
        }
    }

    public async Task<CreatedAliasedActivityResponse> CreateAliasedAsync(Activity activity, string alias)
    {
        var createdActivity = await _activitiesApi.CreateAsync(activity);
        var createdAlias = await _activitiesApi
            .CreateAliasAsync(activity.Id, new Alias() { Id = alias , Version = createdActivity.Version.Value });
        return new CreatedAliasedActivityResponse(createdActivity, createdAlias);
    }

    public async Task<int> UpdateAsync(Activity activity, string alias)
    {
        var newVersion = await _designAutomationClient.UpdateActivityAsync(activity, alias);
        return newVersion;

    }

    public async Task DeleteVersionAsync(string id, int version)
    {
        await _activitiesApi.DeleteActivityVersionAsync(id, version);
    }
    private async Task<Activity> CreateNewVersionAsync(Activity activity, string id)
    {
        return await _activitiesApi.CreateNewVersionAsync(id, activity);
    }
    
}