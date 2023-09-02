namespace Craftify.DesignAutomationToolkit.Executions.Results;

public record TimedExecutionResult<T>(DateTime StartTime, TimeSpan Duration, T Result);