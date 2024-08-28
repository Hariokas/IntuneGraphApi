namespace Repositories.Models.AppModels;

public class ReturnCode
{
    public object? AdditionalData { get; set; }
    public BackingStore BackingStore { get; set; } = default!;
    public object ODataType { get; set; } = default!;
    public int? ReturnCodeValue { get; set; }
    public int? Type { get; set; }
}