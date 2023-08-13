using FluentAssertions;
using LightBDD.ScenarioSync.Core.App;
using LightBDD.ScenarioSync.Core.Entities;
using LightBDD.ScenarioSync.Core.Sync;
using LightBDD.ScenarioSync.Core.Sync.Source;
using LightBDD.ScenarioSync.Core.Sync.Target;
using LightBDD.ScenarioSync.UnitTest.Comparers;
using Moq;
using Xunit;

namespace LightBDD.ScenarioSync.UnitTest.Core.Sync;

public class Test_suites_synchronization_tests
{
    private readonly Mock<ITestCasesSource> _testCasesSourceMock;
    private readonly Mock<ITmsClient> _tmsClientMock;
    private readonly Mock<IAppExecutionContext> _appExecutionContextMock;

    public Test_suites_synchronization_tests()
    {
        _testCasesSourceMock = new Mock<ITestCasesSource>(MockBehavior.Strict);
        _tmsClientMock = new Mock<ITmsClient>(MockBehavior.Strict);
        _tmsClientMock.Setup(m => m.DeleteTestSuite(It.IsAny<IdName>())).Returns(Task.CompletedTask);
        _tmsClientMock.Setup(m => m.GetTestSuites(It.IsAny<TestItemPath>(), true)).ReturnsAsync(new List<IdName>());
        _appExecutionContextMock = new Mock<IAppExecutionContext>(MockBehavior.Default);
        _appExecutionContextMock.Setup(m => m.Arguments.RootTestSuite).Returns(String.Empty);
    }

    [Fact]
    public async Task Add_two_test_cases_to_test_suite()
    {
        IReadOnlyList<TestCase> newTestCases =
            SetupTestCasesSourceMock(
                new()
                {
                    ("test suite name 1", new[] { "test case name 1", "test case name 2" })
                });

        IReadOnlyList<IdName> testSuitesIdNames =
            SetupGetOrCreateTestSuiteMock(newTestCases);

        _tmsClientMock.Setup(m => m.GetTestCases(testSuitesIdNames[0]))
            .ReturnsAsync(new List<IdName>());

        _tmsClientMock.Setup(m => m.AddTestCases(testSuitesIdNames[0], newTestCases))
            .ReturnsAsync(newTestCases.Select(CreateIdName).ToList);

        await CreateSynchronizer().Sync();

        _tmsClientMock.Verify(m => m.AddTestCases(testSuitesIdNames[0], newTestCases), Times.Once);
        _tmsClientMock.Verify(m => m.UpdateTestCases(testSuitesIdNames[0], It.IsAny<IReadOnlyCollection<TestCase>>()), Times.Never);
        _tmsClientMock.Verify(m => m.DeleteTestCases(It.IsAny<IReadOnlyCollection<IdName>>()), Times.Never);
    }

    [Fact]
    public async Task Update_one_test_cases_to_test_suite()
    {
        IReadOnlyList<TestCase> sourceTestCases =
            SetupTestCasesSourceMock(
                new()
                {
                    ("test suite name 1", new[] { "test case name 1", "test case name 2" })
                });

        IReadOnlyList<IdName> testSuitesIdNames =
            SetupGetOrCreateTestSuiteMock(sourceTestCases);

        IReadOnlyList<TestCase> existTestCases = CreateTestCases(new()
        {
            ("test suite name 1", new[] { "test case name 1", "test case name 2" })
        });

        _tmsClientMock.Setup(m => m.GetTestCases(testSuitesIdNames[0]))
            .ReturnsAsync(existTestCases.Select(CreateIdName).ToList);

        _tmsClientMock.Setup(
                m => m.UpdateTestCases(
                    It.Is<IdName>(idName => idName.Equals(testSuitesIdNames[0])),
                    It.Is<IReadOnlyCollection<TestCase>>(tcs => Enumerable.SequenceEqual(tcs, existTestCases, new TestCaseNameComparer()))))
            .Returns(Task.CompletedTask);

        await CreateSynchronizer().Sync();

        _tmsClientMock.Verify(m => m.AddTestCases(testSuitesIdNames[0], sourceTestCases), Times.Never);
        _tmsClientMock.Verify(m => m.UpdateTestCases(testSuitesIdNames[0], It.IsAny<IReadOnlyCollection<TestCase>>()), Times.Once);
        _tmsClientMock.Verify(m => m.DeleteTestCases(It.IsAny<IReadOnlyCollection<IdName>>()), Times.Never);
    }

