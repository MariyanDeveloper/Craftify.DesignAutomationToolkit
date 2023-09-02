namespace Craftify.DesignAutomationToolkit.Interfaces;

public interface IFileConflictHandler
{
    void Handle(string path);
}