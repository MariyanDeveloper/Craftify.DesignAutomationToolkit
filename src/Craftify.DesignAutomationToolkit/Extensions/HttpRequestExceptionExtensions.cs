namespace Craftify.DesignAutomationToolkit.Extensions;

public static class HttpRequestExceptionExtensions
{
    private static readonly string _notFoundStatusCode = "404";
    public static bool IsNotFound(this HttpRequestException exception) =>
        exception.Message.Contains(_notFoundStatusCode);
}