    [Fact]
    public async Task Delete_one_test_case_in_test_suite()
    {
        IReadOnlyList<TestCase> sourceTestCases =
            SetupTestCasesSourceMock(
                new()
                {
                    ("test suite name 1", new[] { "test case name 1" })
                });

        IReadOnlyList<IdName> testSuitesIdNames =
            SetupGetOrCreateTestSuiteMock(sourceTestCases);

        IReadOnlyList<TestCase> existTestCases = CreateTestCases(new()
        {
            ("test suite name 1", new[] { "test case name 1", "test case name 2" })
        });

        _tmsClientMock.Setup(m => m.GetTestCases(testSuitesIdNames[0]))
            .ReturnsAsync(existTestCases);

        IReadOnlyList<TestCase> updateTestCases = CreateTestCases(new()
        {
            ("test suite name 1", new[] { "test case name 1" })
        });

        _tmsClientMock.Setup(
                m => m.UpdateTestCases(
                    It.Is<IdName>(idName => idName.Equals(testSuitesIdNames[0])),
                    It.Is<IReadOnlyCollection<TestCase>>(tcs => Enumerable.SequenceEqual(tcs, updateTestCases, new TestCaseNameComparer()))))
            .Returns(Task.CompletedTask);

        IReadOnlyCollection<IdName> deleteTestCases = new[] { new IdName(1, "test case name 2") }.ToList();

        _tmsClientMock.Setup(
                m => m.DeleteTestCases(
                    It.Is<IReadOnlyCollection<IdName>>(tcs => Enumerable.SequenceEqual(tcs, deleteTestCases))))
            .Returns(Task.CompletedTask);

        await CreateSynchronizer().Sync();

        _tmsClientMock.Verify(m => m.AddTestCases(testSuitesIdNames[0], sourceTestCases), Times.Never);
        _tmsClientMock.Verify(m => m.UpdateTestCases(testSuitesIdNames[0], It.IsAny<IReadOnlyCollection<TestCase>>()), Times.Once);
        _tmsClientMock.Verify(m => m.DeleteTestCases(It.IsAny<IReadOnlyCollection<IdName>>()), Times.Once);
    }

    private static IdName CreateIdName(TestCase tc)
    {
        return new IdName(tc.Id, tc.Name);
    }

    private TestSuitesSynchronizer CreateSynchronizer()
    {
        return new TestSuitesSynchronizer(_testCasesSourceMock.Object, _tmsClientMock.Object, _appExecutionContextMock.Object);
    }

    private IReadOnlyList<IdName> SetupGetOrCreateTestSuiteMock(IReadOnlyList<TestCase> testCases)
    {
        var list = new List<IdName>();
        foreach (TestCase tc in testCases)
        {
            TestSuite ts = tc.GetTestSuite();
            var idName = new IdName(ts.Id, ts.Name);
            _tmsClientMock.Setup(m => m.GetOrCreateTestSuite(ts)).ReturnsAsync(idName);
            list.Add(idName);
        }

        return list;
    }

    private IReadOnlyList<TestCase> SetupTestCasesSourceMock(List<(string TestSuite, string[] TestCases)> testCasesSetups)
    {
        IReadOnlyList<TestCase> newTestCases = CreateTestCases(testCasesSetups);
        _testCasesSourceMock.Setup(m => m.Get()).Returns(newTestCases);
        return newTestCases;
    }

    private static IReadOnlyList<TestCase> CreateTestCases(List<(string TestSuite, string[] TestCases)> testCasesSetups)
    {
        var testCases = new List<TestCase>();
        List<(string TestSuite, string[] TestCases)> testCasesSetupsList = testCasesSetups.ToList();
        for (int tsIndex = 0; tsIndex < testCasesSetupsList.Count; tsIndex++)
        {
            (string TestSuite, string[] TestCases) testCasesSetup = testCasesSetupsList[tsIndex];
            var ts = new TestSuite(testCasesSetup.TestSuite, id: tsIndex);

            for (int tcIndex = 0; tcIndex < testCasesSetup.TestCases.Length; tcIndex++)
            {
                string tcName = testCasesSetup.TestCases[tcIndex];
                testCases.Add(new TestCase(tcName, ts, id: tcIndex));
            }
        }

        return testCases;
    }
}