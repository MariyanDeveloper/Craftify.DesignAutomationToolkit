using Autodesk.Forge.Model;
using Craftify.DesignAutomationToolkit.Interfaces;
using Craftify.DesignAutomationToolkit.Tests.Helpers.ObjectService;
using Craftify.ObjectStorageServiceToolkit.Interfaces;
using Craftify.ObjectStorageServiceToolkit.Options;
using Moq;

namespace Craftify.DesignAutomationToolkit.Tests.Helpers;

public static class MockBuilder
{
    
    
    
    public static Mock<IObjectService> CreateObjectServiceMock(ObjectServiceMockSettings? objectServiceMockSettings = null)
    {
        objectServiceMockSettings ??= new ObjectServiceMockSettings();
        var objectServiceMock = new Mock<IObjectService>();
        var expiration = 2;
        objectServiceMock.Setup(x =>
                x.UploadAsync(
                    It.IsAny<string>(),
        It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<Action<UploadObjectOptions>>()))
            .ReturnsAsync(new ObjectDetails());

        objectServiceMock.Setup(x =>
                x.UploadAsSignedAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<Action<UploadAsSignedOptions>>()))
            .ReturnsAsync(new PostObjectSigned(objectServiceMockSettings.InputUrl, expiration));

        objectServiceMock.Setup(x =>
                x.CreateSignedAsync(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<Action<CreateSignedObjectOptions>>()))
            .ReturnsAsync(new PostObjectSigned(objectServiceMockSettings.OutputUrl, expiration));
        
        return objectServiceMock;
    }
    public static Mock<INicknameProvider> CreateNicknameProviderMock()
    {
        var nicknameProviderMock = new Mock<INicknameProvider>();
        nicknameProviderMock.Setup(p => p.Get())
            .ReturnsAsync(new Nickname("app"));
        return nicknameProviderMock;
    }
}