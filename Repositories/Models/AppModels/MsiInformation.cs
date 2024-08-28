namespace Repositories.Models.AppModels;

public class MsiInformation
{
    public object? AdditionalData { get; set; }
    public BackingStore BackingStore { get; set; } = default!;
    public object ODataType { get; set; } = default!;
    public int? PackageType { get; set; }
    public string ProductCode { get; set; } = "";
    public string ProductName { get; set; } = "";
    public string ProductVersion { get; set; } = "";
    public string Publisher { get; set; } = "";
    public bool? RequiresReboot { get; set; }
    public string UpgradeCode { get; set; } = "";
}