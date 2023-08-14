using LightBDD.ScenarioSync.Cli;
using LightBDD.ScenarioSync.Core.App;
using LightBDD.ScenarioSync.Core.App.Config;
using LightBDD.ScenarioSync.IntegrationTest.Helpers.Configurations;

namespace LightBDD.ScenarioSync.IntegrationTest.Cli;

public class App_command_line_tests
{
    private static char PathSep = Path.DirectorySeparatorChar;

    [Fact]
    public void Run_init_command()
    {
        var args = new[]
        {
            "init",
            "--config", Path.Combine($"..{PathSep}..{PathSep}..{PathSep}..{PathSep}", AppConfig.FileName)
        };

        Program.Main(args);
    }

    [Fact]
    public void Run_push_no_arguments_command()
    {
        var args = new[]
        {
            "push"
        };

        Program.Main(args);
    }
    
    [Fact]
    public void Run_push_command()
    {
        var args = new[]
        {
            "push",
            "--config", Path.Combine($"..{PathSep}..{PathSep}..{PathSep}..{PathSep}", AppConfig.FileName)
        };

        Program.Main(args);
    }

    [Fact]
    public void Run_clean_command()
    {
        var args = new[]
        {
            "clean",
            "--config", Path.Combine($"..{PathSep}..{PathSep}..{PathSep}..{PathSep}", AppConfig.FileName)
        };
    
        Program.Main(args);
    }
}