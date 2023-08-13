namespace LightBDD.ScenarioSync.Core.Entities;

public record TestSuite : IdName, ITestSuite
{
    public TestSuite(
        string name,
        string? description = "",
        Tags? tags = null,
        Relations? relations = null,
        IEnumerable<TestCase>? testCases = null,
        TestItemPath? path = null,
        int id = 0) : base(id, name)
    {
        Description = description ?? string.Empty;

        Tags = new Tags((tags ?? Enumerable.Empty<string>()).ToList());
        Relations = relations ?? new Relations(Enumerable.Empty<int>());
        Path = path ?? new TestItemPath(Name);
        TestCases = (testCases ?? Enumerable.Empty<TestCase>()).ToList();
    }

    public TestItemPath Path { get; }

    public string? Description { get; }

    public Relations Relations { get; }

    public Tags? Tags { get; }

    public IReadOnlyCollection<TestCase>? TestCases { get; }
}