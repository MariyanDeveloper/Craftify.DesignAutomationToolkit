using Craftify.DesignAutomationToolkit.Interfaces;
using Craftify.DesignAutomationToolkit.Settings;

namespace Craftify.DesignAutomationToolkit.Factories;

public class SettingHandlerFactory : ISettingHandlerFactory
{
    private readonly Dictionary<Type,ISettingHandlerActivator> _typeToActivatorMappings;

    public SettingHandlerFactory(IEnumerable<ISettingHandlerActivator> handlerActivators)
    {
        _typeToActivatorMappings = handlerActivators.ToDictionary(h => h.SettingType);
    }
    public IArgumentSettingHandler Create(Type settingType)
    {
        if (settingType is null)
        {
            throw new ArgumentNullException(nameof(settingType));
        }

        if (typeof(BaseSetting).IsAssignableFrom(settingType) is false)
        {
            throw new ArgumentException($"The provided type must be a subclass of Setting", nameof(settingType));
        }

        if (_typeToActivatorMappings.TryGetValue(settingType, out var activator) is false)
        {
            throw new InvalidOperationException($"There is no activator for this type: {settingType}");
        }
        return activator.Create();
    }
}