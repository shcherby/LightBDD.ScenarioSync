using LightBDD.ScenarioSync.Core.App;
using LightBDD.ScenarioSync.Core.Entities;
using LightBDD.ScenarioSync.IntegrationTest.Helpers.Configurations;
using LightBDD.ScenarioSync.IntegrationTest.Target.Tms.Clients;
using Moq;

namespace LightBDD.ScenarioSync.IntegrationTest.Target.Tms;

public class TmsAdoFixture : IAsyncLifetime
{
    private readonly ISet<IdName> _testSuitesForCleanUp = new HashSet<IdName>();
    public ITmsClientExtension TmsClientExtension;

    public async Task<IdName> GetTestSuiteForTest(string name)
    {
        IdName newTestSuite = await TmsClientExtension.GetOrCreateTestSuite(new TestSuite(name));
        _testSuitesForCleanUp.Add(newTestSuite);
        return newTestSuite;
    }

    public Task InitializeAsync()
    {
        TmsClientExtension = new TmsAdoClientExtension(SetupAppContextProviderMock().Object);
        return Task.CompletedTask;
    }

    public async Task DisposeAsync()
    {
        foreach (IdName testSuite in _testSuitesForCleanUp)
        {
            await TmsClientExtension.DeleteTestSuite(testSuite);
        }
    }

    private static Mock<IAppExecutionContext> SetupAppContextProviderMock()
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
