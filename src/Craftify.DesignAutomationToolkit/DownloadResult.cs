namespace Craftify.DesignAutomationToolkit;

public class DownloadResult
{
    public bool IsSuccessful { get; }
    public string? ErrorMessage { get; }

    public DownloadResult(bool isSuccessful, string? errorMessage = null)
    {
        IsSuccessful = isSuccessful;
        ErrorMessage = errorMessage;
    }
}