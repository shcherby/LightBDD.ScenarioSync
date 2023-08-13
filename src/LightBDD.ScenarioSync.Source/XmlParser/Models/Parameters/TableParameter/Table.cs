using System.Xml.Serialization;

namespace LightBDD.ScenarioSync.Source.XmlParser.Models.Parameters.TableParameter;

[XmlRoot]
public class Table
{
    [XmlElement("Column")]
    public List<Column> Columns { get; set; }

    [XmlElement("Row")]
    public List<Row> Rows { get; set; }
}
