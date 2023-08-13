using System.Xml.Serialization;

namespace LightBDD.ScenarioSync.Source.XmlParser.Models.Parameters.TableParameter;

[XmlRoot]
public class RowValue
{
    [XmlAttribute]
    public int Index { get; set; }

    [XmlAttribute]
    public string Value { get; set; }
    
    [XmlAttribute]
    public string Expectation { get; set; }
}
