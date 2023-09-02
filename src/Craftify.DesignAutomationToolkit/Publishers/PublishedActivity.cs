using Craftify.DesignAutomationToolkit.Publishers.Statuses;

namespace Craftify.DesignAutomationToolkit.Publishers;

public record PublishedActivity(string Id, string Alias, int Version, ActivityStatus ActivityStatus);