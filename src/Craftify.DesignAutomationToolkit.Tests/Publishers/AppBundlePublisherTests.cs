using Autodesk.Forge.DesignAutomation.Model;
using Craftify.DesignAutomationToolkit.Extensions.SearchResults;
using Craftify.DesignAutomationToolkit.Publishers;
using Craftify.DesignAutomationToolkit.Publishers.Statuses;
using Craftify.DesignAutomationToolkit.Tests.Constants;
using Craftify.DesignAutomationToolkit.Tests.Helpers.AppBundle;

namespace Craftify.DesignAutomationToolkit.Tests.Publishers;

public class AppBundlePublisherTests
{
    [Fact]
    public async Task Publish_WhenAppBundleAlreadyExists_ReturnsUpdatedPublishedAppBundle()
    {
        var appBundleServiceMock = AppBundleServiceMockBuilder
            .Build(new()
            {
                AppBundleSearchResult = new AppBundleSearchResult(true, new AppBundle()
                {
                    Id = "id",
                    Version = 1
                }),
                NewUpdatedVersion = 2
            });
        var appBundlePublisher = new AppBundlePublisher(appBundleServiceMock.Object);
        var publishedAppBundle = await appBundlePublisher.Publish("id", "alias", ValidFilePaths.RevitProject);

        var expectedVersion = 2;
        var expectedAppBundleStatus = AppBundleStatus.Updated;

        var isValid = publishedAppBundle.Version == expectedVersion &&
                      publishedAppBundle.AppBundleStatus == expectedAppBundleStatus;
        Assert.True(isValid);
    }
    
    [Fact]
    public async Task Publish_WhenAppBundleDoesntExist_ReturnsNewlyPublishedAppBundle()
    {
        var appBundleServiceMock = AppBundleServiceMockBuilder
            .Build(new()
            {
                AppBundleSearchResult = new AppBundleSearchResult(false),
            });
        var appBundlePublisher = new AppBundlePublisher(appBundleServiceMock.Object);
        var publishedAppBundle = await appBundlePublisher.Publish("id", "alias", ValidFilePaths.RevitProject);

        var expectedVersion = 1;
        var expectedAppBundleStatus = AppBundleStatus.Created;

        var isValid = publishedAppBundle.Version == expectedVersion &&
                      publishedAppBundle.AppBundleStatus == expectedAppBundleStatus;
        Assert.True(isValid);
    }
    
    [Fact]
    public async Task Publish_WhenPackagePathIsInvalid_ThrowsInvalidOperation()
    {
        var appBundleServiceMock = AppBundleServiceMockBuilder
            .Build(new()
            {
                AppBundleSearchResult = new AppBundleSearchResult(false),
            });
        var appBundlePublisher = new AppBundlePublisher(appBundleServiceMock.Object);
        Assert.ThrowsAsync<InvalidOperationException>(async () =>
            await appBundlePublisher.Publish("id", "alias", "invalidPath"));
    }
}