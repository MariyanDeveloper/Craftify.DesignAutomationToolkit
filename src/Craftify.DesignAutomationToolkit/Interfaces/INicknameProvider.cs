namespace Craftify.DesignAutomationToolkit.Interfaces;

public interface INicknameProvider
{
    Task<Nickname> Get();
}