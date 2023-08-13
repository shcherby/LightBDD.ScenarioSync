namespace LightBDD.ScenarioSync.Source.Labels;

public static class SyncMetadataLabel
{
    private const string Example = $"{LabelPrefix}:StorageName;TestName";
    public const string LabelPrefix = "Sync:";

    public static (string storageName, string methodName) Parse(IEnumerable<string> labels)
    {
        string? automatedLabel = labels.FirstOrDefault(HasMetadata);

        if (string.IsNullOrEmpty(automatedLabel))
        {
            return new();
        }

        string[] metadata = automatedLabel.Split(':');

        if (metadata.Length < 2)
        {
            throw new ArgumentOutOfRangeException(nameof(labels), $"Invalid format {automatedLabel}, valid example is '{Example}'");
        }

        if (TryParseMetadata(metadata[1], out (string testStorageName, string testCaseMethodName) metadataValue))
        {
            return new(metadataValue.testStorageName, metadataValue.testCaseMethodName);
        }

        return new();
    }

    private static bool TryParseMetadata(string metadataValue, out (string, string) metadata)
    {
        metadata = ("", "");
        string[] path = metadataValue.Split(';');

        if (path.Length < 2)
        {
            return false;
        }

        metadata = (path[0], path[1]);
        return true;
    }

    public static bool HasMetadata(string label) => label.StartsWith(LabelPrefix);
}