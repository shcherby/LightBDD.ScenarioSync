using System.Xml.Serialization;

namespace LightBDD.ScenarioSync.Source.XmlParser.Models;

[XmlRoot]
public class Scenario : ObjectProperties
{
    [XmlElement("Label")]
    public List<Label> Labels { get; set; }
    
    [XmlElement("Category")]
    public List<Category> Categories { get; set; }
    

    [XmlElement("Step")]
    public List<Step> Steps { get; set; }
}
