using Craftify.DesignAutomationToolkit.Handlers;
using Craftify.DesignAutomationToolkit.Interfaces;
using Craftify.DesignAutomationToolkit.Settings;
using Craftify.ObjectStorageServiceToolkit.Interfaces;

namespace Craftify.DesignAutomationToolkit.Activators.Handlers;

public class InputHandlerActivator : ISettingHandlerActivator
{
    private readonly IObjectService _objectService;
    public Type SettingType => typeof(InputSetting);

    public InputHandlerActivator(IObjectService objectService)
    {
        _objectService = objectService ?? throw new ArgumentNullException(nameof(objectService));
    }
    public IArgumentSettingHandler Create()
    {
        return new InputArgumentSettingHandler(_objectService);
    }
}