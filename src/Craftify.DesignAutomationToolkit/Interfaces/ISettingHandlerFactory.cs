namespace Craftify.DesignAutomationToolkit.Interfaces;

public interface ISettingHandlerFactory
{
    IArgumentSettingHandler Create(Type settingType);
}