using Craftify.DesignAutomationToolkit.Extensions;

namespace Craftify.DesignAutomationToolkit;

public record QualifiedName(string Owner, string Name, string Alias)
{
    public override string ToString()
    {
        return Owner.CreateQualifiedName(Name, Alias);
    }
};