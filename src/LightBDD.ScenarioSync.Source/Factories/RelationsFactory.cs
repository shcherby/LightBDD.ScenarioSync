using LightBDD.ScenarioSync.Core.Entities;
using LightBDD.ScenarioSync.Source.Labels;
using LightBDD.ScenarioSync.Source.XmlParser.Models;

namespace LightBDD.ScenarioSync.Source.Factories;

internal static class RelationsFactory
{
    public static Relations Create(IReadOnlyCollection<string> labels)
    {
        IList<int> relationsId = RelationsMetadataLabel.Parse(labels);
        return new Relations(relationsId);
    }
}