using Craftify.DesignAutomationToolkit.Actions.Interfaces;
using Craftify.DesignAutomationToolkit.Handlers;
using Craftify.DesignAutomationToolkit.Interfaces;
using Craftify.DesignAutomationToolkit.Models;
using Microsoft.Extensions.Logging;

namespace Craftify.DesignAutomationToolkit.Console;

public class DownloadFileLocallyAction : IWorkItemExecutedAction
{
    private readonly ILogger<DownloadFileLocallyAction> _logger;
    private readonly IFileDownloadService _fileDownloadService;
    private readonly LocalFileConfiguration _localFileConfiguration;

    public DownloadFileLocallyAction(
        ILogger<DownloadFileLocallyAction> logger,
        IFileDownloadService fileDownloadService,
        LocalFileConfiguration localFileConfiguration)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _fileDownloadService = fileDownloadService ?? throw new ArgumentNullException(nameof(fileDownloadService));
        _localFileConfiguration = localFileConfiguration ?? throw new ArgumentNullException(nameof(localFileConfiguration));
    }
    public async Task Handle(WorkItemExecutedArgs args)
    {
        _logger.LogInformation("Handling WorkItemExecutedArgs with {PublishedArgumentsCount} published arguments.", args.PublishedArguments.Count());
        try
        {
            foreach (var publishedArgument in args.PublishedArguments)
            {
                if (publishedArgument.ArgumentType != ArgumentType.Output)
                {
                    _logger.LogDebug("Skipping file with URL {FileUrl} due to argument type {ArgumentType}.", publishedArgument.FileSignedUrl, publishedArgument.ArgumentType);
                    continue;
                }
                _logger.LogInformation("Starting download of file with URL {FileUrl} to local path {LocalPath}.", publishedArgument.FileSignedUrl, _localFileConfiguration.Path);
                await _fileDownloadService.DownloadLocalAsync(publishedArgument.FileSignedUrl, _localFileConfiguration.Path);
                _logger.LogInformation("Successfully downloaded file with URL {FileUrl} to local path {LocalPath}.", publishedArgument.FileSignedUrl, _localFileConfiguration.Path);
            }
        }
        catch (Exception e)
        {
            _logger.LogError(e, "An error occurred while downloading files locally.");
        }
        
    }
}