using LightBDD.ScenarioSync.Core.Sync;

namespace LightBDD.ScenarioSync.Core.App.Commands;

public class CleanAppCommand : IAppCommand
{
    private readonly ITestSuitesSynchronizer _testSuitesSynchronizer;

    public CleanAppCommand(ITestSuitesSynchronizer testSuitesSynchronizer)
    {
        _testSuitesSynchronizer = testSuitesSynchronizer;
    }

    public async Task RunAsync(AppArguments arguments)
    {
        await _testSuitesSynchronizer.Clean();
    }
}