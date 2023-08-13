namespace LightBDD.ScenarioSync.Source.Labels;

public class RelationsMetadataLabel
{
    public const string LabelPrefix = "Relations:";

    public static IList<int> Parse(IEnumerable<string> labels)
    {
        string? workItemsLabel = labels.FirstOrDefault(HasMetadata);

        if (string.IsNullOrEmpty(workItemsLabel))
        {
            return new List<int>();
        }

        string[] metadata = workItemsLabel.Split(':');

        if (metadata.Length < 2)
        {
            throw new ArgumentOutOfRangeException(nameof(labels), $"{LabelPrefix} should has valid format '{LabelPrefix}12,13,14'");
        }

        return ParseWorkItemsId(metadata[1]);
    }
    
    private static IList<int> ParseWorkItemsId(string workItemsLabel)
    {
        var workItemsIdList = new List<int>();
        string[] ids = workItemsLabel.Split(",", StringSplitOptions.RemoveEmptyEntries);
        foreach (string id in ids)
        {
            if (int.TryParse(id, out int workItemId))
            {
                workItemsIdList.Add(workItemId);
            }
        }

        return workItemsIdList.ToList();
    }

    public static bool HasMetadata(string label)
        => label.StartsWith(LabelPrefix);
}