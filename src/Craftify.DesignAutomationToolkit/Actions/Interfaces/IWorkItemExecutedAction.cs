using Craftify.DesignAutomationToolkit.Models;

namespace Craftify.DesignAutomationToolkit.Actions.Interfaces;

public interface IWorkItemExecutedAction
{
    Task Handle(WorkItemExecutedArgs args);
}