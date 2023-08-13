using LightBDD.ScenarioSync.Core.App;
using Microsoft.Extensions.Configuration;

namespace LightBDD.ScenarioSync.IntegrationTest.Helpers.Configurations;

public class TestEnvConfigurations
{
    private IConfigurationRoot _configuration;

    public TestEnvConfigurations()
    {
        _configuration =
            new ConfigurationBuilder()
                .AddJsonFile("test-settings.json")
                .AddJsonFile("test-settings.ignore.json", optional: true).Build();
    }

    public AppArguments GetAppArguments()
    {
        var argumentsOptions = new ArgumentsOptions();
        _configuration.GetRequiredSection("Arguments").Bind(argumentsOptions);

        return new AppArguments(
            argumentsOptions.ProjectUrl,
            argumentsOptions.TestPlanId,
            argumentsOptions.PatToken,
            argumentsOptions.ReportPath,
            argumentsOptions.RootTestSuite
        );
    }
}