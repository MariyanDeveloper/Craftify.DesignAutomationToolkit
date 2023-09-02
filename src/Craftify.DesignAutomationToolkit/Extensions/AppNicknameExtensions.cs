namespace Craftify.DesignAutomationToolkit.Extensions;

public static class AppNicknameExtensions
{
    public static string CreateQualifiedName(this string appNickname, string id, string alias) => $"{appNickname}.{id}+{alias}";
}