using LightBDD.ScenarioSync.Core.Entities;
using LightBDD.ScenarioSync.Target.Ado.Helpers;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;

namespace LightBDD.ScenarioSync.Target.Ado.Factories;

public static class TestSuiteFactory
{
    public static ITestSuite Map(WorkItem? workItem)
    {
        if (workItem is null)
        {
            return new TestSuite("");
        }

        var ts = new TestSuite(
            workItem.GetTitle(),
            workItem.GetDescription(),
            new Tags(workItem.GetTags()),
            relations: new Relations(workItem.GetRelationsId()),
            testCases: Enumerable.Empty<TestCase>()
        );
        return ts;
    }
}