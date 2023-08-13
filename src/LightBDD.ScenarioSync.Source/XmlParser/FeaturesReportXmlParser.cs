using System.Collections.ObjectModel;
using System.Xml.Serialization;
using LightBDD.ScenarioSync.Source.XmlParser.Models;

namespace LightBDD.ScenarioSync.Source.XmlParser;

internal class FeaturesReportXmlParser
{
    public IReadOnlyCollection<Feature> Parse(string path)
    {
        StreamReader streamReader = new StreamReader(path);
        var serializer = new XmlSerializer(typeof(TestResults));

        TestResults testResults = (TestResults)serializer.Deserialize(streamReader);

        return new ReadOnlyCollection<Feature>(testResults.Features);
    }
}
