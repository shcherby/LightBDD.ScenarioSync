using LightBDD.ScenarioSync.Core.App;
using LightBDD.ScenarioSync.Core.Entities;
using LightBDD.ScenarioSync.Target.Ado;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;
using Microsoft.VisualStudio.Services.WebApi.Patch;
using Microsoft.VisualStudio.Services.WebApi.Patch.Json;

namespace LightBDD.ScenarioSync.IntegrationTest.Target.Tms.Clients;

internal class TmsAdoClientExtension : TmsAdoClient, ITmsClientExtension
{
    public TmsAdoClientExtension(IAppExecutionContext appExecutionContext) : base(appExecutionContext)
    {
    }

    public async Task<WorkItem> GetWorkItem(int id)
    {
        return await WorkItemClient.GetWorkItemAsync(id, expand: WorkItemExpand.All);
    }

    public async Task<WorkItem> CreateWorkItem(string title)
    {
        JsonPatchDocument patchDocument = new JsonPatchDocument();
        patchDocument.Add(
            new JsonPatchOperation()
            {
                Operation = Operation.Add,
                Path = "/fields/System.Title",
                Value = title
            });
        return await WorkItemClient.CreateWorkItemAsync(patchDocument, ProjectName, "Task");
    }
    
    public async Task<WorkItemDelete> DeleteWorkItem(int id)
    {
        return await WorkItemClient.DeleteWorkItemAsync(id);
    }

    public async Task<IEnumerable<WorkItem>> GetWorkItems(IEnumerable<int> ids)
    {
        return await WorkItemClient.GetWorkItemsAsync(ids, expand: WorkItemExpand.Relations);
    }
}