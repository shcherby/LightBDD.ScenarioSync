namespace LightBDD.ScenarioSync.Core.Entities;

public record TestCase : IdName
{
    private TestSuite _testSuite;

    public TestCase(
        string name,
        TestSuite testSuite,
        Tags? tags = null,
        IEnumerable<TestStep>? steps = null,
        Relations? relations = null,
        AutomatedTestMetadata? metadata = null,
        int id = 0)
        : base(id, name)
    {
        Tags = tags ?? new Tags(new List<string>());

        AutomatedMetadata = metadata ?? AutomatedTestMetadata.Empty;

        Relations = relations ?? new Relations(Enumerable.Empty<int>());
        _testSuite = testSuite ?? new TestSuite("");

        Steps = (steps ?? Enumerable.Empty<TestStep>()).ToList();
    }

    public Relations Relations { get; }

    public IReadOnlyCollection<string>? Tags { get; }

    public AutomatedTestMetadata AutomatedMetadata { get; }

    public IReadOnlyCollection<TestStep> Steps { get; }

    public TestSuite GetTestSuite() => _testSuite;
}