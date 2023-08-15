using System.Text.Json;

namespace LightBDD.ScenarioSync.Core.App.Config;

public class AppConfig
{
    private readonly string _configPath;
    public static string FilePath = $".{Path.DirectorySeparatorChar}{FileName}";
    public const string FileName = "scenariosync.json.user";

    public AppConfig(string configPath = null)
    {
        _configPath = string.IsNullOrEmpty(configPath) ? FilePath : configPath;
    }

    public void CreateConfig(AppArguments arguments)
    {
        string configJson = JsonSerializer.Serialize(
            new ArgumentsOptions(
                arguments.ProjectUrl,
                arguments.PatToken,
                arguments.TestPlanId,
                arguments.ReportPath,
                arguments.RootTestSuite)
            ,
            new JsonSerializerOptions()
            {
                WriteIndented = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

        File.WriteAllText(_configPath, configJson);
    }

    public AppArguments ReadConfig()
    {
        string configContent = File.ReadAllText(_configPath);
        var arguments = JsonSerializer.Deserialize<ArgumentsOptions>(configContent, new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true
        });

        return new AppArguments(arguments.ProjectUrl, arguments.PatToken, arguments.TestPlanId, arguments.ReportPath, arguments.RootTestSuite);
    }
}