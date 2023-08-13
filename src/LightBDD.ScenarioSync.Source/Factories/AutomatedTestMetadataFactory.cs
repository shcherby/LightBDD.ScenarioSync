using LightBDD.ScenarioSync.Core.Entities;
using LightBDD.ScenarioSync.Source.Labels;
using LightBDD.ScenarioSync.Source.XmlParser.Models;

namespace LightBDD.ScenarioSync.Source.Factories;

internal static class AutomatedTestMetadataFactory
{
    public static AutomatedTestMetadata Create(IReadOnlyCollection<string> labels)
    {
        (string storageName, string methodName) metadata = SyncMetadataLabel.Parse(labels);
        return new AutomatedTestMetadata(metadata.storageName, metadata.methodName);
    }
}