using System.Net;
using Autodesk.Forge.Core;
using Autodesk.Forge.DesignAutomation.Http;
using Autodesk.Forge.DesignAutomation.Model;
using Moq;

namespace Craftify.DesignAutomationToolkit.Tests.Helpers.WorkItemsApi;

public static class WorkItemsApiMockBuilder
{

    public static Mock<IWorkItemsApi> CreateWorkItemsApiWithInstantSuccessfulStatus()
    {
        var mock = new Mock<IWorkItemsApi>();
        var createItemResponse = new ApiResponse<WorkItemStatus>(
            new HttpResponseMessage(HttpStatusCode.OK), 
            new WorkItemStatus { Id = "12345", Status = Status.Pending });
        mock.Setup(x =>
                x.CreateWorkItemAsync(
                    It.IsAny<WorkItem>(),
                    It.IsAny<string>(),
                    It.IsAny<Dictionary<string, string>>(),
                    It.IsAny<bool>()))
            .Returns(async () => createItemResponse);
        mock.Setup(x =>
                x.GetWorkitemStatusAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<IDictionary<string, string>>(),
                    It.IsAny<bool>()))
            .Returns(async (string id, string _, IDictionary<string, string> __, bool ___) => 
            {
                return new ApiResponse<WorkItemStatus>(
                    new HttpResponseMessage(HttpStatusCode.OK),
                    new WorkItemStatus { Id = id, Status = Status.Success });
                
            });
        
        mock.Setup(x => x.DeleteWorkItemAsync(
                It.IsAny<string>(), 
                It.IsAny<string>(), 
                It.IsAny<IDictionary<string, string>>(), 
                It.IsAny<bool>()))
            .Returns(() =>
            {
                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK));
            });
        return mock;
    }
    public static Mock<IWorkItemsApi> CreateWorkItemsApiWithScenarioOnThirdAttemptSuccessfulSuccess()
    {
        var mock = new Mock<IWorkItemsApi>();
        var createItemResponse = new ApiResponse<WorkItemStatus>(
            new HttpResponseMessage(HttpStatusCode.OK), 
            new WorkItemStatus { Id = "12345", Status = Status.Pending });
        mock.Setup(x =>
                x.CreateWorkItemAsync(
                    It.IsAny<WorkItem>(),
                    It.IsAny<string>(),
                    It.IsAny<Dictionary<string, string>>(),
                    It.IsAny<bool>()))
            .Returns(async () => createItemResponse);
        
        var attempt = 1;
        var isWorkItemDeletionRequested = false;
        
        mock.Setup(x =>
                x.GetWorkitemStatusAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<IDictionary<string, string>>(),
                    It.IsAny<bool>()))
            .Returns(async (string id, string _, IDictionary<string, string> __, bool ___) => 
            {
                await Task.Delay(TimeSpan.FromSeconds(1));
                
                if (isWorkItemDeletionRequested)
                    return new ApiResponse<WorkItemStatus>(
                        new HttpResponseMessage(HttpStatusCode.OK),
                        new WorkItemStatus { Id = id, Status = Status.Cancelled });
                if (attempt == 1)
                {
                    attempt++;
                    return new ApiResponse<WorkItemStatus>(
                        new HttpResponseMessage(HttpStatusCode.OK),
                        new WorkItemStatus { Id = id, Status = Status.Pending });
                }

                if (attempt == 2)
                {
                    attempt++;
                    return new ApiResponse<WorkItemStatus>(
                        new HttpResponseMessage(HttpStatusCode.OK),
                        new WorkItemStatus { Id = id, Status = Status.Inprogress });
                }
                return new ApiResponse<WorkItemStatus>(
                    new HttpResponseMessage(HttpStatusCode.OK),
                    new WorkItemStatus { Id = id, Status = Status.Success });
                
            });
        
        mock.Setup(x => x.DeleteWorkItemAsync(
                It.IsAny<string>(), 
                It.IsAny<string>(), 
                It.IsAny<IDictionary<string, string>>(), 
                It.IsAny<bool>()))
            .Returns(() =>
            {
                isWorkItemDeletionRequested = true;
                return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK));
            });

        return mock;
    }
    public static Mock<IWorkItemsApi> Build()
    {
        var mock = new Mock<IWorkItemsApi>();
        return mock;
    }
}