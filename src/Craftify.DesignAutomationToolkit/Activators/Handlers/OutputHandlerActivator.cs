using Craftify.DesignAutomationToolkit.Handlers;
using Craftify.DesignAutomationToolkit.Interfaces;
using Craftify.DesignAutomationToolkit.Settings;
using Craftify.ObjectStorageServiceToolkit.Interfaces;

namespace Craftify.DesignAutomationToolkit.Activators.Handlers;

public class OutputHandlerActivator : ISettingHandlerActivator
{
    private readonly IObjectService _objectService;
    public Type SettingType => typeof(OutputSetting);

    public OutputHandlerActivator(IObjectService objectService)
    {
        _objectService = objectService ?? throw new ArgumentNullException(nameof(objectService));
    }
    public IArgumentSettingHandler Create()
    {
        return new OutputArgumentSettingHandler(_objectService);
    }
}