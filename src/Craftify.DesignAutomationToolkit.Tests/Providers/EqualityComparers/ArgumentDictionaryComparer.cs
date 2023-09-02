using Autodesk.Forge.DesignAutomation.Model;

namespace Craftify.DesignAutomationToolkit.Tests.Providers.EqualityComparers;

public class ArgumentDictionaryComparer : IEqualityComparer<Dictionary<string, IArgument>>
{
    private readonly IEqualityComparer<IArgument> _argumentComparer;

    public ArgumentDictionaryComparer(IEqualityComparer<IArgument> argumentComparer)
    {
        _argumentComparer = argumentComparer ?? throw new ArgumentNullException(nameof(argumentComparer));
    }
    public bool Equals(Dictionary<string, IArgument> x, Dictionary<string, IArgument> y)
    {
        if (x is null || y is null)
        {
            return false;
        }
        if (x.Count != y.Count)
        {
            return false;
        }

        foreach (var kvp in x)
        {
            if (y.TryGetValue(kvp.Key, out var value) is false)
            {
                return false;
            }
            if (_argumentComparer.Equals(kvp.Value, value) is false)
            {
                return false;
            }
        }

        return true;
    }

    public int GetHashCode(Dictionary<string, IArgument> obj)
    {
        //Return 0 to make sure to call equals method each time
        return 0;
    }
}