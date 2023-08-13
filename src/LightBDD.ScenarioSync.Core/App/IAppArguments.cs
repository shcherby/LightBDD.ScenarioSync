namespace LightBDD.ScenarioSync.Core.App;

public interface IAppArguments
{
    string ReportPath { get; }
    string RootTestSuite { get; }
    string ProjectUrl { get; }
    string ProjectName { get; }
    Uri OrganizationUrl { get; }
    string PatToken { get; }
    int TestPlanId { get; }
}