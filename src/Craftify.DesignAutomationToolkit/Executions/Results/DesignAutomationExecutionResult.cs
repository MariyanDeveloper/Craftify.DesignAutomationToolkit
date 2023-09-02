using Craftify.DesignAutomationToolkit.Publishers;

namespace Craftify.DesignAutomationToolkit.Executions.Results;

public record DesignAutomationExecutionResult(
    TimedExecutionResult<PublishedAppBundle> AppBundleExecutionResult,
    TimedExecutionResult<PublishedActivity> ActivityExecutionResult,
    TimedExecutionResult<WorkItemResult> WorkItemExecutionResult)
{
    public bool IsSuccessful => WorkItemExecutionResult.Result.IsSuccessful;
};