namespace LightBDD.ScenarioSync.Core.Entities.Parameters.TableParameter;

public record RowValue
{
    public RowValue(int index, string value, string? expectation = null)
    {
        Index = index;
        Value = value;
        Expectation = expectation ?? string.Empty;
    }

    public int Index { get; }

    public string Value { get; }

    public string Expectation { get; }
}
