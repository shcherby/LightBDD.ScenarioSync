using LightBDD.ScenarioSync.Core.App;
using LightBDD.ScenarioSync.Core.Entities;
using LightBDD.ScenarioSync.Core.Sync.Target;
using LightBDD.ScenarioSync.Target.Ado.Factories;
using LightBDD.ScenarioSync.Target.Ado.Helpers;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Microsoft.VisualStudio.Services.Common;
using Microsoft.VisualStudio.Services.TestManagement.TestPlanning.WebApi;
using Microsoft.VisualStudio.Services.WebApi;
using SuiteExpand = Microsoft.VisualStudio.Services.TestManagement.TestPlanning.WebApi.SuiteExpand;
using TestCase = LightBDD.ScenarioSync.Core.Entities.TestCase;
using TestSuiteResource = Microsoft.VisualStudio.Services.TestManagement.TestPlanning.WebApi.TestSuite;
using TestCaseResource = Microsoft.VisualStudio.Services.TestManagement.TestPlanning.WebApi.TestCase;
using WorkItem = Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models.WorkItem;
using AddWorkItem = Microsoft.VisualStudio.Services.TestManagement.TestPlanning.WebApi.WorkItem;
using LightBDD.ScenarioSync.Target.Ado.Renderers;
using LightBDD.ScenarioSync.Target.Ado.Models;
using Microsoft.VisualStudio.Services.WebApi.Patch.Json;

namespace LightBDD.ScenarioSync.Target.Ado;

internal class TmsAdoClient : ITmsClient
{
    protected WorkItemTrackingHttpClient WorkItemClient;
    protected TestPlanHttpClient TestPlanClient;
    protected string ProjectName;
    protected int TestPlanId;
    private readonly TestCaseStepsHelper _testCaseStepsHelper;
    private readonly TestStepsAttachmentsHelper _testStepsAttachmentsHelper;
    private readonly RelationsHelper _relationsHelper;

    public TmsAdoClient(IAppExecutionContext appExecutionContext)
    {
        Uri orgUrl = appExecutionContext.Arguments.OrganizationUrl;
        ProjectName = appExecutionContext.Arguments.ProjectName;
        TestPlanId = appExecutionContext.Arguments.TestPlanId;
        string reportDirectoryPath = Path.GetDirectoryName(appExecutionContext.Arguments.ReportPath);
        var cred = new VssBasicCredential(string.Empty, appExecutionContext.Arguments.PatToken);
        var connection = new VssConnection(orgUrl, cred);

        WorkItemClient = connection.GetClient<WorkItemTrackingHttpClient>();
        TestPlanClient = connection.GetClient<TestPlanHttpClient>();
        _testStepsAttachmentsHelper = new TestStepsAttachmentsHelper(WorkItemClient, ProjectName, reportDirectoryPath);
        _testCaseStepsHelper = new TestCaseStepsHelper();
        _relationsHelper = new RelationsHelper(WorkItemClient);
    }

    public async Task<IdName> GetOrCreateTestSuite(ITestSuite testSuite)
    {
        List<TestSuiteResource> testPlanSuites = await TestPlanClient.GetTestSuitesForPlanAsync(ProjectName, TestPlanId, SuiteExpand.Children, asTreeView: true);
        TestSuiteResource testSuiteMarker = await GetTestSuiteMarkerByPath(testSuite.Path, testPlanSuites);

        WorkItem? wi = await WorkItemClient.GetWorkItemAsync(testSuiteMarker.Id, expand: WorkItemExpand.Relations);
        if (wi is not null)
        {
            ITestSuite currentTestSuite = TestSuiteFactory.Map(wi);
            if (currentTestSuite.Name != testSuite.Name
                || currentTestSuite.Description != testSuite.Description
                || string.Join(",", currentTestSuite.Tags) != string.Join(",", testSuite.Tags)
                || string.Join(",", currentTestSuite.Relations) != string.Join(",", testSuite.Relations))
            {
                IReadOnlyCollection<RelationOperation> relatedWorItems = await _relationsHelper.Prepare(testSuite.Relations);

                JsonPatchDocument updatePatchDocument =
                    new TestItemJsonPatchDocumentBuilder()
                        .Load(wi)
                        .SetTitle(testSuite.Name)
                        .SetDescription(testSuite.Description)
                        .SetTags(testSuite.Tags)
                        .SetRelations(relatedWorItems)
                        .Build();

                await WorkItemClient.UpdateWorkItemAsync(updatePatchDocument, testSuiteMarker.Id);
            }
        }

        return new IdName(testSuiteMarker.Id, testSuiteMarker.Name);
    }

