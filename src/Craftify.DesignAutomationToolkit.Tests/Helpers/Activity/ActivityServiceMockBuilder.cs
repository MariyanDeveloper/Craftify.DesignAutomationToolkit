using Craftify.DesignAutomationToolkit.Interfaces;
using Moq;

namespace Craftify.DesignAutomationToolkit.Tests.Helpers.Activity;

public static class ActivityServiceMockBuilder
{
    public static Mock<IActivityService> Build(ActivityServiceMockSettings mockSettings)
    {
        var mock = new Mock<IActivityService>();
        
        mock.Setup(x =>
            x.CreateAliasedAsync(
                It.IsAny<Autodesk.Forge.DesignAutomation.Model.Activity>(),
                It.IsAny<string>()));
        
        mock.Setup(x =>
                x.UpdateAsync(
                    It.IsAny<Autodesk.Forge.DesignAutomation.Model.Activity>(),
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
            .ReturnsAsync(() => mockSettings.ActivitySearchResult);
        return mock;

    }
}