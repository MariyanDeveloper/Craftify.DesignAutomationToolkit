using Autodesk.Forge.DesignAutomation.Model;
using Craftify.DesignAutomationToolkit.Settings;

namespace Craftify.DesignAutomationToolkit.Interfaces;

public interface IActivityProvider
{
    Task<Activity> Get(DesignAutomationExecutionSetting designAutomationExecutionSetting);
}