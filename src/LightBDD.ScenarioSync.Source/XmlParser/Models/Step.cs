using System.Xml.Serialization;
using LightBDD.ScenarioSync.Source.XmlParser.Models.Parameters;

namespace LightBDD.ScenarioSync.Source.XmlParser.Models;

[XmlRoot]
public class Step
{
    [XmlAttribute]
    public string Name { get; set; }

    [XmlAttribute]
    public int Number { get; set; }

    [XmlAttribute]
    public string GroupPrefix { get; set; }

    [XmlElement("Parameter")]
    public List<Parameter> Parameters { get; set; }
    
    [XmlElement("FileAttachment")]
    public List<FileAttachment> FileAttachments { get; set; }

    [XmlElement("Comment")]
    public List<string> Comments { get; set; }
    
    [XmlElement("SubStep")]
    public List<Step>? SubSteps { get; set; }
}
