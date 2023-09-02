using Autodesk.Forge.DesignAutomation.Http;
using Craftify.DesignAutomationToolkit.Constants;

namespace Craftify.DesignAutomationToolkit.Extensions;

public static class ForgeAppsApiExtensions
{
    public static async Task<string> GetNicknameAsync(this IForgeAppsApi forgeAppsApi)
    {
        return (await forgeAppsApi.GetNicknameAsync(ForgeAppsApiReservedWords.Me)).Content;
    }
}