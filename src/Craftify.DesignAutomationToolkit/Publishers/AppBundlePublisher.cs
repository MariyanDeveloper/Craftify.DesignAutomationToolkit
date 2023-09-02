using Autodesk.Forge.DesignAutomation.Model;
using Craftify.DesignAutomation.Shared.Extensions;
using Craftify.DesignAutomationToolkit.Interfaces;
using Craftify.DesignAutomationToolkit.Publishers.Options;
using Craftify.DesignAutomationToolkit.Publishers.Statuses;
using Craftify.DesignAutomationToolkit.Services;

namespace Craftify.DesignAutomationToolkit.Publishers;

public class AppBundlePublisher : IAppBundlePublisher
{
    private readonly IAppBundleService _appBundleService;

    public AppBundlePublisher(
        IAppBundleService appBundleService)
    {
        _appBundleService = appBundleService ?? throw new ArgumentNullException(nameof(appBundleService));
    }
    public async Task<PublishedAppBundle> Publish(
        string appBundleId,
        string appBundleAlias,
        string packagePath,
        Action<PublishAppBundleOptions> configAppBundleOptions = null)
    {
        if (appBundleId is null) throw new ArgumentNullException(nameof(appBundleId));
        if (appBundleAlias is null) throw new ArgumentNullException(nameof(appBundleAlias));
        var options = new PublishAppBundleOptions();
        configAppBundleOptions?.Invoke(options);
        ThrowIfPackagePathIsInvalid(packagePath);
        var appBundle = new AppBundle()
        {
            Id = appBundleId,
            Engine = options.Product.GetEngine(options.ProductVersion),
            Description = options.Description
        };
        var appBundleSearchResult = await _appBundleService
            .TryGetAsync(appBundleId, appBundleAlias);
        if (appBundleSearchResult.IsFound is false)
        {
            await _appBundleService
                .CreateAliasedAsync(appBundle, appBundleAlias, packagePath);
            return new PublishedAppBundle(
                appBundleId,
                appBundleAlias,
                1,
                AppBundleStatus.Created);
        }
        var publishedAppBundle = await CreateUpdatedPublishedAppBundle(
            appBundle,
            appBundleAlias,
            packagePath);
        if (options.CleanUpPreviousVersion)
        {
            await CleanUpVersionIfHas(new AppBundle()
            {
                Id = appBundleId,
                Version = appBundleSearchResult.AppBundle.Version
            });
        }
        return publishedAppBundle;
    }
    private async Task<int> UpdateExistingAppBundle(
        AppBundle existingAppBundle,
        string appBundleAlias,
        string bundlePath)
    {
        var newVersion = await _appBundleService.UpdateAsync(
            existingAppBundle, appBundleAlias, bundlePath);
        return newVersion;
    }

    private async Task CleanUpVersionIfHas(
        AppBundle existingAppBundle)
    {
        if(existingAppBundle.Version is not null)
        {
            await _appBundleService.DeleteVersionAsync(
                existingAppBundle.Id, existingAppBundle.Version.Value);
        }
    }
    private void ThrowIfPackagePathIsInvalid(string? bundlePath)
    {
        if (bundlePath is null)
        {
            throw new InvalidOperationException("Make sure to provide bundle path");
        }

        if (File.Exists(bundlePath) is false)
        {
            throw new InvalidOperationException($"Given bundle path {bundlePath} doesn't exist");
        }
    }
    private async Task<PublishedAppBundle> CreateUpdatedPublishedAppBundle(
        AppBundle appBundle,
        string appBundleAlias,
        string bundlePath)
    {
        var newVersion = await UpdateExistingAppBundle(
            appBundle,
            appBundleAlias,
            bundlePath);
        return new PublishedAppBundle(
            appBundle.Id,
            appBundleAlias,
            newVersion,
            AppBundleStatus.Updated);

    }
}