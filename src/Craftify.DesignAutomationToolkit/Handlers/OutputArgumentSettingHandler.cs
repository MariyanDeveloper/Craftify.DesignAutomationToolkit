using Craftify.DesignAutomation.ModelBuilders.Builders;
using Craftify.DesignAutomationToolkit.Extensions;
using Craftify.DesignAutomationToolkit.Interfaces;
using Craftify.DesignAutomationToolkit.Settings;
using Craftify.ObjectStorageServiceToolkit.Enums;
using Craftify.ObjectStorageServiceToolkit.Interfaces;

namespace Craftify.DesignAutomationToolkit.Handlers;

public class OutputArgumentSettingHandler : IArgumentSettingHandler
{
    private readonly IObjectService _objectService;

    public OutputArgumentSettingHandler(IObjectService objectService)
    {
        _objectService = objectService ?? throw new ArgumentNullException(nameof(objectService));
    }
    public async Task<HandlerResult> Handle(BaseSetting baseSetting, HandlerStorageDetails handlerStorageDetails)
    {
        var localName = baseSetting.LocalName;
        var fileSignedUrl = await CreateSignedUrl(handlerStorageDetails.BucketId, localName);
        if (fileSignedUrl.IsUrlValid() is false)
        {
            throw new InvalidOperationException($"Given signed url {fileSignedUrl} is invalid");
        }
        var argument = new ArgumentBuilder()
            .BuildOutputSignedUrl(fileSignedUrl);
        return new HandlerResult(
            argument, ArgumentType.Output, fileSignedUrl);
    }
    private async Task<string> CreateSignedUrl(string bucketId, string localName)
    {
        var objectName = GetHashCode() + localName;
        var postObjectSigned = await _objectService.CreateSignedAsync(
            bucketId,
            objectName,
            options => { options.Access = Access.ReadWrite; });
        var signedUrl = postObjectSigned.SignedUrl;
        return signedUrl;
    }
}