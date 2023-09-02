using Autodesk.Forge.DesignAutomation;
using Craftify.DesignAutomationToolkit.Extensions.SearchResults;

namespace Craftify.DesignAutomationToolkit.Extensions;

public static class DesignAutomationClientActivityExtensions
{
    public static async Task<ActivitySearchResult> TryGetActivity(
        this DesignAutomationClient designAutomationClient,
        string activityId,
        string activityAlias)
    {
        var appNickname = await designAutomationClient.GetNicknameAsync();
        var qualifiedName = appNickname.CreateQualifiedName(activityId, activityAlias);
        try
        {
            var activity = await designAutomationClient.GetActivityAsync(qualifiedName);
            return new ActivitySearchResult(true, activity);
        }
        catch (HttpRequestException e) when(e.IsNotFound())
        {
            return new ActivitySearchResult(false);
        }
    }
}