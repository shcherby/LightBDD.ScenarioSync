namespace LightBDD.ScenarioSync.Core.App.Config;

public record ArgumentsOptions(
    string ProjectUrl = "",
    string PatToken = "",
    int TestPlanId = 0,
    string ReportPath = "",
    string RootTestSuite = ""
);
