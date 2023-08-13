using System.Xml.Serialization;

namespace LightBDD.ScenarioSync.Source.XmlParser.Models.Parameters.TableParameter;

[XmlRoot]
public class Row
{
    [XmlAttribute]
    public int Index { get; set; }

    [XmlElement("Value")]
    public List<RowValue> Values { get; set; }
}
