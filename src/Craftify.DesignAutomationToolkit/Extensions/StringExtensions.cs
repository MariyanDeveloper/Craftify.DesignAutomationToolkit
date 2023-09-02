namespace Craftify.DesignAutomationToolkit.Extensions;

public static class StringExtensions
{
    public static bool IsUrlValid(this string url) => Uri.TryCreate(url, UriKind.Absolute, out _);
    
    public static FilePathResult GetFilePathResult(this string filePath)
    {
        if (string.IsNullOrWhiteSpace(filePath))
        {
            return new FilePathResult(false, ErrorMessage: "File path is null or empty");
        }
        var fileInfo = new FileInfo(filePath);
        if (fileInfo.Exists)
        {
            return new FilePathResult(fileInfo.Exists, fileInfo.FullName);
        }
        return new FilePathResult(false, ErrorMessage:"File doesn't exist");
    }
}