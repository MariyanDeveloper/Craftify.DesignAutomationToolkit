namespace Craftify.DesignAutomationToolkit.Settings.Descriptors;

public abstract class BaseDescriptor<TSetting> where TSetting: BaseSetting, new()
{
    internal TSetting Setting { get; } = new ();
    
    public BaseDescriptor<TSetting> WithKey(string key)
    {
        Setting.Key = key;
        return this;
    }
    public BaseDescriptor<TSetting> OfValue(object value)
    {
        Setting.Value = value;
        return this;
    }

    public BaseDescriptor<TSetting> WithLocalName(string localName)
    {
        Setting.LocalName = localName;
        return this;
    }
    public BaseDescriptor<TSetting>  OfDescription(string description)
    {
        Setting.Description = description;
        return this;
    }
    public BaseDescriptor<TSetting> Required()
    {
        Setting.Required = true;
        return this;
    }
}