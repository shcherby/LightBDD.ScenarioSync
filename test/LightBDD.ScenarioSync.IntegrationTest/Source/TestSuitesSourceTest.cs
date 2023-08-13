using FluentAssertions;
using LightBDD.ScenarioSync.Core.App;
using LightBDD.ScenarioSync.Core.Entities;
using LightBDD.ScenarioSync.IntegrationTest.Helpers.Configurations;
using LightBDD.ScenarioSync.Source;
using LightBDD.ScenarioSync.Source.XmlParser;
using Moq;

namespace LightBDD.ScenarioSync.IntegrationTest.Source;

public class TestSuitesSourceTest
{
    [Fact]
    public void Get_test_suites()
    {
        var testSuiteRepository = new TestCasesSource(
            SetupAppExecutionContextMock().Object,
            new FeaturesReportXmlParser());

        IReadOnlyCollection<TestCase> testCases = testSuiteRepository.Get();
        IReadOnlyCollection<TestSuite> testSuites = testCases.Select(tc => tc.GetTestSuite()).ToList();

        var testSuite = testSuites.FirstOrDefault(t => t.Name == "Metadata feature generate test report for parser");
        testSuite.Should().NotBeNull();
        testSuite.Path.Should().NotBeNull();
        testSuite.TestCases.Should().HaveCountGreaterThan(1);
        var testCase = testSuite.TestCases.FirstOrDefault(t => t.Name == "Scenario automated test metadata");
        testCase.AutomatedMetadata.Should().NotBeNull();
        testCase.AutomatedMetadata.MethodName.Should().NotBeNullOrEmpty();
        testCase.AutomatedMetadata.StorageName.Should().NotBeNullOrEmpty();
    }

    private static Mock<IAppExecutionContext> SetupAppExecutionContextMock()
    {
        var appContextProvider = new Mock<IAppExecutionContext>();
        appContextProvider
            .Setup(m => m.Arguments)
            .Returns(GetTestArguments());
        return appContextProvider;
    }

    private static AppArguments GetTestArguments()
    {
        return new TestEnvConfigurations().GetAppArguments();
    }
}