using LightBDD.ScenarioSync.Core.Entities;
using LightBDD.ScenarioSync.Source.Labels;

namespace LightBDD.ScenarioSync.Source.Factories;

public static class TagsFactory
{
    public static Tags Create(IReadOnlyCollection<string> labels)
    {
        List<string> tags = labels
            .Where(l =>
                !SyncMetadataLabel.HasMetadata(l)
                && !RelationsMetadataLabel.HasMetadata(l))
            .Select(l => l)
            .ToList();
        return new Tags(tags);
    }
}