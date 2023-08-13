using LightBDD.ScenarioSync.Core.App;
using LightBDD.ScenarioSync.Core.Entities;
using LightBDD.ScenarioSync.Core.Sync.Source;
using LightBDD.ScenarioSync.Source.Factories;
using LightBDD.ScenarioSync.Source.XmlParser;
using LightBDD.ScenarioSync.Source.XmlParser.Models;
using AutomatedTestMetadata = LightBDD.ScenarioSync.Core.Entities.AutomatedTestMetadata;

namespace LightBDD.ScenarioSync.Source;

internal class TestCasesSource : ITestCasesSource
{
    private readonly IAppExecutionContext _appExecutionContext;
    private readonly FeaturesReportXmlParser _featuresXmlParser;

    public TestCasesSource(IAppExecutionContext appExecutionContext, FeaturesReportXmlParser featuresXmlParser)
    {
        _appExecutionContext = appExecutionContext;
        _featuresXmlParser = featuresXmlParser;
    }

    public IReadOnlyCollection<TestCase> Get()
    {
        string xmlReportPath = _appExecutionContext.Arguments.ReportPath;
        IReadOnlyCollection<Feature> features = _featuresXmlParser.Parse(xmlReportPath);

        IReadOnlyCollection<TestCase> testCases = features
            .SelectMany(GetTestCases)
            .Where(tc => tc.AutomatedMetadata != AutomatedTestMetadata.Empty)
            .ToList();

        return testCases;
    }

    private IReadOnlyCollection<TestCase> GetTestCases(Feature feature)
    {
        IReadOnlyCollection<string> labelsList = feature.Labels.Select(l => l.Name).ToList();
        Relations relations = RelationsFactory.Create(labelsList);
        Tags tagsList = TagsFactory.Create(labelsList);
        string rootTestSuite = _appExecutionContext.Arguments.RootTestSuite;
        var testSuitePath = new TestItemPath($"{rootTestSuite}\\{feature.Name}");
        TestSuite testSuite = new TestSuite(feature.Name, feature.Description, tagsList, relations, path: testSuitePath);
        return feature.Scenarios
            .Select(scenario => MapTestCase(scenario, testSuite))
            .ToList();
    }

    private static TestCase MapTestCase(Scenario scenario, TestSuite testSuite)
    {
        return TestCaseFactory.Create(scenario.Name, testSuite, scenario.Labels, scenario.Categories, scenario.Steps);
    }
}