using Craftify.DesignAutomationToolkit.Publishers.Statuses;

namespace Craftify.DesignAutomationToolkit.Publishers;

public record PublishedAppBundle(string Id, string Alias, int Version, AppBundleStatus AppBundleStatus);