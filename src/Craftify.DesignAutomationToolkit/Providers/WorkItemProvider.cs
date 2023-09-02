using Autodesk.Forge.DesignAutomation.Model;
using Craftify.DesignAutomation.ModelBuilders.Builders;
using Craftify.DesignAutomationToolkit.Handlers;
using Craftify.DesignAutomationToolkit.Interfaces;
using Craftify.DesignAutomationToolkit.Preferences;
using Craftify.DesignAutomationToolkit.Settings;

namespace Craftify.DesignAutomationToolkit.Providers;

public class WorkItemProvider: IWorkItemProvider
{
    private readonly INicknameProvider _nicknameProvider;
    private readonly ISettingHandlerFactory _settingHandlerFactory;

    public WorkItemProvider(
        INicknameProvider nicknameProvider,
        ISettingHandlerFactory settingHandlerFactory)
    {
        _nicknameProvider = nicknameProvider ?? throw new ArgumentNullException(nameof(nicknameProvider));
        _settingHandlerFactory = settingHandlerFactory ?? throw new ArgumentNullException(nameof(settingHandlerFactory));
    }
    public async Task<WorkItem> Get(
        DesignAutomationExecutionSetting designAutomationExecutionSetting,
        Action<HandlerResult>? onArgumentResultHandled = default)
    {
        var settings = designAutomationExecutionSetting
            .PluginSetting
            .ParameterSettings;
        var bucketId = designAutomationExecutionSetting
            .StoragePreferences
            .BucketIdPreference.Get();
        var arguments = await HandleCreationOfArguments(
            settings,
            bucketId,
            onArgumentResultHandled);
        var activityPreference = GetActivityPreference(designAutomationExecutionSetting);
        var nickname = await _nicknameProvider.Get();
        var qualifiedName = new QualifiedName(
            nickname.Name,
            activityPreference.Name,
            activityPreference.Alias);
        var qualifiedActivityName = qualifiedName.ToString();
        var workItem = new WorkItem()
        {
            ActivityId = qualifiedActivityName,
            Arguments = arguments
        };
        return workItem;
    }

    private NamedAliasPreference GetActivityPreference(DesignAutomationExecutionSetting designAutomationExecutionSetting)
    {
        var activityPreference = designAutomationExecutionSetting
            .ActivityPreferenceProvider
            .Get();
        return activityPreference;
    }
    

    private async Task<Dictionary<string, IArgument>> HandleCreationOfArguments(
        List<BaseSetting> settings,
        string bucketId,
        Action<HandlerResult>? onArgumentResultHandled = default)
    {
        var argumentsBuilder = new ArgumentsBuilder();
        foreach (var setting in settings)
        {
            var settingHandler = _settingHandlerFactory.Create(setting.GetType());
            var handleResult = await settingHandler.Handle(setting, new HandlerStorageDetails(bucketId));
            onArgumentResultHandled?.Invoke(handleResult);
            argumentsBuilder.Add(setting.Key, handleResult.Argument);
        }
        var arguments = argumentsBuilder.Build();
        return arguments;
    }
}