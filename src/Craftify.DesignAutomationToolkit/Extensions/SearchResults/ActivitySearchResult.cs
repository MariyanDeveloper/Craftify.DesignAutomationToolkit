using Autodesk.Forge.DesignAutomation.Model;

namespace Craftify.DesignAutomationToolkit.Extensions.SearchResults;

public class ActivitySearchResult
{
    public bool IsFound { get; } = false;
    public Activity? Activity { get; }

    public ActivitySearchResult(bool isFound, Activity? activity = null)
    {
        IsFound = isFound;
        Activity = activity;
    }
}