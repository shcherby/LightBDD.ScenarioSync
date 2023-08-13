using LightBDD.ScenarioSync.Core.Entities;
using LightBDD.ScenarioSync.Target.Ado.Models;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;

namespace LightBDD.ScenarioSync.Target.Ado.Helpers;

internal class TestStepsAttachmentsHelper
{
    private readonly WorkItemTrackingHttpClient _workItemClient;
    private readonly string _projectName;
    private readonly string _reportFolderPath;

    public TestStepsAttachmentsHelper(WorkItemTrackingHttpClient workItemClient, string projectName, string reportFolderPath)
    {
        _workItemClient = workItemClient;
        _projectName = projectName;
        _reportFolderPath = reportFolderPath;
    }

    public async Task<IReadOnlyCollection<StepCreateAttachmentOperation>> Prepare(IReadOnlyCollection<TestStep> testCaseSteps)
    {
        var stepAttachmentsOperations = new List<StepCreateAttachmentOperation>();
        IReadOnlyList<TestStep> newTestSteps = FlatSubSteps(testCaseSteps).ToList();
        int stepNumber = 1;
        for (int index = 0; stepNumber < newTestSteps.Count; index++)
        {
            TestStep step = newTestSteps[index];
            foreach (FileAttachment testStepFileAttachment in step.FileAttachments)
            {
                await using FileStream newFileStream = ReadFileStream(Path.Combine(_reportFolderPath, testStepFileAttachment.RelativePath));
                StepCreateAttachmentOperation at = await CreateAttachment(stepNumber, testStepFileAttachment, newFileStream);
                stepAttachmentsOperations.Add(at);
            }

            stepNumber++;
        }

        return stepAttachmentsOperations;
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

    private async Task<StepCreateAttachmentOperation> CreateAttachment(int stepNumber, FileAttachment testStepFileAttachment, FileStream fileStream)
    {
        long fileSize = fileStream.Length;
        AttachmentReference? result = await _workItemClient.CreateAttachmentAsync(fileStream, _projectName, fileName: testStepFileAttachment.Name);
        return new StepCreateAttachmentOperation(stepNumber, testStepFileAttachment.Name, result.Url, fileSize);
    }

    private static FileStream ReadFileStream(string relativePath)
    {
        FileStream uploadFileStream = File.Open(relativePath, FileMode.Open);
        return uploadFileStream;
    }
}