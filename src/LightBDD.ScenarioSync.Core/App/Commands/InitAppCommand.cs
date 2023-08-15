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
        _appConfig.CreateConfig(
            new AppArguments(
                "https://dev.azure.com/organization-name/project-name",
                "personal token with permissions TestManagement write&read, WorkItems write&read",
                1,
                "./Reports/FeaturesReport.xml")
        );

        return Task.CompletedTask;
    }
}