using System.Xml.Serialization;

namespace LightBDD.ScenarioSync.Source.XmlParser.Models.Parameters.TreeParameter;

[XmlRoot]
public class Tree
{
    [XmlElement("Node")]
    public List<Node> Nodes { get; set; }
}
