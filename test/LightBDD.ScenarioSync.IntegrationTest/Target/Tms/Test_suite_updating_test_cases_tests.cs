using LightBDD.ScenarioSync.Core.Entities;
using LightBDD.ScenarioSync.Core.Sync.Target;
using LightBDD.ScenarioSync.IntegrationTest.Helpers;

namespace LightBDD.ScenarioSync.IntegrationTest.Target.Tms
{
    [Collection(nameof(Test_suite_updating_test_cases_tests))]
    public class Test_suite_updating_test_cases_tests : Test_suite_test_cases_tests_base
    {
        [CollectionDefinition(nameof(Test_suite_updating_test_cases_tests))]
        public class TmsAdoTargetCollection : ICollectionFixture<TmsAdoFixture>
        {
        }

        private const string TestCaseContainerName = nameof(Test_suite_updating_test_cases_tests);

        public Test_suite_updating_test_cases_tests(TmsAdoFixture adoTargetFixture)
            : base(adoTargetFixture)
        {
        }

        [Fact]
        public async Task Update_test_case_tags_associatedAutomation_summary_fields()
        {
            ITmsClient client = CreateTmsAdoClient();
            IdName testSuite = await TmsAdoFixture.GetTestSuiteForTest(TestCaseContainerName);
            IReadOnlyCollection<string> tags = new List<string>() { "tag1", "tag2" };
            IReadOnlyCollection<IdName> addedTestCases =
                await client.AddTestCases(
                    testSuite,
                    new List<TestCase>
                    {
                        CreateAutomatedTestCase(
                            testSuite,
                            testCaseName: nameof(Update_test_case_tags_associatedAutomation_summary_fields),
                            testSteps: CreateTestStepAsList(1),
                            tags,
                            automatedTestLabels: MetadataLabelsCreator.CreateAutomatedTestLabel("New_automation_test_name", "New_automation_storage")
                        )
                    }
                );

            IdName? newTestCase = addedTestCases.FirstOrDefault();

            IReadOnlyCollection<string> updateTags = new List<string>() { "tag1", "tag2Updated", "tag3New" };
            IReadOnlyCollection<TestCase> updateTestCases = CreateAutomatedTestCaseAsList(
                testSuite,
                testCaseName: newTestCase.Name,
                testCaseLabels: updateTags,
                automatedTestLabels: MetadataLabelsCreator.CreateAutomatedTestLabel("Updated_automation_test_name", "Updated_automation_storage")
            );

            await client.UpdateTestCases(
                testSuite,
                updateTestCases
            );
        }

        [Fact]
        public async Task Update_second_step_title_in_the_test_case()
        {
            ITmsClient client = CreateTmsAdoClient();
            IdName testSuite = await TmsAdoFixture.GetTestSuiteForTest(TestCaseContainerName);

            IEnumerable<TestStep> newTestSteps = new List<TestStep>()
            {
                CreateTestStep(1),
                CreateTestStep(2)
            };

            IReadOnlyCollection<IdName> addedTestCases =
                await client.AddTestCases(
                    testSuite,
                    new List<TestCase>
                    {
                        CreateAutomatedTestCase(
                            testSuite,
                            testCaseName: nameof(Update_second_step_title_in_the_test_case),
                            testSteps: newTestSteps)
                    }
                );

            IdName? newTestCase = addedTestCases.FirstOrDefault();
            IEnumerable<TestStep> updateTestSteps = new List<TestStep>()
            {
                CreateTestStep(1),
                CreateTestStep(2, nameSuffix: "updated")
            };
            IReadOnlyCollection<TestCase> updateTestCases = CreateAutomatedTestCaseAsList(
                testSuite,
                newTestCase.Name,
                testSteps: updateTestSteps
            );

            await client.UpdateTestCases(
                testSuite,
                updateTestCases
            );
        }

        [Fact]
        public async Task Add_third_step_in_the_test_case()
        {
            ITmsClient client = CreateTmsAdoClient();
            IdName testSuite = await TmsAdoFixture.GetTestSuiteForTest(TestCaseContainerName);

            IEnumerable<TestStep> newTestSteps = new List<TestStep>()
            {
                CreateTestStep(1),
                CreateTestStep(2)
            };

            IReadOnlyCollection<IdName> addedTestCases =
                await client.AddTestCases(
                    testSuite,
                    new List<TestCase>
                    {
                        CreateAutomatedTestCase(
                            testSuite,
                            testCaseName: nameof(Add_third_step_in_the_test_case),
                            testSteps: newTestSteps)
                    }
                );

            IdName? newTestCase = addedTestCases.FirstOrDefault();
            IEnumerable<TestStep> updateTestSteps = new List<TestStep>()
            {
                CreateTestStep(1),
                CreateTestStep(2),
                CreateTestStep(3)
            };

            IReadOnlyCollection<TestCase> updateTestCases = CreateAutomatedTestCaseAsList(
                testSuite,
                newTestCase.Name,
                testSteps: updateTestSteps
            );

            await client.UpdateTestCases(
                testSuite,
                updateTestCases
            );
        }

