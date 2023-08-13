using System.Xml.Serialization;

namespace LightBDD.ScenarioSync.Source.XmlParser.Models;

[XmlRoot]
public class FileAttachment
{
    [XmlAttribute]
    public string Name { get; set; }

    [XmlAttribute]
    public string Path { get; set; }
}