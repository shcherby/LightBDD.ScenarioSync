namespace LightBDD.ScenarioSync.Source.Helpers;

public static class EnumerableExtensions
{
    public static IReadOnlyList<T> ListOrDefault<T>(this IEnumerable<T>? source)
    {
        return (source ?? new List<T>()).ToList();
    }
}