using FluentAssertions;
using LightBDD.ScenarioSync.Core.Entities;
using LightBDD.ScenarioSync.Core.Entities.Parameters;
using LightBDD.ScenarioSync.Core.Entities.Parameters.TableParameter;
using LightBDD.ScenarioSync.Core.Entities.Parameters.TreeParameter;
using LightBDD.ScenarioSync.Core.Sync.Target;
using LightBDD.ScenarioSync.IntegrationTest.Helpers.Extensions;
using LightBDD.ScenarioSync.Source.Factories;
using LightBDD.ScenarioSync.Source.Labels;
using LightBDD.ScenarioSync.Source.XmlParser.Models;
using FileAttachment = LightBDD.ScenarioSync.Core.Entities.FileAttachment;
using PrimitiveParameter = LightBDD.ScenarioSync.Core.Entities.Parameters.PrimitiveParameter;

namespace LightBDD.ScenarioSync.IntegrationTest.Target.Tms
{
    [Collection(nameof(Test_suite_adding_test_cases_tests))]
    public class Test_suite_adding_test_cases_tests : Test_suite_test_cases_tests_base
    {
        [CollectionDefinition(nameof(Test_suite_adding_test_cases_tests))]
        public class TmsAdoTargetCollection : ICollectionFixture<TmsAdoFixture>
        {
        }

        private const string TestCaseStepsTestSuiteContainerName = nameof(Test_suite_adding_test_cases_tests);

        public Test_suite_adding_test_cases_tests(TmsAdoFixture adoTargetFixture)
            : base(adoTargetFixture)
        {
        }

        [Fact]
        public async Task Assign_test_cases_to_work_item_successfully()
        {
            await RelationsContextRun(
                nameof(Assign_test_cases_to_work_item_successfully),
                async workItem =>
                {
                    IdName testSuite = await TmsAdoFixture.GetTestSuiteForTest(TestCaseStepsTestSuiteContainerName);

                    IReadOnlyCollection<TestCase> testCases =
                        CreateAutomatedTestCaseAsList(
                            testSuite,
                            nameof(Assign_test_cases_to_work_item_successfully),
                            testCaseLabels: new[] { GetRelationsLabel(new[] { workItem.Id.Value }), "OtherLabel" }
                        );

                    await CreateTmsAdoClient().AddTestCases(testSuite, testCases);
                });
        }

        [Fact]
        public async Task Assign_test_cases_to_two_work_items_successfully()
        {
            await RelationsContextRun(
                new[]
                {
                    $"{nameof(Assign_test_cases_to_two_work_items_successfully)}_1",
                    $"{nameof(Assign_test_cases_to_two_work_items_successfully)}_2"
                },
                async workItems =>
                {
                    IdName testSuite = await TmsAdoFixture.GetTestSuiteForTest(TestCaseStepsTestSuiteContainerName);
                    IEnumerable<int> workItemsId = workItems.Select(wi => wi.Id.Value);

                    IReadOnlyCollection<TestCase> testCases =
                        CreateAutomatedTestCaseAsList(
                            testSuite,
                            nameof(Assign_test_cases_to_two_work_items_successfully),
                            testCaseLabels: new[] { GetRelationsLabel(workItemsId), "OtherLabel" }
                        );

                    await CreateTmsAdoClient().AddTestCases(testSuite, testCases);
                });
        }

        [Fact]
        public async Task Replace_work_item_relation_in_the_test_case_successfully()
        {
            await RelationsContextRun(
                new[]
                {
                    $"{nameof(Assign_test_cases_to_two_work_items_successfully)}_1",
                    $"{nameof(Assign_test_cases_to_two_work_items_successfully)}_original",
                    $"{nameof(Assign_test_cases_to_two_work_items_successfully)}_replaced"
                },
                async workItems =>
                {
                    IdName testSuite = await TmsAdoFixture.GetTestSuiteForTest(TestCaseStepsTestSuiteContainerName);
                    IEnumerable<int> originalWorkItemsId = new[] { workItems[0].Id.Value, workItems[1].Id.Value };

                    IReadOnlyCollection<TestCase> newTestCases =
                        CreateAutomatedTestCaseAsList(
                            testSuite,
                            nameof(Assign_test_cases_to_two_work_items_successfully),
                            testCaseLabels: new[] { GetRelationsLabel(originalWorkItemsId), "OtherLabel" }
                        );

                    await CreateTmsAdoClient().AddTestCases(testSuite, newTestCases);

                    IEnumerable<int> replaceWorkItemsId = new[] { workItems[0].Id.Value, workItems[2].Id.Value };
                    IReadOnlyCollection<TestCase> updateTestCases =
                        CreateAutomatedTestCaseAsList(
                            testSuite,
                            nameof(Assign_test_cases_to_two_work_items_successfully),
                            testCaseLabels: new[] { GetRelationsLabel(replaceWorkItemsId), "OtherLabel" }
                        );

                    await CreateTmsAdoClient().UpdateTestCases(testSuite, updateTestCases);
                });
        }

