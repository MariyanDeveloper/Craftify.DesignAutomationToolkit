using System.Diagnostics;
using Craftify.DesignAutomationToolkit.Executions.Results;
using Craftify.DesignAutomationToolkit.Interfaces;

namespace Craftify.DesignAutomationToolkit.Measurements;

public class TimedExecutionResultMeasurement : ITimedExecutionResultMeasurement
{
    public async Task<TimedExecutionResult<T>> MeasureAsync<T>(Func<Task<T>> operation)
    {
        var stopwatch = Stopwatch.StartNew();
        var startTime = DateTime.UtcNow;
        var result = await operation();
        var timedExecutionResult = new TimedExecutionResult<T>(
            startTime,
            stopwatch.Elapsed,
            result);
        return timedExecutionResult;
    }
}