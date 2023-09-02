using Autodesk.Forge.DesignAutomation.Http;
using Autodesk.Forge.DesignAutomation.Model;

namespace Craftify.DesignAutomationToolkit.Extensions;

public static class AppBundlesApiExtensions
{
    public static async Task<AppBundle> GetAsync(this IAppBundlesApi appBundlesApi, string qualifiedName)
    {
        var response = await appBundlesApi.GetAppBundleAsync(qualifiedName);
        return response.Content;
    }
    
    public static async Task<AppBundle> CreateAsync(this IAppBundlesApi appBundlesApi, AppBundle appBundle)
    {
        var response = await appBundlesApi.CreateAppBundleAsync(appBundle);
        return response.Content;
    }

    public static async Task<Alias> CreateAliasAsync(this IAppBundlesApi appBundlesApi, string id, Alias alias)
    {
        var response = await appBundlesApi.CreateAppBundleAliasAsync(id, alias);
        return response.Content;
    }
}