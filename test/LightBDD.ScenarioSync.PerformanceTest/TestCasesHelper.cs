namespace LightBDD.ScenarioSync.PerformanceTest;

public class TestCasesHelper
{
    private readonly string _rootTestSuite;

    public TestCasesHelper(string rootTestSuite)
    {
        _rootTestSuite = rootTestSuite;
    }

    public List<TestCase> CreateTestCases(int testSuitesCount = 100, int testCasesInSuiteCount = 10, int stepsInTestCaseCount = 5)
    {
        var allTestCases = new List<TestCase>();
        for (int testSuiteIndex = 1; testSuiteIndex <= testSuitesCount; testSuiteIndex++)
        {
            var testSuiteName = $"Test-suite-name-{testSuiteIndex}";

            IEnumerable<TestStep> steps = CreateSteps(stepsInTestCaseCount);

            IReadOnlyList<TestCase> testCases = Enumerable.Range(1, testCasesInSuiteCount)
                .Select(index => CreateAutomatedTestCase(_rootTestSuite, testSuiteName, $"Test-case-name-{index}", testSteps: steps))
                .ToList();

            allTestCases.AddRange(testCases);
        }

        return allTestCases;
    }

    public IEnumerable<TestStep> CreateSteps(int count = 5)
    {
        for (int stepNumber = 1; stepNumber <= count; stepNumber++)
        {
            yield return CreateTestStep(stepNumber);
        }
    }

    public TestSuite CreateTestSuite(string rootTestSuite, string name, IEnumerable<string>? labels = null)
    {
        labels ??= Enumerable.Empty<string>();
        List<string> labelsList = labels.ToList();
        var testSuitePath = new TestItemPath($"{rootTestSuite}\\{name}");
        return new TestSuite(name, "", new Tags(labelsList), RelationsFactory.Create(labelsList), path: testSuitePath);
    }

    public TestStep CreateTestStep(
        int stepNumber,
        string nameSuffix = "",
        IEnumerable<TestStepParameter>? parameters = null,
        IEnumerable<string>? comments = null,
        IEnumerable<FileAttachment>? fileAttachments = null)
    {
        nameSuffix = string.IsNullOrEmpty(nameSuffix) ? "" : $"{nameSuffix}-";
        return new TestStep(
            $"Step-{stepNumber}-name-{nameSuffix}{CreateUnique()}",
            stepNumber,
            "",
            parameters ?? new List<TestStepParameter>(),
            comments,
            fileAttachments
        );
    }

    public TestCase CreateAutomatedTestCase(
        string rootTestSuite,
        string newTestSuiteName,
        string testCaseName,
        IEnumerable<TestStep>? testSteps = null,
        IEnumerable<string>? testCaseLabels = null,
        string? automatedTestLabels = null)
    {
        TestSuite testCaseTestSuite = CreateTestSuite(rootTestSuite, newTestSuiteName);
        List<string> testCaseLabelsList = (testCaseLabels ?? Enumerable.Empty<string>()).ToList();
        testCaseLabelsList.Add(automatedTestLabels ?? MetadataLabelsCreator.CreateAutomatedTestLabel(testCaseName));
        return new TestCase(
            testCaseName,
            testCaseTestSuite,
            TagsFactory.Create(testCaseLabelsList),
            testSteps,
            RelationsFactory.Create(testCaseLabelsList),
            AutomatedTestMetadataFactory.Create(testCaseLabelsList)
        );
    }

    public string CreateUnique() => Guid.NewGuid().ToString();
}