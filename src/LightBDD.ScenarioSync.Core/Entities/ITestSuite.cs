namespace LightBDD.ScenarioSync.Core.Entities;

public interface ITestSuite
{
    int Id { get; }
    TestItemPath Path { get; }
    string Name { get; }
    string Description { get; }
    Tags Tags { get; }
    Relations Relations { get; }
}