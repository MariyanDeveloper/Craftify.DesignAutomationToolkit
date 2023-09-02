using Craftify.DesignAutomationToolkit.Executions.Results;
using Craftify.DesignAutomationToolkit.Settings;
using Craftify.DesignAutomationToolkit.Settings.Descriptors;

namespace Craftify.DesignAutomationToolkit.Interfaces;

public interface IDesignAutomationExecution
{
    Task<TimedExecutionResult<DesignAutomationExecutionResult>> Execute(
        Action<DesignAutomationSettingDescriptor> setupDescriptor);

    Task<TimedExecutionResult<DesignAutomationExecutionResult>> Execute(
        DesignAutomationExecutionSetting designAutomationExecutionSetting);
}