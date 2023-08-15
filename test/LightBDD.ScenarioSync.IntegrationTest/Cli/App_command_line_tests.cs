using LightBDD.ScenarioSync.Cli;
using LightBDD.ScenarioSync.Core.App;
using LightBDD.ScenarioSync.Core.App.Config;
using LightBDD.ScenarioSync.IntegrationTest.Helpers.Configurations;

namespace LightBDD.ScenarioSync.IntegrationTest.Cli;

public class App_command_line_tests
{
    [Fact]
    public void Run_init_command()
    {
        var args = new[]
        {
            "init"
        };

        Program.Main(args);
    }

    [Fact]
    public void Run_push_command()
    {
        var args = new[]
        {
            "push"
        };

        Program.Main(args);
    }

    [Fact]
    public void Run_clean_command()
    {
        var args = new[]
        {
            "clean"
        };

        Program.Main(args);
    }
}