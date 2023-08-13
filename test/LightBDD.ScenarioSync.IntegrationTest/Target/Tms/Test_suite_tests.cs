using FluentAssertions;
using LightBDD.ScenarioSync.Core.Entities;
using LightBDD.ScenarioSync.Source.Labels;

namespace LightBDD.ScenarioSync.IntegrationTest.Target.Tms
{
    [Collection(nameof(Test_suite_tests))]
    public class Test_suite_tests : Tms_tests_base
    {
        [CollectionDefinition(nameof(Test_suite_tests))]
        public class TmsAdoTargetCollection : ICollectionFixture<TmsAdoFixture>
        {
        }

        public Test_suite_tests(TmsAdoFixture adoTargetFixture)
            : base(adoTargetFixture)
        {
        }

        [Fact]
        public async Task Create_new_test_suite()
        {
            IdName? testSuiteId = null;
            try
            {
                var tags = new Tags(new List<string>() { "tag1", "tag2" });
                var ts = new TestSuite(nameof(Create_new_test_suite), "description", tags);
                testSuiteId = await CreateTmsAdoClient().GetOrCreateTestSuite(ts);

                testSuiteId.Id.Should().BePositive();
            }
            finally
            {
                await TmsAdoClientExtension.DeleteTestSuite(testSuiteId);
            }
        }

        [Fact]
        public async Task Update_exist_test_suite()
        {
            IdName? newTestSuiteId = null;
            try
            {
                var newTs = new TestSuite(nameof(Update_exist_test_suite), "description", new Tags(new[] { "tag1", "tag2" }));
                var updateTs = new TestSuite(nameof(Update_exist_test_suite), "description updated", new Tags(new[] { "tag1", "tag2Updated" }));
                newTestSuiteId = await CreateTmsAdoClient().GetOrCreateTestSuite(newTs);
                IdName updatedTestSuiteId = await CreateTmsAdoClient().GetOrCreateTestSuite(updateTs);
                await CreateTmsAdoClient().GetOrCreateTestSuite(updateTs);

                newTestSuiteId.Id.Should().BePositive();
                updatedTestSuiteId.Id.Should().Be(newTestSuiteId.Id);
            }
            finally
            {
                await TmsAdoClientExtension.DeleteTestSuite(newTestSuiteId);
            }
        }

        [Fact]
        public async Task Create_test_suite_with_relations()
        {
            await RelationsContextRun(
                new[]
                {
                    $"{nameof(Create_test_suite_with_relations)}_1",
                    $"{nameof(Create_test_suite_with_relations)}_2"
                },
                async workItems =>
                {
                    IdName? newTestSuiteId = null;
                    try
                    {
                        List<int?> workItemsIdList = workItems.Select(wi => wi.Id).ToList();
                        string workItemsId = string.Join(",", workItemsIdList);
                        string relatedWorkItemsLabel = $"{RelationsMetadataLabel.LabelPrefix}{workItemsId}";
                        var newTs = new TestSuite(nameof(Create_test_suite_with_relations), "description", new Tags(new[] { relatedWorkItemsLabel }));
                        newTestSuiteId = await CreateTmsAdoClient().GetOrCreateTestSuite(newTs);
                        string updateRelatedWorkItemsLabel = $"{RelationsMetadataLabel.LabelPrefix}{workItemsIdList[0]}";
                        var updateTs = new TestSuite(nameof(Create_test_suite_with_relations), description: "", new Tags(new[] { updateRelatedWorkItemsLabel }));
                        IdName updateTestSuiteId = await CreateTmsAdoClient().GetOrCreateTestSuite(updateTs);
                        IdName updateTestSuiteId2 = await CreateTmsAdoClient().GetOrCreateTestSuite(updateTs);

                        newTestSuiteId.Id.Should().BePositive();
                    }
                    finally
                    {
                        await TmsAdoClientExtension.DeleteTestSuite(newTestSuiteId);
                    }
                });
        }
    }
}