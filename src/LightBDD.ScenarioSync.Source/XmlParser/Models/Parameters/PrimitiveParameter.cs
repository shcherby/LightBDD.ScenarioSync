using System.Xml.Serialization;

namespace LightBDD.ScenarioSync.Source.XmlParser.Models.Parameters;

public class PrimitiveParameter
{
    [XmlAttribute]
    public string Value { get; set; }
    
    [XmlAttribute]
    public string Expectation { get; set; }
}