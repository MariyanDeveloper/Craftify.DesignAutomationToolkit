using Craftify.DesignAutomationToolkit.Services;
using Moq;

namespace Craftify.DesignAutomationToolkit.Tests.Helpers.AppBundle;

public static class AppBundleServiceMockBuilder
{
    public static Mock<IAppBundleService> Build(AppBundleServiceMockSettings mockSettings)
    {
        var mock = new Mock<IAppBundleService>();
        
        mock.Setup(x =>
            x.CreateAliasedAsync(
                It.IsAny<Autodesk.Forge.DesignAutomation.Model.AppBundle>(),
                It.IsAny<string>(),
                It.IsAny<string>()));
        
        mock.Setup(x =>
                x.UpdateAsync(
                    It.IsAny<Autodesk.Forge.DesignAutomation.Model.AppBundle>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()))
            .ReturnsAsync(() => mockSettings.NewUpdatedVersion);
        
        mock.Setup(x =>
            x.DeleteVersionAsync(
                It.IsAny<string>(),
                It.IsAny<int>()));
        
        mock.Setup(x =>
                x.TryGetAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>()))
            .ReturnsAsync(() => mockSettings.AppBundleSearchResult);
        return mock;

    }
}