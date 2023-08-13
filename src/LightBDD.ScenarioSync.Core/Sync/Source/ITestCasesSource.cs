using LightBDD.ScenarioSync.Core.Entities;

namespace LightBDD.ScenarioSync.Core.Sync.Source;

public interface ITestCasesSource
{
    IReadOnlyCollection<TestCase> Get();
}
