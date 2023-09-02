using Microsoft.AspNetCore.Http;

namespace Craftify.DesignAutomationToolkit.Interfaces;

public interface IFileDownloadService
{
    Task<DownloadResult> DownloadLocalAsync(string signedUrl, string localPath, Action<LocalDownloadOptions>? configOptions = null);
    Task<DownloadResult> DownloadBrowserAsync(HttpContext context, string signedUrl, string fileName);
}