using FluentAssertions;
using LightBDD.ScenarioSync.Core.Entities;
using Xunit;

namespace LightBDD.ScenarioSync.UnitTest;

public class TestSuitePathTest
{
    [Fact]
    public void Create_new_TestSuitePath_with_two_segments()
    {
        var testSuitePath = new TestItemPath("auto\api1");
        testSuitePath.Levels.Should().HaveCount(2);
    }

    [Fact]
    public void Create_new_TestSuitePath_with_one_segments()
    {
        var testSuitePath = new TestItemPath("auto");
        testSuitePath.Levels.Should().HaveCount(1);
    }

    [Fact]
    public void Create_new_TestSuitePath_with_root()
    {
        var testSuitePath = new TestItemPath("");
        testSuitePath.Levels.Should().HaveCount(0);
    }
}