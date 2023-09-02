using Autodesk.Forge.DesignAutomation.Model;
using Craftify.DesignAutomationToolkit.Extensions.SearchResults;
using Craftify.DesignAutomationToolkit.Services.Responses;

namespace Craftify.DesignAutomationToolkit.Interfaces;

public interface IActivityService
{
    Task<ActivitySearchResult> TryGetAsync(string activityId, string activityAlias);
    Task<CreatedAliasedActivityResponse> CreateAliasedAsync(Activity activity, string alias);
    Task<int> UpdateAsync(Activity activity, string alias);
    Task DeleteVersionAsync(string id, int version);
}