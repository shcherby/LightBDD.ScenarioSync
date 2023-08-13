using System.Text.RegularExpressions;
using LightBDD.ScenarioSync.Target.Ado.Constants;
using Microsoft.TeamFoundation.WorkItemTracking.WebApi.Models;

namespace LightBDD.ScenarioSync.Target.Ado.Helpers;

public static class WorkItemExtensions
{
    private static Regex HtmlRegex = new Regex("<.*?>", RegexOptions.Compiled);

    public static string GetTitle(this WorkItem workItem)
    {
        return GetFieldValueOrDefault(workItem, WorkItemFieldsConst.Title);
    }

    private static string GetFieldValueOrDefault(WorkItem workItem, string fieldName)
    {
        if (workItem is null)
        {
            return string.Empty;
        }

        object name = string.Empty;
        return workItem.Fields.TryGetValue(fieldName, out name) ? (string)name : string.Empty;
    }

    public static string GetDescription(this WorkItem workItem)
    {
        return GetFieldValueOrDefault(workItem, WorkItemFieldsConst.Description);
    }

    public static string GetDescriptionSanitized(this WorkItem workItem)
    {
        string description = GetFieldValueOrDefault(workItem, WorkItemFieldsConst.Description);
        return HtmlRegex.Replace(description, string.Empty);
    }

    public static string GetTagsStr(this WorkItem workItem)
    {
        return GetFieldValueOrDefault(workItem, WorkItemFieldsConst.Tags);
    }

    public static IReadOnlyCollection<int> GetRelationsId(this WorkItem workItem)
    {
        IList<WorkItemRelation> relationsList = workItem.Relations ?? new List<WorkItemRelation>();
        var ids = new List<int>();
        foreach (WorkItemRelation relation in relationsList)
        {
            if (relation.Rel == WorkItemRelationConst.TestedByReverseRel)
            {
                var url = new Uri(relation.Url);
                string idSegment = url.Segments.Last();
                if (Int32.TryParse(idSegment, out int id))
                {
                    ids.Add(id);
                }
            }
        }

        return ids;
    }


    public static IList<string> GetTags(this WorkItem workItem)
    {
        string tagsStr = GetFieldValueOrDefault(workItem, WorkItemFieldsConst.Tags);
        if (string.IsNullOrEmpty(tagsStr))
        {
            return new List<string>();
        }

        return tagsStr.Replace(" ", "").Split(";", StringSplitOptions.RemoveEmptyEntries);
    }
}