        [Fact]
        public async Task Remove_work_item_relations_in_the_test_case_successfully()
        {
            await RelationsContextRun(
                new[]
                {
                    $"{nameof(Assign_test_cases_to_two_work_items_successfully)}_1",
                    $"{nameof(Assign_test_cases_to_two_work_items_successfully)}_2"
                },
                async workItems =>
                {
                    IdName testSuite = await TmsAdoFixture.GetTestSuiteForTest(TestCaseStepsTestSuiteContainerName);
                    IEnumerable<int> originalWorkItemsId = new[] { workItems[0].Id.Value, workItems[1].Id.Value };

                    IReadOnlyCollection<TestCase> newTestCases =
                        CreateAutomatedTestCaseAsList(
                            testSuite,
                            nameof(Assign_test_cases_to_two_work_items_successfully),
                            testCaseLabels: new[] { GetRelationsLabel(originalWorkItemsId), "OtherLabel" }
                        );

                    await CreateTmsAdoClient().AddTestCases(testSuite, newTestCases);

                    IEnumerable<int> removeWorkItemsId = new int[0];
                    IReadOnlyCollection<TestCase> updateTestCases =
                        CreateAutomatedTestCaseAsList(
                            testSuite,
                            nameof(Assign_test_cases_to_two_work_items_successfully),
                            testCaseLabels: new[] { GetRelationsLabel(removeWorkItemsId), "OtherLabel" }
                        );

                    await CreateTmsAdoClient().UpdateTestCases(testSuite, updateTestCases);
                });
        }

        [Fact]
        public async Task Assign_test_cases_to_not_exist_work_item_create_test_case_successfully()
        {
            IdName testSuite = await TmsAdoFixture.GetTestSuiteForTest(TestCaseStepsTestSuiteContainerName);

            IReadOnlyCollection<TestCase> testCases =
                CreateAutomatedTestCaseAsList(
                    testSuite,
                    nameof(Assign_test_cases_to_not_exist_work_item_create_test_case_successfully),
                    testCaseLabels: new[] { GetRelationsLabel(new[] { 999999 }), "OtherLabel" }
                );

            await CreateTmsAdoClient().AddTestCases(testSuite, testCases);
        }

        [Fact]
        public async Task Get_no_test_cases_from_empty_test_suite()
        {
            IdName testSuite = await TmsAdoFixture.GetTestSuiteForTest(nameof(Get_no_test_cases_from_empty_test_suite));

            IReadOnlyCollection<IdName> testCases = await CreateTmsAdoClient().GetTestCases(testSuite);

            testCases.Should().BeEmpty();
        }

        [Fact]
        public async Task Add_two_test_cases_to_test_suite_and_get_two()
        {
            IdName testSuite = await TmsAdoFixture.GetTestSuiteForTest(nameof(Add_two_test_cases_to_test_suite_and_get_two));

            ITmsClient client = CreateTmsAdoClient();
            await client.AddTestCases(
                testSuite,
                new List<TestCase>
                {
                    CreateAutomatedTestCase(testSuite),
                    CreateAutomatedTestCase(testSuite)
                }
            );

            IReadOnlyCollection<IdName> actualTestCases = await TmsAdoClientExtension.GetTestCases(testSuite);

            actualTestCases.Should().HaveCount(2);
        }

        [Fact]
        public async Task Add_step_without_parameters_to_test_case()
        {
            ITmsClient client = CreateTmsAdoClient();
            IdName testSuite = await TmsAdoFixture.GetTestSuiteForTest(TestCaseStepsTestSuiteContainerName);
            await client.AddTestCases(
                testSuite,
                new List<TestCase>
                {
                    CreateAutomatedTestCase(
                        testSuite,
                        testCaseName: nameof(Add_step_without_parameters_to_test_case),
                        testSteps: CreateTestStepAsList(1))
                }
            );
        }

