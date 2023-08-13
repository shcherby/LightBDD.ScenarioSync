namespace LightBDD.ScenarioSync.Core.Sync;

public interface ITestSuitesSynchronizer
{
    Task Sync();
    Task Clean();
}