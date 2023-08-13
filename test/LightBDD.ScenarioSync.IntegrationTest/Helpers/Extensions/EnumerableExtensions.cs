namespace LightBDD.ScenarioSync.IntegrationTest.Helpers.Extensions;

public static class EnumerableExtensions
{
    public static IReadOnlyCollection<T> ToList<T>(this T single) =>
        new List<T>() { single };
}
