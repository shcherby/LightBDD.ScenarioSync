namespace LightBDD.ScenarioSync.Core.Entities;

public static class IdNameExtensions
{
    public static IReadOnlyCollection<int> ToIds(this IEnumerable<IdName> idNames)
        => idNames.Select(idName => idName.Id).ToList();

    public static IReadOnlyCollection<string> ToNames(this IEnumerable<IdName> idNames)
        => idNames.Select(idName => idName.Name).ToList();
}