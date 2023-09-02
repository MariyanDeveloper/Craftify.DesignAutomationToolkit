using Autodesk.Forge.DesignAutomation.Model;
using Craftify.DesignAutomation.ModelBuilders.Builders;
using Craftify.DesignAutomationToolkit.Extensions;
using Craftify.DesignAutomationToolkit.Interfaces;
using Craftify.DesignAutomationToolkit.Settings;
using Craftify.ObjectStorageServiceToolkit.Interfaces;

namespace Craftify.DesignAutomationToolkit.Handlers;

public class InputArgumentSettingHandler : IArgumentSettingHandler
{
    private readonly IObjectService _objectService;

    public InputArgumentSettingHandler(IObjectService objectService)
    {
        _objectService = objectService ?? throw new ArgumentNullException(nameof(objectService));
    }
    public async Task<HandlerResult> Handle(BaseSetting baseSetting, HandlerStorageDetails handlerStorageDetails)
    {
        var localName = baseSetting.LocalName;
        var value = baseSetting.Value;
        if (value is null)
        {
            throw new NullReferenceException($"{nameof(value)} cannot be null. Make sure to provide value for input parameters");
        }
        if (value is not string filePath)
        {
            return CreateJsonHandleResult(value);
        }
        var filePathResult = filePath.GetFilePathResult();
        if (filePathResult.Exists is false)
        {
            return CreateJsonHandleResult(filePath);
        }
        var fileSignedUrl = await GetSignedUrl(handlerStorageDetails.BucketId, localName, filePath);
        if (fileSignedUrl.IsUrlValid() is false)
        {
            return CreateJsonHandleResult(filePath);
        }
        var argument = new ArgumentBuilder()
            .BuildInputSignedUrl(fileSignedUrl);
        
        return CreateHandleResult(argument, fileSignedUrl);
    }
    private HandlerResult CreateJsonHandleResult(object @object)
    {
        return CreateHandleResult(CreateJsonArgument(@object));
    }
    private HandlerResult CreateHandleResult(IArgument argument, string? fileSignedUrl = default) =>
        new(argument, ArgumentType.Input, fileSignedUrl); 
    private IArgument CreateJsonArgument(object @object) =>
        new ArgumentBuilder().BuildJson(@object);
    
    private async Task<string> GetSignedUrl(string bucketId, string localName, string filePath)
    {
        var postObjectSigned = await _objectService.UploadAsSignedAsync(
            bucketId,
            localName,
            filePath);
        return postObjectSigned.SignedUrl;
    }
}