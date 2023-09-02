using Craftify.DesignAutomationToolkit.Interfaces;
using Craftify.DesignAutomationToolkit.Preferences;

namespace Craftify.DesignAutomationToolkit.Providers;

public class NamedAliasPreferenceProvider : INamedAliasPreferenceProvider
{
    private readonly string _name;
    private readonly string _alias;

    public NamedAliasPreferenceProvider(string name, string alias)
    {
        _name = name ?? throw new ArgumentNullException(nameof(name));
        _alias = alias ?? throw new ArgumentNullException(nameof(alias));
    }
    public NamedAliasPreference Get()
    {
        return new NamedAliasPreference(_name, _alias);
    }
}