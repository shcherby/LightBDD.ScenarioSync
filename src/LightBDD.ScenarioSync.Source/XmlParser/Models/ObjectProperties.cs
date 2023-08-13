using System.Xml.Serialization;

namespace LightBDD.ScenarioSync.Source.XmlParser.Models;

public class ObjectProperties
{
    [XmlAttribute]
    public string Name { get; set; }
}
