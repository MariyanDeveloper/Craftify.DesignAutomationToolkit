using Craftify.DesignAutomation.Shared;

namespace Craftify.DesignAutomationToolkit.Publishers.Options;

public class PublishAppBundleOptions
{
    public Product Product { get; set; } = Product.Revit;
    public int ProductVersion { get; set; } = 2023;
    public string Description { get; set; } = "Default Description";
    public bool CleanUpPreviousVersion { get; set; } = true;
}