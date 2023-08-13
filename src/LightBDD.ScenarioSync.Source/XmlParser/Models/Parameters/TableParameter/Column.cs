using System.Xml.Serialization;

namespace LightBDD.ScenarioSync.Source.XmlParser.Models.Parameters.TableParameter;

[XmlRoot]
public class Column : ObjectProperties
{
    [XmlAttribute]
    public int Index { get; set; }

    [XmlAttribute]
    public bool IsKey { get; set; }
}
