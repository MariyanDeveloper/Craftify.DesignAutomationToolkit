using Craftify.DesignAutomationToolkit.Handlers;
using Craftify.DesignAutomationToolkit.Interfaces;

namespace Craftify.DesignAutomationToolkit;

public class LocalDownloadOptions
{
    public IFileConflictHandler FileConflictHandler { get; set; } = FileConflictHandlers.Default;
}