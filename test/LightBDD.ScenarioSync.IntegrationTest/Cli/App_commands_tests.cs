using LightBDD.ScenarioSync.Cli;
using LightBDD.ScenarioSync.Core.App;
using LightBDD.ScenarioSync.Core.App.Commands;
using LightBDD.ScenarioSync.IntegrationTest.Helpers.Configurations;

namespace LightBDD.ScenarioSync.IntegrationTest.Cli;

public class App_commands_tests
{
    [Fact]
    public async Task Sync_command_test()
    {
        var configOverride = new Dictionary<string, string>() { { "arguments:reportPath", "../../../../Reports/FeaturesReport.xml" } };
        AppArguments arguments = new TestEnvConfigurations(configOverride).GetAppArguments();

        var app = new AppBootstrap(arguments)
            .WithStartup(new Startup())
            .WithOverrides(collection => { });

        await app.RunAsync<PushAppCommand>();
    }

    [Fact]
    public async Task Clean_command_test()
    {
        AppArguments arguments = new TestEnvConfigurations().GetAppArguments();
        var app = new AppBootstrap(arguments)
            .WithStartup(new Startup())
            .WithOverrides(collection => { });

        await app.RunAsync<CleanAppCommand>();
    }
}