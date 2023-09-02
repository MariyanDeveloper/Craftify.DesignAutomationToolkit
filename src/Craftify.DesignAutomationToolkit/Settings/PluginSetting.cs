namespace Craftify.DesignAutomationToolkit.Settings;

public class PluginSetting
{
    public string Path { get; set; }
    public List<BaseSetting> ParameterSettings { get; set; }
    public ProductSpecification ProductSpecification { get; set; } = new(DesignAutomation.Shared.Product.Revit, 2023);
}