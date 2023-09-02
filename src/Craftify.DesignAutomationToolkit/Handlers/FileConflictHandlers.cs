using Craftify.DesignAutomationToolkit.Interfaces;

namespace Craftify.DesignAutomationToolkit.Handlers;

public static class FileConflictHandlers
{
    public static readonly IFileConflictHandler Default = new ReplaceFileHandler();
}