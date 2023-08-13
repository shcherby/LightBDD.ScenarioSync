using LightBDD.ScenarioSync.Core.Entities;
using LightBDD.ScenarioSync.Source.XmlParser.Models;

namespace LightBDD.ScenarioSync.Source.Factories;

internal static class TestCaseFactory
{
    public static TestCase Create(string name, TestSuite testSuite, IEnumerable<Label> labels, IEnumerable<Category> categories, IEnumerable<Step> steps)
    {
        IReadOnlyCollection<string> labelsList = labels.Select(l => l.Name).ToList();
        IReadOnlyCollection<string> categoriesList = categories.Select(l => l.Name).ToList();
        Relations relations = RelationsFactory.Create(labelsList);
        HashSet<string> tagsList = labelsList.Union(categoriesList).ToHashSet();
        Tags tags = TagsFactory.Create(tagsList);
        AutomatedTestMetadata metadata = AutomatedTestMetadataFactory.Create(labelsList);
        IEnumerable<TestStep> testSteps = steps.Select(TestStepFactory.Create);
        return new TestCase(name, testSuite, tags, testSteps, relations, metadata);
    }
}