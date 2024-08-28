namespace Repositories.Models.AppModels;

public class AppInfo
{
    public int? ApplicableArchitectures { get; set; }
    public string InstallCommandLine { get; set; } = "";
    public InstallExperience InstallExperience { get; set; } = default!;
    public string MinimumCpuSpeedInMHz { get; set; } = "";
    public string MinimumFreeDiskSpaceInMB { get; set; } = "";
    public string MinimumMemoryInMB { get; set; } = "";
    public string MinimumNumberOfProcessors { get; set; } = "";
    public string MinimumSupportedWindowsRelease { get; set; } = "";
    public MsiInformation MsiInformation { get; set; } = default!;
    public IEnumerable<ReturnCode> ReturnCodes { get; set; } = [];
    public IEnumerable<Rule> Rules { get; set; } = [];
    public string SetupFilePath { get; set; } = "";
    public string UninstallCommandLine { get; set; } = "";
    public string CommittedContentVersion { get; set; } = "";
    public object? ContentVersions { get; set; }
    public string FileName { get; set; } = "";
    public int? Size { get; set; }
    public object? Assignments { get; set; }
    public object? Categories { get; set; }
    public DateTime? CreatedDateTime { get; set; }
    public string Description { get; set; } = "";
    public string Developer { get; set; } = "";
    public string DisplayName { get; set; } = "";
    public object? InformationUrl { get; set; }
    public bool? IsFeatured { get; set; }
    public object? LargeIcon { get; set; }
    public DateTime? LastModifiedDateTime { get; set; }
    public string Notes { get; set; } = "";
    public string Owner { get; set; } = "";
    public object? PrivacyInformationUrl { get; set; }
    public string Publisher { get; set; } = "";
    public int? PublishingState { get; set; }
    public object? AdditionalData { get; set; }
    public BackingStore BackingStore { get; set; } = default!;
    public string Id { get; set; } = "";
    public string OdataType { get; set; } = "";
}