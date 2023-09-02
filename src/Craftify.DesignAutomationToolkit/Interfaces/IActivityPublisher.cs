using Autodesk.Forge.DesignAutomation.Model;
using Craftify.DesignAutomationToolkit.Publishers;
using Craftify.DesignAutomationToolkit.Publishers.Options;

namespace Craftify.DesignAutomationToolkit.Interfaces;

public interface IActivityPublisher
{
    Task<PublishedActivity> Publish(
        Activity activity,
        string alias,
        Action<PublishActivityOptions>? configOptions = null);
}