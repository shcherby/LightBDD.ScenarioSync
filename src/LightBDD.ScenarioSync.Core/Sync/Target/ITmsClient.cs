using LightBDD.ScenarioSync.Core.Entities;

namespace LightBDD.ScenarioSync.Core.Sync.Target;

/// <summary>
/// Test Management System Client
/// </summary>
public interface ITmsClient
{
    Task<IdName> GetOrCreateTestSuite(ITestSuite testSuite);
    Task<IReadOnlyList<IdName>> GetTestCases(IdName testSuiteId);
    Task<IReadOnlyList<IdName>> AddTestCases(IdName testSuiteId, IReadOnlyCollection<TestCase> testCases);
    Task UpdateTestCases(IdName testSuiteId, IReadOnlyCollection<TestCase> testCases);
    Task DeleteTestCases(IReadOnlyCollection<IdName> testCasesId);
    Task<IReadOnlyList<IdName>> GetTestSuites(TestItemPath rootTestSuitePath, bool excludeRoot = true);
    Task DeleteTestSuite(IdName id);
}