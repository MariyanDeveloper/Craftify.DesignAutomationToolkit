namespace Craftify.DesignAutomationToolkit.Settings;

public abstract class BaseSetting
{
    public string Key { get; set; }
    public object Value { get; set; }
    public string LocalName { get; set; }
    public string Description { get; set; }
    public bool Required { get; set; }
}