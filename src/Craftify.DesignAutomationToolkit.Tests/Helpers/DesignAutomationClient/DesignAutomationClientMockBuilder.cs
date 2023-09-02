using Craftify.DesignAutomationToolkit.Extensions;
using Moq;

namespace Craftify.DesignAutomationToolkit.Tests.Helpers.DesignAutomationClient;

public static class DesignAutomationClientMockBuilder
{
    public static Mock<Autodesk.Forge.DesignAutomation.DesignAutomationClient> Build(DesignAutomationClientMockSettings mockSettings)
    {
        var mock = new Mock<Autodesk.Forge.DesignAutomation.DesignAutomationClient>();
        SetupTryGetActivity(mockSettings, mock);
        SetupCreateActivityAsync(mock);
        SetupUpdateActivityAsync(mockSettings, mock);
        SetupDeleteActivityVersionAsync(mock);
        return mock;
    }

    private static void SetupDeleteActivityVersionAsync(Mock<Autodesk.Forge.DesignAutomation.DesignAutomationClient> mock)
    {
        mock.Setup(x =>
            x.DeleteActivityVersionAsync(
                It.IsAny<string>(),
                It.IsAny<int>()));
    }

    private static void SetupUpdateActivityAsync(DesignAutomationClientMockSettings mockSettings, Mock<Autodesk.Forge.DesignAutomation.DesignAutomationClient> mock)
    {
        mock.Setup(x =>
                x.UpdateActivityAsync(
                    It.IsAny<Autodesk.Forge.DesignAutomation.Model.Activity>(),
                    It.IsAny<string>()))
            .ReturnsAsync(() => mockSettings.NewUpdatedVersion);
    }

    private static void SetupCreateActivityAsync(Mock<Autodesk.Forge.DesignAutomation.DesignAutomationClient> mock)
    {
        mock.Setup(x =>
            x.CreateActivityAsync(
                It.IsAny<Autodesk.Forge.DesignAutomation.Model.Activity>(),
                It.IsAny<string>()));
    }

    private static void SetupTryGetActivity(DesignAutomationClientMockSettings mockSettings, Mock<Autodesk.Forge.DesignAutomation.DesignAutomationClient> mock)
    {
        mock.Setup(x =>
                x.TryGetActivity(
                    It.IsAny<string>(),
                    It.IsAny<string>()))
            .ReturnsAsync(() => mockSettings.ActivitySearchResult);
    }
}