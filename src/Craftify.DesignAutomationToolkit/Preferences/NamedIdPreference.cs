using Craftify.DesignAutomationToolkit.Interfaces;

namespace Craftify.DesignAutomationToolkit.Preferences;

public class NamedIdPreference : IIdPreference
{
    private readonly string _id;

    public NamedIdPreference(string id)
    {
        _id = id ?? throw new ArgumentNullException(nameof(id));
    }
    public string Get()
    {
        return _id;
    }
}