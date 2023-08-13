namespace LightBDD.ScenarioSync.Core.Entities.Parameters;

public record PrimitiveParameter
{
    public PrimitiveParameter(string value = "", string expectation = "")
    {
        Value = value;
        Expectation = expectation;
    }

    public string Value { get; }

    public string Expectation { get; }
}