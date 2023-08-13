using System.Collections.ObjectModel;

namespace LightBDD.ScenarioSync.Core.Entities;

public record TestItemPath
{
    public TestItemPath(string testSuitePath = "")
    {
        path = testSuitePath ?? string.Empty;
        Levels = new ReadOnlyCollection<string>(ToSegments(path));
        if (IsRoot)
        {
            Name = string.Empty;
        }
        else
        {
            Name = Levels[LastLevelIndex];
            ParentPath = new TestItemPath(string.Join("\\", Levels.Take(LastLevelIndex)));
        }
    }

    public TestItemPath ParentPath { get; }

    public string Name { get; }

    private static List<string> ToSegments(string testSuitePath)
    {
        return testSuitePath.Split(new char[] { '/', '\\' }, StringSplitOptions.RemoveEmptyEntries).ToList();
    }

    private readonly string path;

    public readonly IReadOnlyList<string> Levels;

    public bool IsRoot => Levels.Count == 0;

    public int LastLevelIndex => Levels.Count - 1;

    public string GetNameOnTheLevel(int index) => Levels[index];

    public override string ToString()
    {
        return path;
    }
}