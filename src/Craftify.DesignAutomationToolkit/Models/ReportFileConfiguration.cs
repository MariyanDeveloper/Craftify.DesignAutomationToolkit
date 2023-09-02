namespace Craftify.DesignAutomationToolkit.Models;

public class ReportFileConfiguration
{
    public bool UseExecutingAssemblyLocation { get; set; } = true;
    public string? CustomPath { get; set; } = default;
}