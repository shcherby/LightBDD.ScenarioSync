using System.Xml.Serialization;

namespace LightBDD.ScenarioSync.Source.XmlParser.Models;

[XmlRoot]
public class Feature : ObjectProperties
{
    [XmlElement]
    public string? Description { get; set; }

    [XmlElement("Scenario")]
    public List<Scenario> Scenarios { get; set; }

    [XmlElement("Label")]
    public List<Label> Labels { get; set; }
}
