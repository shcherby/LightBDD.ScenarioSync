using System.CommandLine;
using System.CommandLine.Invocation;
using System.Text.Json.Serialization;
using LightBDD.ScenarioSync.Core.App;
using LightBDD.ScenarioSync.Core.App.Commands;
using LightBDD.ScenarioSync.Core.App.Config;
using Newtonsoft.Json;

namespace LightBDD.ScenarioSync.Cli;

public class Program
{
    public static void Main(string[] args)
    {
        var rootCommand = new RootCommand("A cli tool for automatically associate automated tests with test cases.");

        Command initCommand = CreateSubCommand<InitAppCommand>("init", $"This command will create a ScenarioSync configuration file {AppConfig.FilePath}");
        Command pushCommand = CreateSubCommand<PushAppCommand>("push", "Import scenarios to Azure Devops Test Suite.");
        Command cleanCommand = CreateSubCommand<CleanAppCommand>("clean", "Clean imported scenarios in Azure Devops.");

        rootCommand.Add(initCommand);
        rootCommand.Add(pushCommand);
        rootCommand.Add(cleanCommand);
        rootCommand.Invoke(args);
    }

    private static Command CreateSubCommand<TCommand>(string subCommand, string description) where TCommand : IAppCommand
    {
        var syncCommand = new Command(subCommand, description);

        syncCommand.Handler = CommandHandler.Create(() =>
        {
            AppArguments arguments = new AppConfig().ReadConfig();
            Task syncTask = RunSyncCommand<TCommand>(arguments);

            syncTask.GetAwaiter().GetResult();
        });
        return syncCommand;
    }

    private static async Task RunSyncCommand<TCommand>(AppArguments arguments) where TCommand : IAppCommand
    {
        var app = new AppBootstrap(arguments)
            .WithStartup(new Startup());

        await app.RunAsync<TCommand>();
    }
}