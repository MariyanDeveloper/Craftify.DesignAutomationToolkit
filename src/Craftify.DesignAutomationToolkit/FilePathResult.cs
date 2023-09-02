namespace Craftify.DesignAutomationToolkit;

public record FilePathResult(bool Exists, string? FullPath = null, string? ErrorMessage = null);