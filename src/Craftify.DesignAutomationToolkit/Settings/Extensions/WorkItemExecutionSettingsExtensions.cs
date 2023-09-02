using Craftify.DesignAutomationToolkit.Constants;

namespace Craftify.DesignAutomationToolkit.Settings.Extensions;

public static class WorkItemExecutionSettingsExtensions
{
    public static int ExtractDelay(this WorkItemExecutionSettings settings)
    {
        if (settings.Arguments.TryGetValue(ArgumentNames.Delay, out var delay) is false)
        {
            throw new InvalidOperationException(
                $"{nameof(WorkItemExecutionSettings)} doesn't have argument : {ArgumentNames.Delay}");
        }
        if (delay is not int delayAsInt)
        {
            throw new InvalidOperationException($"Delay must be of type {typeof(int)}. You passed: {delay.GetType()}");
        }
        return delayAsInt;
    }
    
    public static string ExtractBaseCallbackUrl(this WorkItemExecutionSettings settings)
    {
        if (settings.Arguments.TryGetValue(ArgumentNames.BaseCallbackUrl, out var baseCallbackUrl) is false)
        {
            throw new InvalidOperationException(
                $"{nameof(WorkItemExecutionSettings)} doesn't have argument : {ArgumentNames.BaseCallbackUrl}");
        }
        if (baseCallbackUrl is not string baseCallbackUrlAsString)
        {
            throw new InvalidOperationException($"BaseCallbackUrl must be of type {typeof(string)}. You passed: {baseCallbackUrl.GetType()}");
        }
        return baseCallbackUrlAsString;
    }
}