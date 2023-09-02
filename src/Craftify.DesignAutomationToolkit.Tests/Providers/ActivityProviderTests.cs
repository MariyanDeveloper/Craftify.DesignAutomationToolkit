using Craftify.DesignAutomationToolkit.Providers;
using Craftify.DesignAutomationToolkit.Tests.Fixtures;
using Craftify.DesignAutomationToolkit.Tests.Helpers;

namespace Craftify.DesignAutomationToolkit.Tests.Providers;

public class ActivityProviderTests : IClassFixture<DesignAutomationSettingFixture>
{
    private readonly DesignAutomationSettingFixture _designAutomationSettingFixture;
    
    public ActivityProviderTests(DesignAutomationSettingFixture designAutomationDesignAutomationSettingFixture)
    {
        _designAutomationSettingFixture = designAutomationDesignAutomationSettingFixture;
    }
    
    [Fact]
    public async Task Get_WithValidSettings_ReturnsExpectedActivity()
    {
        var nicknameProviderMock = MockBuilder.CreateNicknameProviderMock();
        var activityProvider = new ActivityProvider(nicknameProviderMock.Object);
        var activity = await activityProvider.Get(_designAutomationSettingFixture.DesignAutomationExecutionSetting);
        
        var qualifiedAppBundleName = "app.bundle_id+bundle_alias";
        var commandLine = """
                          $(engine.path)\\\\revitcoreconsole.exe /i \"$(args[rvtFile].path)\" \"$(args[paramFile].path)\" /al \"$(appbundles[bundle_id].path)\"
                          """;
        var engine = "Autodesk.Revit+2023";
        var appBundlesCount = 1;
        var commandLinesCount = 1;
        var parametersCount = 3;
        
        var isValid = activity.Appbundles.Count == appBundlesCount &&
                      activity.Appbundles[0] == qualifiedAppBundleName &&
                      activity.CommandLine.Count == commandLinesCount &&
                      activity.CommandLine[0] == commandLine;
        
        Assert.True(isValid);
    }

}