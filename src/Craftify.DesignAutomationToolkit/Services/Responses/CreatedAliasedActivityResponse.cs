using Autodesk.Forge.DesignAutomation.Model;

namespace Craftify.DesignAutomationToolkit.Services.Responses;

public record CreatedAliasedActivityResponse(Activity Activity, Alias Alias);