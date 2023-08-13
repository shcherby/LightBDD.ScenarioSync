namespace LightBDD.ScenarioSync.Core.App;

public interface IAppCommand
{
    Task RunAsync(AppArguments arguments);
}