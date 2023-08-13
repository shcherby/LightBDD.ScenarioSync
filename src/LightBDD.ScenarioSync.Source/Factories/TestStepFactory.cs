using LightBDD.ScenarioSync.Core.Entities;
using LightBDD.ScenarioSync.Core.Entities.Parameters;
using LightBDD.ScenarioSync.Core.Entities.Parameters.TableParameter;
using LightBDD.ScenarioSync.Core.Entities.Parameters.TreeParameter;
using LightBDD.ScenarioSync.Source.Helpers;
using LightBDD.ScenarioSync.Source.XmlParser.Models;
using LightBDD.ScenarioSync.Source.XmlParser.Models.Parameters;
using FileAttachmentEntity = LightBDD.ScenarioSync.Core.Entities.FileAttachment;
using FileAttachmentXmlModel = LightBDD.ScenarioSync.Source.XmlParser.Models.FileAttachment;

namespace LightBDD.ScenarioSync.Source.Factories;

internal static class TestStepFactory
{
    public static TestStep Create(Step scenarioStep)
    {
        IReadOnlyList<Step> scenarioSubSteps = scenarioStep.SubSteps.ListOrDefault();
        var subSteps = new List<TestStep>();
        foreach (Step subStep in scenarioSubSteps)
        {
            IReadOnlyCollection<TestStep> subSteps2 = subStep.SubSteps.ListOrDefault().Select(Create).ToList();
            subSteps.Add(CreateStep(subStep, subSteps2));
        }

        TestStep testStep = CreateStep(scenarioStep, subSteps);

        return testStep;
    }

    private static TestStep CreateStep(Step scenarioStep, IReadOnlyCollection<TestStep> subSteps)
    {
        IEnumerable<TestStepParameter> testStepParameters = GetTestStepParameters(scenarioStep.Parameters.ListOrDefault());
        IReadOnlyList<FileAttachmentXmlModel> fileAttachments = scenarioStep.FileAttachments.ListOrDefault();
        var testStep = new TestStep(
            scenarioStep.Name,
            scenarioStep.Number,
            scenarioStep.GroupPrefix,
            testStepParameters,
            scenarioStep.Comments,
            fileAttachments.Select(f => new FileAttachmentEntity(f.Name, f.Path)),
            subSteps
        );
        return testStep;
    }

    private static IEnumerable<TestStepParameter> GetTestStepParameters(IReadOnlyList<Parameter> parameters)
    {
        var testStepParameters = new List<TestStepParameter>();
        foreach (Parameter parameter in parameters)
        {
            var treeDomain = new Tree(
                parameter.Tree?.Nodes?.Select(n => new Node(n.Path, n.Value, n.Value))
            );

            var tableDomain = new Table(
                MapToTableColumns(parameter.Table?.Columns),
                MapToTableRows(parameter.Table?.Rows)
            );

            var primitive = new Core.Entities.Parameters.PrimitiveParameter(parameter.Value?.Value, parameter.Value?.Expectation);

            testStepParameters.Add(new TestStepParameter(parameter.Name, treeDomain, tableDomain, primitive));
        }

        return testStepParameters;
    }

    private static IEnumerable<Column>? MapToTableColumns(IEnumerable<XmlParser.Models.Parameters.TableParameter.Column> columns)
    {
        return columns?.Select(n => new Column(n.Name, n.Index, n.IsKey));
    }

    private static IEnumerable<Row>? MapToTableRows(IEnumerable<XmlParser.Models.Parameters.TableParameter.Row> rows)
    {
        return rows?.Select(n => new Row(
            n.Index,
            n.Values.Select(v => new RowValue(v.Index, v.Value, v.Expectation))
        ));
    }
}