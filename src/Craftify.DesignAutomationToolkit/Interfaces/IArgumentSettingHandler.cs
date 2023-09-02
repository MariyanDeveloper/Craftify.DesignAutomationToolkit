using Craftify.DesignAutomationToolkit.Handlers;
using Craftify.DesignAutomationToolkit.Settings;

namespace Craftify.DesignAutomationToolkit.Interfaces;

public interface IArgumentSettingHandler
{
    Task<HandlerResult> Handle(BaseSetting baseSetting, HandlerStorageDetails handlerStorageDetails);
}