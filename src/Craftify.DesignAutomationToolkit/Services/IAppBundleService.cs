using Autodesk.Forge.DesignAutomation.Model;
using Craftify.DesignAutomationToolkit.Extensions.SearchResults;
using Craftify.DesignAutomationToolkit.Services.Responses;

namespace Craftify.DesignAutomationToolkit.Services;

public interface IAppBundleService
{
    Task<AppBundleSearchResult> TryGetAsync(string appBundleId, string appBundleAlias);
    Task<CreatedAliasedAppBundleResponse> CreateAliasedAsync(AppBundle appBundle, string alias, string packagePath);
    Task<int> UpdateAsync(AppBundle appBundle, string alias, string packagePath);
    Task DeleteVersionAsync(string id, int version);
}