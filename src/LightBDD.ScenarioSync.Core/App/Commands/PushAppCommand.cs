using LightBDD.ScenarioSync.Core.Sync;

namespace LightBDD.ScenarioSync.Core.App.Commands;

public class PushAppCommand : IAppCommand
{
    private readonly ITestSuitesSynchronizer _testSuitesSynchronizer;

    public PushAppCommand(ITestSuitesSynchronizer testSuitesSynchronizer)
    {
        _testSuitesSynchronizer = testSuitesSynchronizer;
    }

    public async Task RunAsync(AppArguments arguments)
    {
        await _testSuitesSynchronizer.Sync();
    }
}