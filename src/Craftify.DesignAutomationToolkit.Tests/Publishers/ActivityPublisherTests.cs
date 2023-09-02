using Autodesk.Forge.DesignAutomation.Model;
using Craftify.DesignAutomationToolkit.Extensions.SearchResults;
using Craftify.DesignAutomationToolkit.Publishers;
using Craftify.DesignAutomationToolkit.Publishers.Statuses;
using Craftify.DesignAutomationToolkit.Tests.Helpers.Activity;

namespace Craftify.DesignAutomationToolkit.Tests.Publishers;

public class ActivityPublisherTests
{
    [Fact]
    public async Task Publish_WhenActivityAlreadyExists_ReturnsUpdatedPublishedActivity()
    {
        var id = "activity_test";
        var alias = "alias";
        var activity = new Activity()
        {
            Id = id,
            Version = 1
        };
        var mockActivityService = ActivityServiceMockBuilder
            .Build(new ()
            {
                ActivitySearchResult = new ActivitySearchResult(true, new Activity()
                {
                    Id = id,
                    Version = 1
                }),
                NewUpdatedVersion = 2
            });
        var activityPublisher = new ActivityPublisher(mockActivityService.Object);
        var publishedActivity = await activityPublisher.Publish(activity, alias);
        var expectedVersion = 2;
        var expectedStatus = ActivityStatus.Updated;
        
        var isValid = publishedActivity.Version == expectedVersion &&
                      publishedActivity.ActivityStatus == ActivityStatus.Updated;
        Assert.True(isValid);
    }

    [Fact]
    public async Task Publish_WhenActivityDoesntExist_ReturnsNewlyPublishedActivity()
    {
        var id = "activity_test";
        var alias = "alias";
        var activity = new Activity()
        {
            Id = id,
            Version = 1
        };
        var mockActivityService = ActivityServiceMockBuilder
            .Build(new ()
            {
                ActivitySearchResult = new ActivitySearchResult(false),
            });
        var activityPublisher = new ActivityPublisher(mockActivityService.Object);
        var publishedActivity = await activityPublisher.Publish(activity, alias);
        var expectedVersion = 1;
        var expectedStatus = ActivityStatus.Created;
        
        var isValid = publishedActivity.Version == expectedVersion &&
                      publishedActivity.ActivityStatus == expectedStatus;
        Assert.True(isValid);
    }
}