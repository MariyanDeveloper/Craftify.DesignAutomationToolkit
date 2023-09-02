using Craftify.DesignAutomationToolkit.Publishers;
using Craftify.DesignAutomationToolkit.Publishers.Options;

namespace Craftify.DesignAutomationToolkit.Interfaces;

public interface IAppBundlePublisher
{
    Task<PublishedAppBundle> Publish(
        string appBundleId,
        string appBundleAlias,
        string packagePath,
        Action<PublishAppBundleOptions> configAppBundleOptions = null);
}