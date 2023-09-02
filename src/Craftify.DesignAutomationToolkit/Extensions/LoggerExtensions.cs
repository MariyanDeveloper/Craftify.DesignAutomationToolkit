using Craftify.DesignAutomationToolkit.Executions;
using Craftify.DesignAutomationToolkit.Executions.Results;
using Craftify.DesignAutomationToolkit.Publishers;
using Microsoft.Extensions.Logging;

namespace Craftify.DesignAutomationToolkit.Extensions;

public static class LoggerExtensions
{
    public static void LogExecutedWorkItem(this ILogger<DesignAutomationExecution> logger,
        TimedExecutionResult<WorkItemResult> workItemExecutionResult)
    {
        logger.LogInformation(
            "Work item execution completed in {DurationSeconds} seconds. Id: {Id}, ActivityId: {ActivityId}, Status: {WorkItemStatus}.",
            workItemExecutionResult.Duration.TotalSeconds,
            workItemExecutionResult.Result.Id,
            workItemExecutionResult.Result.ActivityId,
            workItemExecutionResult.Result.WorkItemStatus);
    }
    public static void LogPublishedActivity(this ILogger<DesignAutomationExecution> logger,
        TimedExecutionResult<PublishedActivity> activityExecutionResult)
    {
        logger.LogInformation(
            "Activity publishing completed in {DurationSeconds} seconds. Id: {Id}, Alias: {Alias}, Version: {Version}, Status: {ActivityStatus}.",
            activityExecutionResult.Duration.TotalSeconds,
            activityExecutionResult.Result.Id,
            activityExecutionResult.Result.Alias,
            activityExecutionResult.Result.Version,
            activityExecutionResult.Result.ActivityStatus);
    }
    
    public static void LogPublishedAppBundle(this ILogger<DesignAutomationExecution> logger,
        TimedExecutionResult<PublishedAppBundle> appBundleExecutionResult)
    {
        logger.LogInformation(
            "App bundle publishing completed in {DurationSeconds} seconds. Id: {Id}, Alias: {Alias}, Version: {Version}, Status: {AppBundleStatus}.",
            appBundleExecutionResult.Duration.TotalSeconds,
            appBundleExecutionResult.Result.Id,
            appBundleExecutionResult.Result.Alias,
            appBundleExecutionResult.Result.Version,
            appBundleExecutionResult.Result.AppBundleStatus);
    }
    
    public static void LogExecutedDesignAutomation(this ILogger<DesignAutomationExecution> logger,
        TimedExecutionResult<DesignAutomationExecutionResult> result)
    {
        logger.LogInformation(
            "Design automation execution completed in {DurationSeconds} seconds. Is successful: {IsSuccessful}.",
            result.Duration.TotalSeconds,
            result.Result.IsSuccessful);
    }

    public static void LogStartingDesignAutomationExecution(this ILogger<DesignAutomationExecution> logger)
    {
        logger.LogInformation("Starting design automation execution.");
    }
    
}