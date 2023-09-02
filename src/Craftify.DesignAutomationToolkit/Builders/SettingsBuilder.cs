using Craftify.DesignAutomationToolkit.Settings;
using Craftify.DesignAutomationToolkit.Settings.Descriptors;

namespace Craftify.DesignAutomationToolkit.Builders;

public class SettingsBuilder
{
    private readonly List<BaseSetting> _settings = new();

    public SettingsBuilder Input(Action<InputSettingDescriptor> inputSettingDescriptor)
    {
        var descriptor = new InputSettingDescriptor();
        inputSettingDescriptor(descriptor);
        _settings.Add(descriptor.Setting);
        return this;
    }

    public SettingsBuilder Output(Action<OutputSettingDescriptor> outputSettingDescriptor)
    {
        var descriptor = new OutputSettingDescriptor();
        outputSettingDescriptor(descriptor);
        _settings.Add(descriptor.Setting);
        return this;
    }

    public List<BaseSetting> Build() => _settings;
}