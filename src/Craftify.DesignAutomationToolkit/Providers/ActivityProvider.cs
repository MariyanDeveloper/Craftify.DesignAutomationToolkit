using Autodesk.Forge.DesignAutomation.Model;
using Craftify.DesignAutomation.ModelBuilders.Builders;
using Craftify.DesignAutomation.ModelBuilders.Builders.CommandLineBuilderStages;
using Craftify.DesignAutomation.Shared;
using Craftify.DesignAutomationToolkit.Extensions;
using Craftify.DesignAutomationToolkit.Interfaces;
using Craftify.DesignAutomationToolkit.Preferences;
using Craftify.DesignAutomationToolkit.Settings;

namespace Craftify.DesignAutomationToolkit.Providers;

public class ActivityProvider : IActivityProvider
{
    private readonly INicknameProvider _nicknameProvider;

    public ActivityProvider(INicknameProvider nicknameProvider)
    {
        _nicknameProvider = nicknameProvider ?? throw new ArgumentNullException(nameof(nicknameProvider));
    }


    public async Task<Activity> Get(DesignAutomationExecutionSetting designAutomationExecutionSetting)
    {
        var pluginSetting = designAutomationExecutionSetting.PluginSetting;
        var parametersBuilder = new ParametersBuilder();
        var productSpecification = pluginSetting.ProductSpecification;
        var commandLineBuilder = CommandLineBuilder
            .Create()
            .ForProduct(productSpecification.Product)
            .IntroduceInputFlag();
        foreach (var setting in pluginSetting.ParameterSettings)
        {
            var parameter = CreateParameterBySetting(setting, inputAdded =>
            {
                commandLineBuilder.Append(inputAdded.Key);
            });
            parametersBuilder.Add(setting.Key, parameter);
        }
        var parameters = parametersBuilder.Build();
        var nickname = await _nicknameProvider.Get();
        var appBundlePreference = GetAppBundlePreference(designAutomationExecutionSetting);
        var activityPreference = GetActivityPreference(designAutomationExecutionSetting);
        var qualifiedName = new QualifiedName(
            nickname.Name,
            appBundlePreference.Name,
            appBundlePreference.Alias);
        var qualifiedAppBundleName = qualifiedName.ToString();
        var commandLine = ((IBuildWithArgumentsStage)commandLineBuilder)
            .IncludeAppBundle(appBundlePreference.Name)
            .Build();
        return new ActivityBuilder()
            .OfName(activityPreference.Name)
            .ForEngineOfProduct(Product.Revit)
            .OfProductVersion(productSpecification.Version)
            .WithCommandLine(commandLine)
            .IncludeParameters(parameters)
            .ForAppBundles(qualifiedAppBundleName)
            .Build();

    }

    private NamedAliasPreference GetActivityPreference(DesignAutomationExecutionSetting designAutomationExecutionSetting)
    {
        var activityPreference = designAutomationExecutionSetting
            .ActivityPreferenceProvider
            .Get();
        return activityPreference;
    }

    private NamedAliasPreference GetAppBundlePreference(DesignAutomationExecutionSetting designAutomationExecutionSetting)
    {
        var appBundlePreference = designAutomationExecutionSetting
            .AppBundlePreferenceProvider
            .Get();
        return appBundlePreference;
    }

    private Parameter CreateParameterBySetting(BaseSetting setting, Action<InputSetting> onInputParameterBuiltCallback)
    {
        if (setting is InputSetting inputSetting)
        {
            var inputParameter = new ParameterBuilder()
                .BuildInput(setting.LocalName, options =>
                {
                    options.ApplyOptionsSettings(setting);
                });
            onInputParameterBuiltCallback(inputSetting);
            return inputParameter;
        }
        return new ParameterBuilder()
            .BuildOutput(setting.LocalName, options =>
            {
                options.ApplyOptionsSettings(setting);
            });
    }
}