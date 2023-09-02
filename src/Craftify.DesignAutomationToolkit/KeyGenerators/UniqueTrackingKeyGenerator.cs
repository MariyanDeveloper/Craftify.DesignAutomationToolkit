using Craftify.DesignAutomationToolkit.Interfaces;

namespace Craftify.DesignAutomationToolkit.KeyGenerators;

public class UniqueTrackingKeyGenerator : IKeyGenerator
{
    public string Generate()
    {
        return Guid.NewGuid().ToString("N");
    }
}