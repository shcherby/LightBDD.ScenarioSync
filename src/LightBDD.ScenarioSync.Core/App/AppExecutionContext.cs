namespace LightBDD.ScenarioSync.Core.App;

public class AppExecutionContext : IAppExecutionContext
{
    public AppExecutionContext(IAppArguments arguments)
    {
        Arguments = arguments;
    }

    public IAppArguments Arguments { get; }
}