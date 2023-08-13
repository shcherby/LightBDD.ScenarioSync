using System.Xml.Serialization;
using LightBDD.ScenarioSync.Source.XmlParser.Models.Parameters.TableParameter;
using LightBDD.ScenarioSync.Source.XmlParser.Models.Parameters.TreeParameter;

namespace LightBDD.ScenarioSync.Source.XmlParser.Models.Parameters;

[XmlRoot]
public class Parameter : ObjectProperties
{
    [XmlElement]
    public Tree Tree { get; set; }

    [XmlElement]
    public Table Table { get; set; }

    [XmlElement]
    public PrimitiveParameter Value { get; set; }
}
