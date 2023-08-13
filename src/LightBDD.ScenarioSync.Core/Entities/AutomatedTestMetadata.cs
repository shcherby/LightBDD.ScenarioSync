namespace LightBDD.ScenarioSync.Core.Entities;

public record AutomatedTestMetadata
{
    public static readonly AutomatedTestMetadata Empty = new();

    public AutomatedTestMetadata(string? storageName = "", string? methodName = "")
    {
        MethodName = methodName ?? "";
        StorageName = storageName ?? "";
    }

    public string? MethodName { get; }
    public string? StorageName { get; }
}