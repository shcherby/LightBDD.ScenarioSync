using System.Text.Encodings.Web;
using LightBDD.ScenarioSync.Core.Entities;
using LightBDD.ScenarioSync.Target.Ado.Renderers;
using Microsoft.TeamFoundation.TestManagement.WebApi;

namespace LightBDD.ScenarioSync.Target.Ado.Helpers;

internal class TestCaseStepsHelper
{
    public void SetTestSteps(ITestBase testBase, IReadOnlyCollection<TestStep> testCaseSteps)
    {
        IReadOnlyList<TestStep> newTestSteps = FlatSubSteps(testCaseSteps).ToList();

        int stepIndex = 0;
        int existingTestStepsCount = testBase.Actions.Count;
        while (stepIndex < newTestSteps.Count)
        {
            TestStep testStep = newTestSteps[stepIndex];
            ITestStep adoTestStep = testBase.CreateTestStep();
            if (stepIndex < existingTestStepsCount)
            {
                adoTestStep = (ITestStep)testBase.Actions[stepIndex];
            }
            else
            {
                testBase.Actions.Add(adoTestStep);
            }
            adoTestStep.Title = new StepNameWithParametersRenderer(testStep).Render();
            adoTestStep.ExpectedResult = new StepNameParameterExpectationRenderer(testStep.Parameters).Render();

            stepIndex++;
        }

        while (stepIndex < existingTestStepsCount)
        {
            testBase.Actions.RemoveAt(stepIndex);
            existingTestStepsCount--;
        }
    }

    private static IReadOnlyList<TestStep> FlatSubSteps(IReadOnlyCollection<TestStep> testSteps, List<TestStep>? aggregator = null)
    {
        aggregator ??= new List<TestStep>();
        foreach (TestStep testStep in testSteps)
        {
            aggregator.Add(testStep);
            FlatSubSteps(testStep.GetSubSteps(), aggregator);
        }

        return aggregator;
    }
}