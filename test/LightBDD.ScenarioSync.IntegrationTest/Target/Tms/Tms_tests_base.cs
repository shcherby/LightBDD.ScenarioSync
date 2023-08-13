using LightBDD.ScenarioSync.Core.App;
using LightBDD.ScenarioSync.Core.Entities;
using LightBDD.ScenarioSync.Core.Sync.Target;
using LightBDD.ScenarioSync.IntegrationTest.Helpers.Configurations;
using LightBDD.ScenarioSync.IntegrationTest.Target.Tms.Clients;
using LightBDD.ScenarioSync.Source.Factories;
using LightBDD.ScenarioSync.Target.Ado;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Moq;

namespace LightBDD.ScenarioSync.IntegrationTest.Target.Tms
{
    public abstract class Tms_tests_base
    {
        public TmsAdoFixture TmsAdoFixture { get; }
        public ITmsClientExtension TmsAdoClientExtension { get; }

        public string CreateUnique() => Guid.NewGuid().ToString();

        public Tms_tests_base(TmsAdoFixture adoTargetFixture)
        {
            TmsAdoFixture = adoTargetFixture;
            TmsAdoClientExtension = adoTargetFixture.TmsClientExtension;
        }

        protected TestSuite CreateTestSuite(IdName idName, IEnumerable<string>? labels = null)
        {
            labels ??= Enumerable.Empty<string>();
            List<string> labelsList = labels.ToList();
            return new TestSuite(idName.Name, "", new Tags(labelsList), RelationsFactory.Create(labelsList));
        }

        protected async Task RelationsContextRun(string workItemTitle, Func<WorkItem, Task> execute)
            => await RelationsContextRun(
                new[] { workItemTitle },
                async workItems => await execute.Invoke(workItems[0]));

        protected async Task RelationsContextRun(string[] workItemsTitle, Func<IReadOnlyList<WorkItem>, Task> execute)
        {
            List<WorkItem?> workItems = new List<WorkItem?>();
            try
            {
                foreach (string workItemTitle in workItemsTitle)
                {
                    workItems.Add(await TmsAdoClientExtension.CreateWorkItem(workItemTitle));
                }

                await execute.Invoke(workItems.Where(isNotNull).ToList());
            }
            finally
            {
                foreach (WorkItem? workItem in workItems.Where(isNotNull))
                {
                    await TmsAdoClientExtension.DeleteWorkItem(workItem.Id.Value);
                }
            }

            bool isNotNull(WorkItem? wi) => wi.Id is not null;
        }

        protected ITmsClient CreateTmsAdoClient()
            => new TmsAdoClient(SetupAppContextProviderMock().Object);

        protected ITmsClientExtension CreateTmsAdoClientExtension()
        {
            return new TmsAdoClientExtension(SetupAppContextProviderMock().Object);
        }

        protected Mock<IAppExecutionContext> SetupAppContextProviderMock()
        {
            var appContextProvider = new Mock<IAppExecutionContext>();
            appContextProvider
                .Setup(m => m.Arguments)
                .Returns(GetTestArguments());
            return appContextProvider;
        }

        protected AppArguments GetTestArguments()
            => new TestEnvConfigurations().GetAppArguments();
    }
}