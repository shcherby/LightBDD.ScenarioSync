using System.Xml.Serialization;

namespace LightBDD.ScenarioSync.Source.XmlParser.Models;

[XmlRoot]
public class TestResults
{
    public TestResults()
    {
        Features = new List<Feature>();
    }

    [XmlElement("Feature")]
    public List<Feature> Features { get; }
}
