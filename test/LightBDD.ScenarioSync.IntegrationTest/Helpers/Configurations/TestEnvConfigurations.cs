using LightBDD.ScenarioSync.Core.App;
using Microsoft.Extensions.Configuration;

namespace LightBDD.ScenarioSync.IntegrationTest.Helpers.Configurations;

public class TestEnvConfigurations
{
    private IConfigurationRoot _configuration;

    public TestEnvConfigurations(IDictionary<string, string>? configsOverride = null)
    {
        using FileStream testSettingsStream = File.OpenRead("../../../test-settings.ignore.json");
        var builder = new ConfigurationBuilder()
            .AddJsonFile("test-settings.json")
            .AddJsonStream(testSettingsStream);

        if (configsOverride is not null)
        {
            builder.AddInMemoryCollection(configsOverride);
        }

        _configuration = builder.Build();
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