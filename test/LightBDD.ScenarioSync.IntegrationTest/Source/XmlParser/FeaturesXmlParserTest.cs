using System.Xml.Serialization;
using FluentAssertions;
using LightBDD.ScenarioSync.IntegrationTest.TestData;
using LightBDD.ScenarioSync.Source.XmlParser.Models;

namespace LightBDD.ScenarioSync.IntegrationTest.Source.XmlParser;

public class FeaturesXmlParserTest
{
    [Fact]
    public async Task Parse_primitive_steps()
    {
        TestResults testResults = ParseTestResult();

        var features = testResults.Features.ToList();

        var feature = features.FirstOrDefault(f => f.Name == "Primitive parameters steps feature generate test report for parser");

        feature.Name.Should().NotBeNullOrEmpty();
        feature.Description.Should().NotBeNullOrEmpty();
        feature.Scenarios.Should().HaveCountGreaterThan(1);
        feature.Scenarios[0].Steps.Should().HaveCountGreaterThan(1);
        feature.Scenarios.Should().AllSatisfy(sc => { sc.Name.Should().NotBeNullOrEmpty(); });
    }

    [Fact]
    public async Task Parse_one_array_tree_parameter_step()
    {
        TestResults testResults = ParseTestResult();

        var features = testResults.Features.ToList();
        var featureName = "Complex parameters steps feature generate test report for parser";
        var feature = features.FirstOrDefault(f => f.Name == featureName);
        var scenarioName = "Scenario with tree parameters steps";
        var scenario = feature.Scenarios.FirstOrDefault(sc => sc.Name == scenarioName);
        var objectTreeParametersStepNumber = 1;
        var step = scenario.Steps.FirstOrDefault(st => st.Number == objectTreeParametersStepNumber);
        var parameterContact = step.Parameters.FirstOrDefault(p => p.Name == "contacts");
        parameterContact.Tree.Should().NotBeNull();
        parameterContact.Tree.Nodes.Should().HaveCountGreaterThan(1);
    }

    [Fact]
    public void Parse_scenario_with_one_level_sub_steps()
    {
        TestResults testResults = ParseTestResult();

        var features = testResults.Features.ToList();
        var featureName = "Scenarios with substeps";
        var feature = features.FirstOrDefault(f => f.Name == featureName);
        var scenarioName = "Scenario with one level sub steps";
        var scenario = feature.Scenarios.FirstOrDefault(sc => sc.Name == scenarioName);

        var stepWithSubSteps = scenario.Steps.FirstOrDefault(st => st.Name == "GIVEN customer is logged in");

        stepWithSubSteps.Should().NotBeNull();
        stepWithSubSteps.SubSteps.Should().HaveCount(5);
    }

    [Fact]
    public async Task Parse_one_table_parameter_step()
    {
        TestResults testResults = ParseTestResult();

        var features = testResults.Features.ToList();
        var featureName = "Complex parameters steps feature generate test report for parser";
        var feature = features.FirstOrDefault(f => f.Name == featureName);
        var scenarioName = "Scenario with table parameters steps";
        var scenario = feature.Scenarios.FirstOrDefault(sc => sc.Name == scenarioName);
        var oneTableParametersStepNumber = 1;
        var step = scenario.Steps.FirstOrDefault(st => st.Number == oneTableParametersStepNumber);
        var parameterContact = step.Parameters.FirstOrDefault(p => p.Name == "contacts");
        parameterContact.Table.Should().NotBeNull();
        parameterContact.Table.Columns.Should().HaveCountGreaterThan(1);
        parameterContact.Table.Rows.Should().HaveCountGreaterThan(1);
    }

    [Fact]
    public async Task Parse_metadata_labels()
    {
        TestResults testResults = ParseTestResult();

        var features = testResults.Features.ToList();
        var featureName = "Metadata feature generate test report for parser";
        var feature = features.FirstOrDefault(f => f.Name == featureName);
        var scenarioName = "Scenario automated test metadata";
        var scenario = feature.Scenarios.FirstOrDefault(sc => sc.Name == scenarioName);
        scenario.Labels[0].Name.Should().StartWith("Sync:");
    }

    private static TestResults ParseTestResult()
    {
        StreamReader streamReader = new StreamReader(TestDataReportResource.TestDataFeatureReportPath);
        var serializer = new XmlSerializer(typeof(TestResults));

        TestResults testResults = (TestResults)serializer.Deserialize(streamReader);
        return testResults;
    }
}