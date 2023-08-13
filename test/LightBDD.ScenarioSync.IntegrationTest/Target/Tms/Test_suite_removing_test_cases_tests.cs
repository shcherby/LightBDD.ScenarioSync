using LightBDD.ScenarioSync.Core.Entities;
using LightBDD.ScenarioSync.IntegrationTest.Helpers.Extensions;

namespace LightBDD.ScenarioSync.IntegrationTest.Target.Tms
{
    [Collection(nameof(Test_suite_removing_test_cases_tests))]
    public class Test_suite_removing_test_cases_tests : Test_suite_test_cases_tests_base
    {
        [CollectionDefinition(nameof(Test_suite_removing_test_cases_tests))]
        public class TmsAdoTargetCollection : ICollectionFixture<TmsAdoFixture>
        {
        }

        private const string TestCaseStepsTestSuiteContainerName = nameof(Test_suite_removing_test_cases_tests);

        public Test_suite_removing_test_cases_tests(TmsAdoFixture adoTargetFixture)
            : base(adoTargetFixture)
        {
        }

        [Fact]
        public async Task Remove_one_test_case_and_keep_test_suite()
        {
            IdName testSuite = await TmsAdoFixture.GetTestSuiteForTest(TestCaseStepsTestSuiteContainerName);

            IReadOnlyCollection<TestCase> testCases =
                new[]
                {
                    CreateAutomatedTestCase(
                        testSuite,
                        $"{nameof(Remove_one_test_case_and_keep_test_suite)}_1"
                    ),
                    CreateAutomatedTestCase(
                        testSuite,
                        $"{nameof(Remove_one_test_case_and_keep_test_suite)}_2"
                    ),
                };

            IReadOnlyList<IdName> added = await CreateTmsAdoClient().AddTestCases(testSuite, testCases);

            IReadOnlyCollection<IdName> removeTestCases = added[1].ToList();

            await CreateTmsAdoClient().DeleteTestCases(removeTestCases);
        }

        [Fact]
        public async Task Remove_two_test_cases_and_empty_test_suite_successfully()
        {
            IdName testSuite = await TmsAdoClientExtension.GetOrCreateTestSuite(new TestSuite(TestCaseStepsTestSuiteContainerName));

            IReadOnlyCollection<TestCase> testCases =
                new[]
                {
                    CreateAutomatedTestCase(
                        testSuite,
                        $"{nameof(Remove_two_test_cases_and_empty_test_suite_successfully)}_1"
                    ),
                    CreateAutomatedTestCase(
                        testSuite,
                        $"{nameof(Remove_two_test_cases_and_empty_test_suite_successfully)}_2"
                    ),
                };

            IReadOnlyList<IdName> added = await CreateTmsAdoClient().AddTestCases(testSuite, testCases);
            await CreateTmsAdoClient().DeleteTestCases(added);
        }
    }
}