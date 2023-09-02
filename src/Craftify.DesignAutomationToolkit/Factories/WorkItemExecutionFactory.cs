using Craftify.DesignAutomationToolkit.Interfaces;
using Craftify.DesignAutomationToolkit.Settings;

namespace Craftify.DesignAutomationToolkit.Factories;

public class WorkItemExecutionFactory : IWorkItemExecutionFactory
{
    private readonly Dictionary<string,IWorkItemExecutionActivator> _typeToExecutionActivators;

    public WorkItemExecutionFactory(IEnumerable<IWorkItemExecutionActivator> executionActivators)
    {
        if (executionActivators is null) throw new ArgumentNullException(nameof(executionActivators));
        _typeToExecutionActivators = executionActivators.ToDictionary(x => x.Type);
    }
    public IWorkItemExecution Create(WorkItemExecutionSettings executionSettings)
    {
        if (_typeToExecutionActivators
                .TryGetValue(executionSettings.WorkItemExecutionPreference, out var activator) is false)
        {
            throw new InvalidOperationException(
                $"Activator of this type {executionSettings.WorkItemExecutionPreference} was not found");
        }

        return activator.Create(executionSettings);
    }
}