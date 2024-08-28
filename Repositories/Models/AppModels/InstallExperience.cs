namespace Repositories.Models.AppModels;

public class InstallExperience
{
    public object? AdditionalData { get; set; }
    public BackingStore BackingStore { get; set; } = default!;
    public int? DeviceRestartBehaviour { get; set; }
    public object ODataType { get; set; } = default!;
    public int? RunAsAccount { get; set; }
}