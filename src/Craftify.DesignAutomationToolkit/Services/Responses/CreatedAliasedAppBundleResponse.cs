using Autodesk.Forge.DesignAutomation.Model;

namespace Craftify.DesignAutomationToolkit.Services.Responses;

public record CreatedAliasedAppBundleResponse(AppBundle AppBundle, Alias Alias);