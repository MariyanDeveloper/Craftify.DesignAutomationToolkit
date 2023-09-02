using Autodesk.Forge.DesignAutomation;
using Craftify.DesignAutomationToolkit.Extensions;
using Craftify.DesignAutomationToolkit.Interfaces;

namespace Craftify.DesignAutomationToolkit.Providers;

public class NicknameProvider : INicknameProvider
{
    private readonly DesignAutomationClient _designAutomationClient;

    public NicknameProvider(DesignAutomationClient designAutomationClient)
    {
        _designAutomationClient = designAutomationClient ?? throw new ArgumentNullException(nameof(designAutomationClient));
    }
    public async Task<Nickname> Get()
    {
        var nickname = await _designAutomationClient.GetNicknameAsync();
        return new Nickname(nickname);
    }
}