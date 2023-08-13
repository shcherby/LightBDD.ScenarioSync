namespace LightBDD.ScenarioSync.Core.Entities.Parameters.TreeParameter;

public record Node
{
    public Node(string path, string value, string? expectation = null)
    {
        Path = path;
        Value = value;
        Expectation = expectation ?? string.Empty;
    }

    public string Path { get; }

    public string Value { get; }

    public string Expectation { get; }
}