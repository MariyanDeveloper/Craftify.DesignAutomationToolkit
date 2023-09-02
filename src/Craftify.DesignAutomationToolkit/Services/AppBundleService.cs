using Autodesk.Forge.DesignAutomation;
using Autodesk.Forge.DesignAutomation.Http;
using Autodesk.Forge.DesignAutomation.Model;
using Craftify.DesignAutomationToolkit.Extensions;
using Craftify.DesignAutomationToolkit.Extensions.SearchResults;
using Craftify.DesignAutomationToolkit.Services.Responses;

namespace Craftify.DesignAutomationToolkit.Services;

public class AppBundleService : IAppBundleService
{
    private readonly IForgeAppsApi _forgeAppsApi;
    private readonly DesignAutomationClient _designAutomationClient;
    private readonly IAppBundlesApi _appBundlesApi;

    public AppBundleService(
        IForgeAppsApi forgeAppsApi,
        DesignAutomationClient designAutomationClient,
        IAppBundlesApi appBundlesApi)
    {
        _forgeAppsApi = forgeAppsApi ?? throw new ArgumentNullException(nameof(forgeAppsApi));
        _designAutomationClient = designAutomationClient ?? throw new ArgumentNullException(nameof(designAutomationClient));
        _appBundlesApi = appBundlesApi ?? throw new ArgumentNullException(nameof(appBundlesApi));
    }

    public async Task<AppBundleSearchResult> TryGetAsync(string appBundleId, string appBundleAlias)
    {
        try
        {
            var appNickname = await _forgeAppsApi.GetNicknameAsync();
            var qualifiedBundleName = appNickname.CreateQualifiedName(appBundleId, appBundleAlias);
            var appBundle = await _appBundlesApi.GetAsync(qualifiedBundleName);
            return new AppBundleSearchResult(true, appBundle);
        }
        catch (HttpRequestException e) when (e.IsNotFound())
        {
            return new AppBundleSearchResult(false);
        }
    }

    public async Task<CreatedAliasedAppBundleResponse> CreateAliasedAsync(AppBundle appBundle, string alias, string packagePath)
    {
        var createdAppBundle = await _appBundlesApi.CreateAsync(appBundle);
        await _designAutomationClient.UploadAppBundleBits(createdAppBundle.UploadParameters, packagePath);
        var createdAlias = await _appBundlesApi.CreateAliasAsync(
            appBundle.Id,
            new Alias() { Id = alias, Version = createdAppBundle.Version.Value });
        return new CreatedAliasedAppBundleResponse(createdAppBundle, createdAlias);
    }

    public async Task<int> UpdateAsync(AppBundle appBundle, string alias, string packagePath)
    {
        var newVersion = await _designAutomationClient.UpdateAppBundleAsync(appBundle, alias, packagePath);
        return newVersion;
    }

    public async Task DeleteVersionAsync(string id, int version)
    {
        await _appBundlesApi.DeleteAppBundleVersionAsync(id, version);
    }
}