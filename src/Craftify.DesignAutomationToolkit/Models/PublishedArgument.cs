using Autodesk.Forge.DesignAutomation.Model;
using Craftify.DesignAutomationToolkit.Handlers;

namespace Craftify.DesignAutomationToolkit.Models;

public record PublishedArgument(IArgument Argument, ArgumentType ArgumentType, string FileSignedUrl);