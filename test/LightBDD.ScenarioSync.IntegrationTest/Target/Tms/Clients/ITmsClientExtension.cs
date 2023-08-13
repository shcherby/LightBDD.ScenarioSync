using LightBDD.ScenarioSync.Core.Sync.Target;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;

namespace LightBDD.ScenarioSync.IntegrationTest.Target.Tms.Clients;

public interface ITmsClientExtension: ITmsClient
{
    Task<WorkItem> GetWorkItem(int id);
    Task<WorkItemDelete> DeleteWorkItem(int id);
    Task<WorkItem> CreateWorkItem(string title);
    Task<IEnumerable<WorkItem>> GetWorkItems(IEnumerable<int> ids);
}