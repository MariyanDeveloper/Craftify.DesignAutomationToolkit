using Craftify.DesignAutomationToolkit.Constants;
using Craftify.DesignAutomationToolkit.Preferences;
using Craftify.DesignAutomationToolkit.Providers;
using Craftify.DesignAutomationToolkit.Settings;
using Craftify.DesignAutomationToolkit.Tests.Constants;

namespace Craftify.DesignAutomationToolkit.Tests.Fixtures;

public class DesignAutomationSettingFixture : IDisposable
{
    public DesignAutomationExecutionSetting DesignAutomationExecutionSetting { get; private set; }
    
    public DesignAutomationSettingFixture()
    {
        var pluginSetting = new PluginSetting()
        {
            Path = "",
            ParameterSettings = new List<BaseSetting>()
            {
                new InputSetting()
                {
                    Key = "rvtFile",
                    Value = ValidFilePaths.RevitProject,
                    LocalName = "input.rvt",
                    Description = "Input Revit Model",
                    Required = true
                },
                new InputSetting()
                {
                    Key = "paramFile",
                    Value = new
                    {
                        Height = 20,
                        Width = 10
                    },
                    LocalName = "params.json",
                    Description = "Json parameters",
                    Required = true
                },
                new OutputSetting()
                {
                    Key = "result",
                    Description = "Results",
                    LocalName = "result.rvt"
                }
            }
        };
        DesignAutomationExecutionSetting =  new DesignAutomationExecutionSetting()
        {
            PluginSetting = pluginSetting,
            ActivityPreferenceProvider = new NamedAliasPreferenceProvider("activity_id", "activity_alias"),
            AppBundlePreferenceProvider = new NamedAliasPreferenceProvider("bundle_id", "bundle_alias"),
            StoragePreferences = new StoragePreferences()
            {
                BucketIdPreference = new NamedIdPreference("custom_bucket_id")
            },
            WorkItemPreferences = new WorkItemPreferences()
            {
                WorkItemExecutionSettings = new WorkItemExecutionSettings()
                {
                    WorkItemExecutionPreference = WorkItemExecutions.Polling,
                    Arguments = new Dictionary<string, object>()
                    {
                        ["delay"] = 3
                    }
                }
            }
        };
    }


    public void Dispose()
    {
    }
}