    public async Task<IReadOnlyList<IdName>> GetTestCases(IdName testSuiteId)
    {
        IReadOnlyCollection<TestCaseResource> adoTestCases = await GetAllTestCases(testSuiteId.Id);
        return adoTestCases.Select(tc => new IdName(tc.workItem.Id, tc.workItem.Name)).ToList();
    }

    public async Task<IReadOnlyList<IdName>> AddTestCases(IdName testSuiteId, IReadOnlyCollection<TestCase> testCases)
    {
        var workItems = new List<WorkItem>();
        foreach (TestCase testCase in testCases)
        {
            IReadOnlyCollection<RelationOperation> relatedWorkItems =
                await _relationsHelper.Prepare(testCase.Relations);

            IReadOnlyCollection<StepCreateAttachmentOperation> stepAttachments =
                await _testStepsAttachmentsHelper.Prepare(testCase.Steps);

            JsonPatchDocument testCasePatchDocument =
                new TestItemJsonPatchDocumentBuilder(_testCaseStepsHelper)
                    .SetTitle(testCase.Name)
                    .SetDescription(TestCaseDescriptionRenderer.Render(testCase), true)
                    .SetTags(testCase.Tags)
                    .SetAutomationMetadata(testCase.AutomatedMetadata)
                    .SetRelations(relatedWorkItems)
                    .SetSteps(testCase.Steps)
                    .SetStepAttachments(stepAttachments)
                    .Build();

            WorkItem? workItem = await WorkItemClient.CreateWorkItemAsync(testCasePatchDocument, ProjectName, "Test Case");
            workItems.Add(workItem);
        }

        List<IdName> addedIds = workItems.Select(wi => new IdName(wi.Id.Value, wi.GetTitle())).ToList();
        List<SuiteTestCaseCreateUpdateParameters> addList = addedIds.Select(wId => new SuiteTestCaseCreateUpdateParameters { workItem = new AddWorkItem { Id = wId.Id } }).ToList();

        await TestPlanClient.AddTestCasesToSuiteAsync(addList, ProjectName, TestPlanId, testSuiteId.Id);
        return addedIds;
    }

    public async Task UpdateTestCases(IdName testSuiteId, IReadOnlyCollection<TestCase> testCases)
    {
        IReadOnlyCollection<TestCaseResource> existTestCases = await GetAllTestCases(testSuiteId.Id);
        IReadOnlyDictionary<string, int> existingTestCasesIdNameMap = ToExistingTestCasesIdNameMap(existTestCases);
        List<WorkItem> workItems = new List<WorkItem>();
        foreach (TestCase testCase in testCases)
        {
            if (existingTestCasesIdNameMap.TryGetValue(testCase.Name, out int testCaseId))
            {
                WorkItem? workItem = await WorkItemClient.GetWorkItemAsync(testCaseId, expand: WorkItemExpand.All);

                IReadOnlyCollection<RelationOperation> relatedWorkItems =
                    await _relationsHelper.Prepare(testCase.Relations);

                IReadOnlyCollection<StepCreateAttachmentOperation> stepAttachments =
                    await _testStepsAttachmentsHelper.Prepare(testCase.Steps);

                JsonPatchDocument testCasePatchDocument =
                    new TestItemJsonPatchDocumentBuilder(_testCaseStepsHelper)
                        .Load(workItem)
                        .SetTitle(testCase.Name)
                        .SetDescription(TestCaseDescriptionRenderer.Render(testCase), true)
                        .SetTags(testCase.Tags)
                        .SetAutomationMetadata(testCase.AutomatedMetadata)
                        .SetRelations(relatedWorkItems)
                        .SetSteps(testCase.Steps)
                        .SetStepAttachments(stepAttachments)
                        .Build();

                WorkItem? updatedWorkItem = await WorkItemClient.UpdateWorkItemAsync(testCasePatchDocument, testCaseId);
                workItems.Add(updatedWorkItem);
            }
        }
    }

    public async Task DeleteTestCases(IReadOnlyCollection<IdName> testCasesId)
    {
        if (testCasesId.Any())
        {
            foreach (IdName caseId in testCasesId)
            {
                await TestPlanClient.DeleteTestCaseAsync(ProjectName, caseId.Id);
            }
        }
    }

    public async Task<IReadOnlyList<IdName>> GetTestSuites(TestItemPath rootTestSuitePath, bool excludeRoot = true)
    {
        List<TestSuiteResource> testPlanSuites = await TestPlanClient.GetTestSuitesForPlanAsync(ProjectName, TestPlanId, SuiteExpand.Children, asTreeView: true);
        TestSuiteResource testSuiteMarker = await GetTestSuiteMarkerByPath(rootTestSuitePath, testPlanSuites);
        IReadOnlyList<TestSuiteResource> allTestSuites = new List<TestSuiteResource>();
        if (excludeRoot)
        {
            testSuiteMarker.Children ??= new List<TestSuiteResource>();
            allTestSuites = testSuiteMarker.Children.SelectMany(tsm => GetFlatTestSuites(tsm)).ToList();
        }
        else
        {
            allTestSuites = GetFlatTestSuites(testSuiteMarker).ToList();
        }

        return allTestSuites.Select(ts => new IdName(ts.Id, ts.Name)).ToList();
    }

