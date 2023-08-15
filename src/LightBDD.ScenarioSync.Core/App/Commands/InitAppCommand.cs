using LightBDD.ScenarioSync.Core.App.Config;

namespace LightBDD.ScenarioSync.Core.App.Commands;

public class InitAppCommand : IAppCommand
{
    private readonly AppConfig _appConfig;

    public InitAppCommand(AppConfig appConfig)
    {
        _appConfig = appConfig;
    }

    public Task RunAsync(AppArguments arguments)
    {
        _appConfig.CreateConfig(arguments);

        return Task.CompletedTask;
    }
}