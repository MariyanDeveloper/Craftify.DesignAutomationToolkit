using Autodesk.Forge.DesignAutomation;

namespace Craftify.DesignAutomationToolkit.Extensions;

public static class DesignAutomationClientExtensions
{
    private static readonly string _reservedKeyForCurrentUser = "me";
    public static async Task<string> GetNicknameAsync(this DesignAutomationClient designAutomationClient)
    {
        if (designAutomationClient is null) throw new ArgumentNullException(nameof(designAutomationClient));
        return await designAutomationClient.GetNicknameAsync(_reservedKeyForCurrentUser);
    }
}