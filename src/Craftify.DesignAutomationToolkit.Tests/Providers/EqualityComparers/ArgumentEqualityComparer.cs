using Autodesk.Forge.DesignAutomation.Model;

namespace Craftify.DesignAutomationToolkit.Tests.Providers.EqualityComparers;

public class ArgumentEqualityComparer : IEqualityComparer<IArgument>
{
    public bool Equals(IArgument x, IArgument y)
    {
        if (x is null || y is null)
        {
            return false;
        }

        if (x is not XrefTreeArgument xrefArgX || y is not XrefTreeArgument xrefArgY)
        {
            return false;
        }
        return xrefArgX.Url == xrefArgY.Url && xrefArgX.Verb == xrefArgY.Verb;
        
    }

    public int GetHashCode(IArgument obj)
    {
        //Return 0 to make sure to call equals method each time
        return 0;
    }
}