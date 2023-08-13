using LightBDD.ScenarioSync.Core.Entities;

namespace LightBDD.ScenarioSync.UnitTest.Comparers;

internal class TestCaseNameComparer : IEqualityComparer<TestCase>
{
    public bool Equals(TestCase? current, TestCase? other)
    {
        if (ReferenceEquals(null, other)) return false;
        return current.Name == other.Name;
    }

    public int GetHashCode(TestCase obj)
    {
        return HashCode.Combine(obj.Name);
    }
}