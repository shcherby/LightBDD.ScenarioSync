using LightBDD.ScenarioSync.Core.Entities;
using LightBDD.ScenarioSync.Target.Ado.Models;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;

namespace LightBDD.ScenarioSync.Target.Ado.Helpers;

internal class RelationsHelper
{
    private WorkItemTrackingHttpClient _workItemClient;

    public RelationsHelper(WorkItemTrackingHttpClient workItemClient)
    {
        _workItemClient = workItemClient;
    }

    public async Task<IReadOnlyCollection<RelationOperation>> Prepare(Relations workItems)
    {
        if (!workItems.Any())
        {
            return new List<RelationOperation>();
        }

        IEnumerable<WorkItem?> associatedWorkItems = await GetWorkItems(workItems);
        IReadOnlyCollection<RelationOperation> associatedWorkItemsId = associatedWorkItems.Where(wi => wi is not null).Select(wi => new RelationOperation(wi.Id.Value, wi.Url)).ToList();
        return associatedWorkItemsId;
    }

    private async Task<IEnumerable<WorkItem>> GetWorkItems(IEnumerable<int> ids)
    {
        return await _workItemClient.GetWorkItemsAsync(ids, expand: WorkItemExpand.Relations, errorPolicy: WorkItemErrorPolicy.Omit);
    }
}