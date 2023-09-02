using System.Reflection;
using Autodesk.Forge.DesignAutomation.Model;
using Craftify.DesignAutomationToolkit.Actions.Interfaces;
using Craftify.DesignAutomationToolkit.Models;
using Microsoft.Extensions.Logging;

namespace Craftify.DesignAutomationToolkit.Console;

public class ReportWorkItemExecutedAction : IWorkItemExecutedAction
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<ReportWorkItemExecutedAction> _logger;
    private readonly ReportFileConfiguration _reportFileConfiguration;
    private readonly string _reportFileExtension = "txt";

    public ReportWorkItemExecutedAction(
        HttpClient httpClient,
        ILogger<ReportWorkItemExecutedAction> logger,
        ReportFileConfiguration reportFileConfiguration)
    {
        _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _reportFileConfiguration = reportFileConfiguration ?? throw new ArgumentNullException(nameof(reportFileConfiguration));
    }
    public async Task Handle(WorkItemExecutedArgs args)
    {
        var workItemStatus = GetWorkItemStatus(args);
        var reportFileName = GetReportFileNameForStatus(workItemStatus);
        _logger.LogInformation("Starting report generation for WorkItem with ID {WorkItemId}.", workItemStatus.Id);
        try
        {
            await using var reportStream = await _httpClient.GetStreamAsync(workItemStatus.ReportUrl);
            await using var reportFileStream = new FileStream(reportFileName, FileMode.CreateNew);
            await reportStream.CopyToAsync(reportFileStream);
            
            _logger.LogInformation("Successfully saved report for WorkItem with ID {WorkItemId} to {ReportFilePath}.", workItemStatus.Id, reportFileName);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to generate report for WorkItem with ID {WorkItemId}.", workItemStatus.Id);
        }
    }
    private string GetReportFileNameForStatus(WorkItemStatus workItemStatus)
    {
        var reportName = FormatReportNameOfStatus(workItemStatus);
        if (_reportFileConfiguration.CustomPath is not null)
        {
            _logger.LogDebug("Using custom path {CustomPath} for report storage.", _reportFileConfiguration.CustomPath);
            return Path.Combine(_reportFileConfiguration.CustomPath, reportName);
        }
        var assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        _logger.LogDebug("Using executing assembly location {AssemblyPath} for report storage.", assemblyPath);
        return Path.Combine(assemblyPath, reportName);
    }

    private string FormatReportNameOfStatus(WorkItemStatus workItemStatus)
    {
        return $"Report: {DateTime.UtcNow:yyyy-MM-dd_HH-mm-ss}_{workItemStatus.Id}.{_reportFileExtension}";
    }
    private WorkItemStatus GetWorkItemStatus(WorkItemExecutedArgs args)
    {
        return args
            .WorkItemResult
            .Result
            .WorkItemStatus;
    }
}