        [Fact]
        public async Task Add_step_with_substeps_to_test_case()
        {
            ITmsClient client = CreateTmsAdoClient();
            IdName testSuite = await TmsAdoFixture.GetTestSuiteForTest(TestCaseStepsTestSuiteContainerName);

            var steps = new List<TestStep>()
            {
                TestStepFactory.Create(
                    new Step()
                    {
                        Name = $"Step-name-{Guid.NewGuid()}",
                        Number = 1,
                        SubSteps = new List<Step>()
                        {
                            new Step()
                            {
                                Name = $"Step-name-{Guid.NewGuid()}",
                                Number = 1,
                                GroupPrefix = "1."
                            },
                            new Step()
                            {
                                Name = $"Step-name-{Guid.NewGuid()}",
                                Number = 2,
                                GroupPrefix = "1."
                            }
                        }
                    }),
                TestStepFactory.Create(
                    new Step()
                    {
                        Name = $"Step-name-{Guid.NewGuid()}",
                        Number = 2,
                        SubSteps = new List<Step>()
                        {
                            new Step()
                            {
                                Name = $"Step-name-{Guid.NewGuid()}",
                                Number = 1,
                                GroupPrefix = "2."
                            },
                            new Step()
                            {
                                Name = $"Step-name-{Guid.NewGuid()}",
                                Number = 2,
                                GroupPrefix = "2.",
                            }
                        }
                    })
            };

            await client.AddTestCases(
                testSuite,
                new List<TestCase>
                {
                    CreateAutomatedTestCase(
                        testSuite,
                        testCaseName: nameof(Add_step_with_substeps_to_test_case),
                        testSteps: steps)
                }
            );
        }


        [Fact]
        public async Task Add_step_with_primitive_parameters_and_expectation_to_test_case()
        {
            ITmsClient client = CreateTmsAdoClient();
            IdName testSuite = await TmsAdoFixture.GetTestSuiteForTest(TestCaseStepsTestSuiteContainerName);

            var primitiveParameters = new List<TestStepParameter>
            {
                new("name", value: new PrimitiveParameter("test-name", "test-name")),
                new("name2", value: new PrimitiveParameter("test-name2", "test-name2"))
            };

            IEnumerable<TestStep> testSteps = CreateTestStepAsList(1, parameters: primitiveParameters);
            await client.AddTestCases(
                testSuite,
                new List<TestCase>
                {
                    CreateAutomatedTestCase(
                        testSuite,
                        testCaseName: nameof(Add_step_with_primitive_parameters_and_expectation_to_test_case),
                        testSteps: testSteps)
                }
            );
        }

        [Fact]
        public async Task Add_step_with_table_parameter_to_test_case()
        {
            ITmsClient client = CreateTmsAdoClient();
            IdName testSuite = await TmsAdoFixture.GetTestSuiteForTest(TestCaseStepsTestSuiteContainerName);
            IEnumerable<TestStep> testSteps = CreateTestStepAsList(1, parameters: CreateTableParameterAsList("table-param1"));

            await client.AddTestCases(
                testSuite,
                new List<TestCase>
                {
                    CreateAutomatedTestCase(
                        testSuite,
                        testCaseName: nameof(Add_step_with_table_parameter_to_test_case),
                        testSteps: testSteps)
                }
            );
        }

        [Fact]
        public async Task Add_step_with_tree_parameter_to_test_case()
        {
            ITmsClient client = CreateTmsAdoClient();
            IdName testSuite = await TmsAdoFixture.GetTestSuiteForTest(TestCaseStepsTestSuiteContainerName);
            IEnumerable<TestStep> testSteps = CreateTestStepAsList(1, parameters: CreateTreeParameterAsList("tree-param1"));

            await client.AddTestCases(
                testSuite,
                new List<TestCase>
                {
                    CreateAutomatedTestCase(
                        testSuite,
                        testCaseName: nameof(Add_step_with_table_and_step_with_tree_parameter_to_test_case),
                        testSteps: testSteps)
                }
            );
        }

        [Fact]
        public async Task Add_step_with_table_and_step_with_tree_parameter_to_test_case()
        {
            ITmsClient client = CreateTmsAdoClient();
            IdName testSuite = await TmsAdoFixture.GetTestSuiteForTest(TestCaseStepsTestSuiteContainerName);

            IEnumerable<TestStep> testSteps = CreateTestStepAsList(
                1,
                parameters: new List<TestStepParameter>()
                {
                    CreateTableParameter("table-param1"),
                    CreateTreeParameter("tree-param2")
                }
            );

            await client.AddTestCases(
                testSuite,
                new List<TestCase>
                {
                    CreateAutomatedTestCase(
                        testSuite,
                        testCaseName: nameof(Add_step_with_table_and_step_with_tree_parameter_to_test_case),
                        testSteps: testSteps)
                }
            );
        }

        [Fact]
        public async Task Add_step_with_comments_to_test_case()
        {
            ITmsClient client = CreateTmsAdoClient();
            IdName testSuite = await TmsAdoFixture.GetTestSuiteForTest(TestCaseStepsTestSuiteContainerName);

            IEnumerable<TestStep> testSteps = CreateTestStepAsList(
                1,
                comments: GenerateComments(4)
            );

            await client.AddTestCases(
                testSuite,
                new List<TestCase>
                {
                    CreateAutomatedTestCase(
                        testSuite,
                        testCaseName: nameof(Add_step_with_comments_to_test_case),
                        testSteps: testSteps)
                }
            );
        }

