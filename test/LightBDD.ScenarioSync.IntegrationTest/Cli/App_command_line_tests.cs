using LightBDD.ScenarioSync.Cli;
using LightBDD.ScenarioSync.Core.App;
using LightBDD.ScenarioSync.IntegrationTest.Helpers.Configurations;

namespace LightBDD.ScenarioSync.IntegrationTest.Cli;

public class App_command_line_tests
{
    private static readonly AppArguments Arguments = new TestEnvConfigurations().GetAppArguments();

    [Fact]
    public void Run_sync_command()
    {
        var args = new[]
        {
            "sync",
            "--projectUrl", Arguments.ProjectUrl,
            "--patToken", Arguments.PatToken,
            "--testPlanId", Arguments.TestPlanId.ToString(),
            "--reportFilePath", Arguments.ReportPath,
        };

        Program.Main(args);
    }

    [Fact]
    public void Run_clean_command()
    {
        var args = new[]
        {
            "clean",
            "--projectUrl", Arguments.ProjectUrl,
            "--patToken", Arguments.PatToken,
            "--testPlanId", Arguments.TestPlanId.ToString(),
            "--reportFilePath", Arguments.ReportPath,
        };

        Program.Main(args);
    }
}