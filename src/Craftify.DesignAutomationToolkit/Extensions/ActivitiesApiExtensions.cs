using Autodesk.Forge.DesignAutomation.Http;
using Autodesk.Forge.DesignAutomation.Model;

namespace Craftify.DesignAutomationToolkit.Extensions;

public static class ActivitiesApiExtensions
{
    public static async Task<Activity> GetAsync(this IActivitiesApi activitiesApi, string qualifiedName)
    {
        var response = await activitiesApi.GetActivityAsync(qualifiedName);
        return response.Content;
    }

    public static async Task<Activity> CreateAsync(this IActivitiesApi activitiesApi, Activity activity)
    {
        var response = await activitiesApi.CreateActivityAsync(activity);
        return response.Content;
    }

    public static async Task<Activity> CreateNewVersionAsync(this IActivitiesApi activitiesApi, string id, Activity activity)
    {
        var response = await activitiesApi.CreateActivityVersionAsync(id, activity);
        return response.Content;
    }
    public static async Task<Alias> CreateAliasAsync(this IActivitiesApi activitiesApi, string id, Alias alias)
    {
        var response = await activitiesApi
            .CreateActivityAliasAsync(id, alias);
        return response.Content;
    }
}