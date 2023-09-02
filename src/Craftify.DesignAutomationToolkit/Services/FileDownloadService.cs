using Craftify.DesignAutomationToolkit.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Craftify.DesignAutomationToolkit.Services;


//TODO - think about moving it to dedicated nuget package
public class FileDownloadService : IFileDownloadService
{
    private readonly HttpClient _httpClient;

    public FileDownloadService(HttpClient httpClient)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }
    public async Task<DownloadResult> DownloadLocalAsync(string signedUrl, string localPath, Action<LocalDownloadOptions>? configOptions = null)
    {
        try
        {
            return await DownloadLocalInternalAsync(
                signedUrl,
                localPath,
                configOptions);
        }
        catch (Exception e)
        {
            return new DownloadResult(false, e.Message);
        }
    }

    public async Task<DownloadResult> DownloadBrowserAsync(HttpContext context, string signedUrl, string fileName)
    {
        try
        {
            return await DownloadBrowserInternalAsync(context, signedUrl, fileName);
        }
        catch (Exception e)
        {
            return new DownloadResult(false, e.Message);
        }
    }

    private async Task<DownloadResult> DownloadBrowserInternalAsync(HttpContext context, string signedUrl, string fileName)
    {
        await using var stream = await _httpClient.GetStreamAsync(signedUrl);

        context.Response.ContentType = "application/octet-stream";

        context.Response.Headers.Add("Content-Disposition", $"attachment; filename={fileName}");

        await stream.CopyToAsync(context.Response.Body);
        return new DownloadResult(true);
    }

    private async Task<DownloadResult> DownloadLocalInternalAsync(string signedUrl, string localPath, Action<LocalDownloadOptions>? configOptions)
    {
        var options = CreateOptions(configOptions);
        if (File.Exists(localPath))
        {
            options.FileConflictHandler.Handle(localPath);
        }

        await using var clientStream = await _httpClient.GetStreamAsync(signedUrl);
        await using var fileStream = new FileStream(localPath, FileMode.Create);
        await clientStream.CopyToAsync(fileStream);
        return new DownloadResult(true);
    }

    private static LocalDownloadOptions CreateOptions(Action<LocalDownloadOptions>? configOptions)
    {
        var options = new LocalDownloadOptions();
        configOptions?.Invoke(options);
        return options;
    }
}