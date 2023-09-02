namespace Craftify.DesignAutomationToolkit.Interfaces;

public interface ISettingHandlerActivator
{
    Type SettingType { get; }
    IArgumentSettingHandler Create();
}