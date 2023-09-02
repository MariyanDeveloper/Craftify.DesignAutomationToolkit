using Craftify.DesignAutomationToolkit.Executions.Results;

namespace Craftify.DesignAutomationToolkit.Interfaces;

public interface ITimedExecutionResultMeasurement
{
    Task<TimedExecutionResult<T>> MeasureAsync<T>(Func<Task<T>> operation);
}