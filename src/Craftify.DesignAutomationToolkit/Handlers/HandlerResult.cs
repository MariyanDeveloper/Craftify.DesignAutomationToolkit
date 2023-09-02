using Autodesk.Forge.DesignAutomation.Model;

namespace Craftify.DesignAutomationToolkit.Handlers;

public enum ArgumentType
{
    Input,
    Output
}
public record HandlerResult(IArgument Argument, ArgumentType ArgumentType, string? FileSignedUrl = default);