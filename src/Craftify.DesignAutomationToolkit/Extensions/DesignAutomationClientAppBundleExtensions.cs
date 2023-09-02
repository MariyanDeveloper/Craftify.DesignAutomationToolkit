using Autodesk.Forge.DesignAutomation;
using Craftify.DesignAutomationToolkit.Extensions.SearchResults;

namespace Craftify.DesignAutomationToolkit.Extensions;

public static class DesignAutomationClientAppBundleExtensions
{
    public static async Task<AppBundleSearchResult> TryGetAppBundleAsync(
        this DesignAutomationClient designAutomationClient,
        string appBundleId,
        string appBundleAlias)
    {
        if (designAutomationClient is null) throw new ArgumentNullException(nameof(designAutomationClient));
        try
        {
            var appNickname = await designAutomationClient.GetNicknameAsync();
            var qualifiedBundleName = appNickname.CreateQualifiedName(appBundleId, appBundleAlias);
            var appBundle = await designAutomationClient.GetAppBundleAsync(qualifiedBundleName);
            return new AppBundleSearchResult(true, appBundle);
        }
        catch (HttpRequestException e) when (e.IsNotFound())
        {
            return new AppBundleSearchResult(false);
        }
    }
}