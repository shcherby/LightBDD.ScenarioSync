using System.CommandLine;
using System.CommandLine.Invocation;
using LightBDD.ScenarioSync.Core.App;
using LightBDD.ScenarioSync.Core.App.Commands;

namespace LightBDD.ScenarioSync.Cli;

public class Program
{
    public static void Main(string[] args)
    {
        var rootCommand = new RootCommand("A cli tool for automatically associate automated tests with test cases.");

        Command pushCommand = CreateSubCommand<PushAppCommand>("push", $"Import scenarios to Azure Devops Test Suite {AppArguments.RootTestSuiteDefault}.");
        Command cleanCommand = CreateSubCommand<CleanAppCommand>("clean", "Clean imported scenarios in Azure Devops.");

        rootCommand.Add(pushCommand);
        rootCommand.Add(cleanCommand);
        rootCommand.Invoke(args);
    }

    private static Command CreateSubCommand<TCommand>(string subCommand, string description) where TCommand: IAppCommand
    {
        var projectUrl = new Option<string>("--projectUrl", description: "Azure Devops project url. Example: https://dev.azure.com/organization/project-name") { IsRequired = true };
        var testPlanId = new Option<int>("--testPlanId", description: "Azure Devops test planId") { IsRequired = true };
        var patToken = new Option<string>("--patToken", description: "Azure Devops pat token") { IsRequired = true };
        var reportFilePath = new Option<string>("--reportFilePath", description: "Path to LightBDD FeaturesReport.xml") { IsRequired = true };
        var rootTestSuite = new Option<string>("--rootTestSuite", () => AppArguments.RootTestSuiteDefault, description: "Root Test suite name") { IsRequired = true };
        
        var syncCommand = new Command(subCommand, description)
        {
            projectUrl,
            testPlanId,
            patToken,
            reportFilePath,
            rootTestSuite
        };

        syncCommand.Handler = CommandHandler.Create((
            string projectUrl,
            int testPlanId,
            string patToken,
            string reportFilePath,
            string rootTestSuite) =>
        {
            Task syncTask =
                RunSyncCommand<TCommand>(
                    new AppArguments(
                        projectUrl,
                        testPlanId,
                        patToken,
                        reportFilePath,
                        rootTestSuite
                    ));

            syncTask.GetAwaiter().GetResult();
        });
        return syncCommand;
    }

    private static async Task RunSyncCommand<TCommand>(AppArguments arguments) where TCommand: IAppCommand
    {
        var app = new AppBootstrap(arguments)
            .WithStartup(new Startup());

        await app.RunAsync<TCommand>();
    }
}