namespace LightBDD.ScenarioSync.IntegrationTest.Helpers.Configurations;

public record ArgumentsOptions(
    string ProjectUrl = "",
    string PatToken = "",
    int TestPlanId = 0,
    string ReportPath = "",
    string RootTestSuite = "",
    int MaxParallelism = 5
);