    public async Task DeleteTestSuite(IdName id)
    {
        IReadOnlyCollection<IdName> testCases = await GetTestCases(id);
        await DeleteTestCases(testCases);
        await TestPlanClient.DeleteTestSuiteAsync(ProjectName, TestPlanId, id.Id);
    }

    private IReadOnlyList<TestSuiteResource> GetFlatTestSuites(TestSuiteResource tsMarker, List<TestSuiteResource>? allTestSuitesAgg = null)
    {
        allTestSuitesAgg ??= new List<TestSuiteResource>();
        tsMarker.Children ??= new List<TestSuiteResource>();

        allTestSuitesAgg.Add(tsMarker);
        foreach (TestSuiteResource? tsChild in tsMarker.Children)
        {
            GetFlatTestSuites(tsChild, allTestSuitesAgg);
        }

        return allTestSuitesAgg.AsReadOnly();
    }

    private async Task<TestSuiteResource> GetTestSuiteMarkerByPath(TestItemPath rootTestSuitePath, List<TestSuiteResource> testPlanSuites)
    {
        TestSuiteResource testSuiteMarker = testPlanSuites[0]; //First level is the root suite
        TestSuiteReference parentTestSuiteMarker = MapToReference(testSuiteMarker);

        for (int levelIndex = 0; levelIndex < rootTestSuitePath.Levels.Count; levelIndex++)
        {
            string testSuiteName = rootTestSuitePath.GetNameOnTheLevel(levelIndex);
            testSuiteMarker = FindTestSuiteByName(testSuiteMarker.Children, testSuiteName);

            if (testSuiteMarker == null)
            {
                testSuiteMarker = await CreateTestSuite(testSuiteName, parentTestSuiteMarker);
                parentTestSuiteMarker = MapToReference(testSuiteMarker);
            }
            else
            {
                parentTestSuiteMarker = MapToReference(testSuiteMarker);
            }
        }

        return testSuiteMarker;
    }

    private static IReadOnlyDictionary<string, int> ToExistingTestCasesIdNameMap(IReadOnlyCollection<TestCaseResource> existingTestCases)
    {
        Dictionary<string, int> existingTestCasesIdNameMap = new();
        foreach (TestCaseResource tc in existingTestCases)
        {
            existingTestCasesIdNameMap.TryAdd(tc.workItem.Name, tc.workItem.Id);
        }

        return existingTestCasesIdNameMap;
    }

    private async Task<TestSuiteResource> CreateTestSuite(string name, TestSuiteReference parentTestSuiteReference)
    {
        var newTestSuiteParams = new TestSuiteCreateParams
        {
            Name = name,
            ParentSuite = parentTestSuiteReference,
            SuiteType = TestSuiteType.StaticTestSuite
        };

        return await TestPlanClient.CreateTestSuiteAsync(newTestSuiteParams, ProjectName, TestPlanId);
    }

    private async Task<IReadOnlyCollection<TestCaseResource>> GetAllTestCases(int testSuiteId)
    {
        var resources = new List<TestCaseResource>();
        PagedList<TestCaseResource>? testCasesPagedList = await TestPlanClient.GetTestCaseListAsync(ProjectName, TestPlanId, testSuiteId, expand: true);
        if (testCasesPagedList is null)
        {
            return resources;
        }

        resources.AddRange(testCasesPagedList);

        while (!string.IsNullOrEmpty(testCasesPagedList.ContinuationToken))
        {
            PagedList<TestCaseResource> nextPageResources = await TestPlanClient.GetTestCaseListAsync(ProjectName, TestPlanId, testSuiteId, expand: true, continuationToken: testCasesPagedList.ContinuationToken);
            if (nextPageResources is not null)
            {
                resources.AddRange(nextPageResources);
            }
        }

        return resources;
    }

    private static TestSuiteResource? FindTestSuiteByName(List<TestSuiteResource> testSuiteChildren, string testSuiteName)
    {
        return testSuiteChildren?.FirstOrDefault(ts => ts.Name == testSuiteName);
    }

    private static TestSuiteReference MapToReference(TestSuiteResource testSuiteMarker)
    {
        return new TestSuiteReference { Id = testSuiteMarker.Id, Name = testSuiteMarker.Name };
    }
}