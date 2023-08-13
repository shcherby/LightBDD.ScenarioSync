using LightBDD.ScenarioSync.Core.App;
using LightBDD.ScenarioSync.Core.Entities;
using LightBDD.ScenarioSync.Core.Sync.Source;
using LightBDD.ScenarioSync.Core.Sync.Target;

namespace LightBDD.ScenarioSync.Core.Sync;

internal class TestSuitesSynchronizer : ITestSuitesSynchronizer
{
    private readonly ITestCasesSource _testCasesSource;
    private readonly ITmsClient _tmsClient;
    private readonly TestItemPath _rootTestSuitePath;

    public TestSuitesSynchronizer(
        ITestCasesSource testCasesSource,
        ITmsClient tmsClient,
        IAppExecutionContext appExecutionContext)
    {
        _testCasesSource = testCasesSource;
        _tmsClient = tmsClient;
        _rootTestSuitePath = new TestItemPath(appExecutionContext.Arguments.RootTestSuite);
    }

    public async Task Sync()
    {
        IReadOnlyCollection<TestCase> testCases = _testCasesSource.Get();
        IReadOnlyCollection<IGrouping<TestSuite, TestCase>> reportTestSuites = testCases.GroupBy(tc => tc.GetTestSuite(), new TestSuitePathEqualityComparer()).ToList();

        foreach (IGrouping<TestSuite, TestCase> testSuiteGp in reportTestSuites)
        {
            IdName testSuiteId = await _tmsClient.GetOrCreateTestSuite(testSuiteGp.Key);
            IReadOnlyCollection<IdName> tmsTestCases = await _tmsClient.GetTestCases(testSuiteId);
            IReadOnlyCollection<TestCase> sourceTestCases = testSuiteGp.ToList();
            IReadOnlyCollection<string?> sourceTestSuiteTestCasesName = sourceTestCases.Select(tc => tc.Name).ToList();

            await AddTestCases(testSuiteId, tmsTestCases, sourceTestCases);

            // test cases that not exist in report but exist in external source
            List<IdName> testCasesToRemove = tmsTestCases
                .Where(tc => !sourceTestSuiteTestCasesName.Contains(tc.Name))
                .Select(tc => new IdName(tc.Id, tc.Name))
                .ToList();

            await UpdateTestCases(testSuiteId, sourceTestCases, tmsTestCases, testCasesToRemove);

            await DeleteTestCases(testCasesToRemove);
        }

        await DeleteTestSuitesMissedInSource(reportTestSuites);
    }

    public async Task Clean()
    {
        IReadOnlyList<IdName> tmTestSuites = await _tmsClient.GetTestSuites(_rootTestSuitePath);

        foreach (var ts in tmTestSuites)
        {
            await _tmsClient.DeleteTestSuite(ts);
        }
    }

    private async Task DeleteTestSuitesMissedInSource(IReadOnlyCollection<IGrouping<TestSuite, TestCase>> reportTestSuites)
    {
        IReadOnlyList<IdName> tmTestSuites = await _tmsClient.GetTestSuites(_rootTestSuitePath);
        ISet<string> reportTestSuitesNameSet = reportTestSuites.Select(ts => ts.Key.Name).ToHashSet();
        foreach (IdName tmTestSuite in tmTestSuites)
        {
            if (!reportTestSuitesNameSet.Contains(tmTestSuite.Name))
            {
                await _tmsClient.DeleteTestSuite(tmTestSuite);
            }
        }
    }

    private async Task UpdateTestCases(
        IdName testSuiteId,
        IReadOnlyCollection<TestCase> reportTestCases,
        IReadOnlyCollection<IdName> tmTestCasesIdName,
        IReadOnlyCollection<IdName> testCasesToRemove)
    {
        List<string> tmTestCasesName = tmTestCasesIdName.Select(idName => idName.Name).ToList();
        List<string> testCasesToRemoveNames = testCasesToRemove.Select(idName => idName.Name).ToList();
        // testcases exist in report and in external source and not for delete
        List<TestCase> testCasesToUpdate = reportTestCases.Where(tc => tmTestCasesName.Contains(tc.Name) && !testCasesToRemoveNames.Contains(tc.Name)).ToList();

        if (testCasesToUpdate.Any())
        {
            await _tmsClient.UpdateTestCases(testSuiteId, testCasesToUpdate);
        }
    }

    private async Task AddTestCases(IdName testSuiteId, IReadOnlyCollection<IdName> tmsTestCases, IReadOnlyCollection<TestCase> sourceTestCases)
    {
        List<string> tmsTestCasesName = tmsTestCases.Select(tc => tc.Name).ToList();
        // testcases exist in report and not exist in external source
        List<TestCase> testCasesToAdd = sourceTestCases.Where(tc => !tmsTestCasesName.Contains(tc.Name)).ToList();
        if (testCasesToAdd.Any())
        {
            await _tmsClient.AddTestCases(testSuiteId, testCasesToAdd);
        }
    }

    private async Task DeleteTestCases(IReadOnlyCollection<IdName> testCasesToRemove)
    {
        if (testCasesToRemove.Any())
        {
            await _tmsClient.DeleteTestCases(testCasesToRemove);
        }
    }

    private class TestSuitePathEqualityComparer : IEqualityComparer<TestSuite>
    {
        public bool Equals(TestSuite x, TestSuite y)
        {
            return x.Path.ToString() == y.Path.ToString();
        }

        public int GetHashCode(TestSuite obj)
        {
            return obj.Path.ToString().GetHashCode();
        }
    }
}