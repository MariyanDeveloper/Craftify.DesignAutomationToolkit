using Craftify.DesignAutomationToolkit.Interfaces;

namespace Craftify.DesignAutomationToolkit.Handlers;

public class ReplaceFileHandler : IFileConflictHandler
{
    public void Handle(string path)
    {
        File.Delete(path);
    }
}