        [Fact]
        public async Task Remove_third_step_in_the_test_case()
        {
            ITmsClient client = CreateTmsAdoClient();
            IdName testSuite = await TmsAdoFixture.GetTestSuiteForTest(TestCaseContainerName);

            IEnumerable<TestStep> newTestSteps = new List<TestStep>()
            {
                CreateTestStep(1),
                CreateTestStep(2),
                CreateTestStep(3)
            };

            IReadOnlyCollection<IdName> addedTestCases =
                await client.AddTestCases(
                    testSuite,
                    new List<TestCase>
                    {
                        CreateAutomatedTestCase(
                            testSuite,
                            testCaseName: nameof(Remove_third_step_in_the_test_case),
                            testSteps: newTestSteps)
                    }
                );

            IdName? newTestCase = addedTestCases.FirstOrDefault();
            IEnumerable<TestStep> updateTestSteps = new List<TestStep>()
            {
                CreateTestStep(1),
                CreateTestStep(2)
            };

            IReadOnlyCollection<TestCase> updateTestCases = CreateAutomatedTestCaseAsList(
                testSuite,
                newTestCase.Name,
                testSteps: updateTestSteps
            );

            await client.UpdateTestCases(
                testSuite,
                updateTestCases
            );
        }

        [Fact]
        public async Task Update_first_step_add_new_attachments()
        {
            ITmsClient client = CreateTmsAdoClient();
            IdName testSuite = await TmsAdoFixture.GetTestSuiteForTest(TestCaseContainerName);

            IReadOnlyCollection<IdName> addedTestCases =
                await client.AddTestCases(
                    testSuite,
                    new List<TestCase>
                    {
                        CreateAutomatedTestCase(
                            testSuite,
                            testCaseName: nameof(Update_first_step_add_new_attachments),
                            testSteps: CreateTestStepAsList(1))
                    }
                );

            IdName? newTestCase = addedTestCases.FirstOrDefault();
            IEnumerable<TestStep> updateTestSteps = new List<TestStep>()
            {
                CreateTestStep(
                    1,
                    fileAttachments: new List<FileAttachment>()
                    {
                        Text1FileAttachment
                    })
            };

            IReadOnlyCollection<TestCase> updateTestCases = CreateAutomatedTestCaseAsList(
                testSuite,
                newTestCase.Name,
                testSteps: updateTestSteps
            );

            await client.UpdateTestCases(
                testSuite,
                updateTestCases
            );
        }
        
         [Fact]
        public async Task Update_first_step_replace_first_attachment()
        {
            ITmsClient client = CreateTmsAdoClient();
            IdName testSuite = await TmsAdoFixture.GetTestSuiteForTest(TestCaseContainerName);

            IEnumerable<TestStep> addTestSteps = new List<TestStep>()
            {
                CreateTestStep(
                    1,
                    fileAttachments: new List<FileAttachment>()
                    {
                        Text1FileAttachment,
                    })
            };

            IReadOnlyCollection<IdName> addedTestCases =
                await client.AddTestCases(
                    testSuite,
                    new List<TestCase>
                    {
                        CreateAutomatedTestCase(
                            testSuite,
                            testCaseName: nameof(Update_first_step_replace_first_attachment),
                            testSteps: addTestSteps)
                    }
                );

            IdName? newTestCase = addedTestCases.FirstOrDefault();
            IEnumerable<TestStep> updateTestSteps = new List<TestStep>()
            {
                CreateTestStep(
                    1,
                    fileAttachments: new List<FileAttachment>()
                    {
                        Image1FileAttachment
                    })
            };

            IReadOnlyCollection<TestCase> updateTestCases = CreateAutomatedTestCaseAsList(
                testSuite,
                newTestCase.Name,
                testSteps: updateTestSteps
            );

            await client.UpdateTestCases(
                testSuite,
                updateTestCases
            );
        }

        [Fact]
        public async Task Update_first_step_replace_second_attachment_of_three()
        {
            ITmsClient client = CreateTmsAdoClient();
            IdName testSuite = await TmsAdoFixture.GetTestSuiteForTest(TestCaseContainerName);

            IEnumerable<TestStep> addTestSteps = new List<TestStep>()
            {
                CreateTestStep(
                    1,
                    fileAttachments: new List<FileAttachment>()
                    {
                        Text1FileAttachment,
                        Text2FileAttachment,
                        Image2FileAttachment,
                    }),
                CreateTestStep(
                    2,
                    fileAttachments: new List<FileAttachment>()
                    {
                        Text2FileAttachment
                    })
            };

            IReadOnlyCollection<IdName> addedTestCases =
                await client.AddTestCases(
                    testSuite,
                    new List<TestCase>
                    {
                        CreateAutomatedTestCase(
                            testSuite,
                            testCaseName: nameof(Update_first_step_replace_second_attachment_of_three),
                            testSteps: addTestSteps)
                    }
                );

            IdName? newTestCase = addedTestCases.FirstOrDefault();
            IEnumerable<TestStep> updateTestSteps = new List<TestStep>()
            {
                CreateTestStep(
                    1,
                    fileAttachments: new List<FileAttachment>()
                    {
                        Text1FileAttachment,
                        Image1FileAttachment,
                        Image2FileAttachment,
                    }),
                CreateTestStep(
                    2,
                    fileAttachments: new List<FileAttachment>()
                    {
                        Text1FileAttachment
                    })
            };

            IReadOnlyCollection<TestCase> updateTestCases = CreateAutomatedTestCaseAsList(
                testSuite,
                newTestCase.Name,
                testSteps: updateTestSteps
            );

            await client.UpdateTestCases(
                testSuite,
                updateTestCases
            );
        }
    }
}