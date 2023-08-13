using System.Xml.Serialization;

namespace LightBDD.ScenarioSync.Source.XmlParser.Models.Parameters.TreeParameter;

[XmlRoot]
public class Node
{
    [XmlAttribute]
    public string Path { get; set; }

    [XmlAttribute]
    public string Value { get; set; }

    [XmlAttribute]
    public string Expectation { get; set; }
}
