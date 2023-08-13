namespace LightBDD.ScenarioSync.Core.App;

public class AppArguments : IAppArguments
{
    public const string RootTestSuiteDefault = "LightBddSync";

    public AppArguments(string projectUrl, int testPlanId, string patToken, string reportPath, string rootTestSuite = "")
    {
        projectUrl = projectUrl ?? throw new ArgumentNullException(nameof(projectUrl));
        ProjectUrl = projectUrl;
        var projectUrlUri = new Uri(projectUrl);
        if (projectUrlUri.Segments.Length < 3)
        {
            throw new ArgumentOutOfRangeException(nameof(projectUrl), "Url should have organization name and project name");
        }

        OrganizationUrl = new Uri(new Uri($"{projectUrlUri.Scheme}://{projectUrlUri.Host}"), projectUrlUri.Segments[1].TrimEnd('/'));
        ProjectName = projectUrlUri.Segments[2];
        patToken = patToken ?? throw new ArgumentNullException(nameof(patToken));
        PatToken = patToken;

        TestPlanId = testPlanId;
        ReportPath = reportPath;
        RootTestSuite = string.IsNullOrEmpty(rootTestSuite) ? RootTestSuiteDefault : rootTestSuite;
    }

    public string ReportPath { get; }
    public string RootTestSuite { get; }
    public string ProjectUrl { get; }
    public string ProjectName { get; }
    public Uri OrganizationUrl { get; }
    public string PatToken { get; }
    public int TestPlanId { get; }
}