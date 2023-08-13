using FluentAssertions;
using LightBDD.ScenarioSync.Core.App;
using Xunit;

namespace LightBDD.ScenarioSync.UnitTest;

public class AppArgumentsTest
{
    [Fact]
    public void CreateNewInstanceSuccessfully()
    {
        var arguments = new AppArguments(
            "https://dev.azure.com/organization/project-name",
            333,
            "344urpefnuf4skfobpu3fejhlumm7mvo373pxqmwhbbdxabjq",
            "FeaturesReport.xml"
        );

        arguments.OrganizationUrl.Should().Be("https://dev.azure.com/organization");
        arguments.ProjectName.Should().Be("project-name");
        arguments.TestPlanId.Should().Be(333);
        arguments.PatToken.Should().Be("344urpefnuf4skfobpu3fejhlumm7mvo373pxqmwhbbdxabjq");
        arguments.ReportPath.Should().Be("FeaturesReport.xml");
    }
}