        [Fact]
        public async Task Add_step_with_attachments_to_test_case()
        {
            ITmsClient client = CreateTmsAdoClient();
            IdName testSuite = await TmsAdoFixture.GetTestSuiteForTest(TestCaseStepsTestSuiteContainerName);

            IEnumerable<TestStep> testSteps = CreateTestStepAsList(
                1,
                fileAttachments: GetTestFileAttachments()
            );

            await client.AddTestCases(
                testSuite,
                new List<TestCase>
                {
                    CreateAutomatedTestCase(
                        testSuite,
                        testCaseName: nameof(Add_step_with_attachments_to_test_case),
                        testSteps: testSteps)
                }
            );
        }

        [Fact]
        public async Task Add_step_with_fully_filled_sections_to_test_case()
        {
            ITmsClient client = CreateTmsAdoClient();
            IdName testSuite = await TmsAdoFixture.GetTestSuiteForTest(TestCaseStepsTestSuiteContainerName);

            var stepParameters = new List<TestStepParameter>
            {
                new("primitiveParameter1", value: new PrimitiveParameter("value 1", "expectation 1")),
                new("primitiveParameter2", value: new PrimitiveParameter("value 2", "expectation 2")),
                CreateTableParameter("table-param1"),
                CreateTreeParameter("tree-param2")
            };

            IEnumerable<TestStep> testSteps = CreateTestStepAsList(
                1,
                parameters: stepParameters,
                comments: GenerateComments(4),
                fileAttachments: GetTestFileAttachments()
            );

            await client.AddTestCases(
                testSuite,
                new List<TestCase>
                {
                    CreateAutomatedTestCase(
                        testSuite,
                        testCaseName: nameof(Add_step_with_fully_filled_sections_to_test_case),
                        testSteps: testSteps)
                }
            );
        }

        private string GetRelationsLabel(IEnumerable<int> ids)
            => $"{RelationsMetadataLabel.LabelPrefix}{string.Join(",", ids)}";

        private List<FileAttachment> GetTestFileAttachments()
        {
            return new List<FileAttachment>
            {
                Text1FileAttachment,
                Image1FileAttachment,
            };
        }

        private static List<string> GenerateComments(int i)
        {
            return Enumerable.Range(start: 1, i).Select(x => $"Test comment with content{x}").ToList();
        }

        private static List<FileAttachment> CreateFileAttachments(int i)
        {
            return Enumerable.Range(start: 1, i).Select(x => new FileAttachment($"Test file with content {x}", $"/parent/path-to-file-{x}.jpg")).ToList();
        }

        private static IReadOnlyCollection<TestStepParameter> CreateTableParameterAsList(string name)
        {
            return CreateTableParameter(name).ToList();
        }

        private static TestStepParameter CreateTableParameter(string name)
        {
            var columns = new List<Column>
            {
                new("column1", index: 1, isKey: false),
                new("column2", index: 2, isKey: false),
                new("column3", index: 3, isKey: false)
            };
            var rows = new List<Row>
            {
                new(index: 1, new List<RowValue> { new(index: 1, "row-value-1.1", "row-expectation-1.1"), new(index: 1, "row-value-2.1", "row-expectation-2.1"), new(index: 1, "row-value-3.1", "row-expectation-3.1") }),
                new(index: 2, new List<RowValue> { new(index: 2, "row-value-1.1", "row-expectation-1.1"), new(index: 2, "row-value-2.2", "row-expectation-2.2"), new(index: 2, "row-value-3.2", "row-expectation-3.2") }),
                new(index: 3, new List<RowValue> { new(index: 3, "row-value-1.1", "row-expectation-1.1"), new(index: 3, "row-value-2.3", "row-expectation-2.3"), new(index: 3, "row-value-3.3", "row-expectation-3.1") })
            };
            var table = new Table(columns, rows);
            return new TestStepParameter(name, tree: null, table);
        }

        private static IReadOnlyCollection<TestStepParameter> CreateTreeParameterAsList(string name)
        {
            return CreateTreeParameter(name).ToList();
        }

        private static TestStepParameter CreateTreeParameter(string name)
        {
            var nodes = new List<Node>
            {
                new("$", "object"),
                new("$.Address", "Rynek Główny 1"),
                new("$.City", "Kraków")
            };

            var tree = new Tree(nodes);
            return new TestStepParameter(name, tree);
        }
    }
}