using LightBDD.ScenarioSync.Core.Entities;
using LightBDD.ScenarioSync.Core.Entities.Parameters;
using LightBDD.ScenarioSync.IntegrationTest.Helpers;
using LightBDD.ScenarioSync.IntegrationTest.Helpers.Extensions;
using LightBDD.ScenarioSync.Source.Factories;

namespace LightBDD.ScenarioSync.IntegrationTest.Target.Tms
{
    public abstract class Test_suite_test_cases_tests_base : Tms_tests_base
    {
        public Test_suite_test_cases_tests_base(TmsAdoFixture adoTargetFixture)
            : base(adoTargetFixture)
        {
        }

        protected static readonly FileAttachment Text1FileAttachment = new("test-step-text-file-1-attachment.txt", "TestData\\test-step-text-file-1-attachment.txt");
        protected static readonly FileAttachment Text2FileAttachment = new("test-step-text-file-2-attachment.txt", "TestData\\test-step-text-file-2-attachment.txt");
        protected static readonly FileAttachment Image1FileAttachment = new("test-step-image-file-1-attachment.png", "TestData\\test-step-image-file-1-attachment.png");
        protected static readonly FileAttachment Image2FileAttachment = new("test-step-image-file-2-attachment.jpg", "TestData\\test-step-image-file-2-attachment.jpg");

        protected IReadOnlyCollection<TestCase> CreateAutomatedTestCaseAsList(
            IdName newTestSuite,
            string testCaseName = "",
            IEnumerable<TestStep>? testSteps = null,
            IEnumerable<string>? testCaseLabels = null,
            string? automatedTestLabels = null)
        {
            return CreateAutomatedTestCase(newTestSuite, testCaseName, testSteps, testCaseLabels, automatedTestLabels).ToList();
        }

        protected TestStep CreateTestStep(
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

        protected TestCase CreateAutomatedTestCase(
            IdName newTestSuite,
            string testCaseName = "",
            IEnumerable<TestStep>? testSteps = null,
            IEnumerable<string>? testCaseLabels = null,
            string? automatedTestLabels = null)
        {
            TestSuite testCaseTestSuite = CreateTestSuite(newTestSuite);
            List<string> testCaseLabelsList = (testCaseLabels ?? Enumerable.Empty<string>()).ToList();
            testCaseLabelsList.Add(automatedTestLabels ?? MetadataLabelsCreator.CreateAutomatedTestLabel(testCaseName));
            return new TestCase(
                string.IsNullOrEmpty(testCaseName) ? $"Test-case-name-{CreateUnique()}" : testCaseName,
                testCaseTestSuite,
                TagsFactory.Create(testCaseLabelsList),
                testSteps,
                RelationsFactory.Create(testCaseLabelsList),
                AutomatedTestMetadataFactory.Create(testCaseLabelsList)
            );
        }

        protected IReadOnlyCollection<TestStep> CreateTestStepAsList(
            int stepNumber,
            string nameSuffix = "",
            IEnumerable<TestStepParameter>? parameters = null,
            IEnumerable<string>? comments = null,
            IEnumerable<FileAttachment>? fileAttachments = null)
        {
            return CreateTestStep(stepNumber, nameSuffix, parameters, comments, fileAttachments).ToList();
        }
    }
}