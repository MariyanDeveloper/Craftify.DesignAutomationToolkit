using Craftify.DesignAutomation.ModelBuilders.Builders.Options;
using Craftify.DesignAutomationToolkit.Settings;

namespace Craftify.DesignAutomationToolkit.Extensions;

public static class ParameterExtensions
{
    public static void ApplyOptionsSettings(this ParameterOptions parameterOptions,
        BaseSetting baseSetting)
    {
        parameterOptions.Description = baseSetting.Description;
        parameterOptions.Required = baseSetting.Required;
    }
}