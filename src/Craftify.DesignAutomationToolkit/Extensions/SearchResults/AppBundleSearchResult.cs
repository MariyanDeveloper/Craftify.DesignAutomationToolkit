using Autodesk.Forge.DesignAutomation.Model;

namespace Craftify.DesignAutomationToolkit.Extensions.SearchResults;

public class AppBundleSearchResult
{
    public bool IsFound { get; } = false;
    public AppBundle? AppBundle { get; }

    public AppBundleSearchResult(bool isFound, AppBundle? appBundle = null)
    {
        IsFound = isFound;
        AppBundle = appBundle;
    }
}