namespace LightBDD.ScenarioSync.Core.Entities.Parameters.TableParameter;

public record Column : ObjectProperties
{
    public Column(string name, int index, bool isKey)
        : base(name)
    {
        Index = index;
        IsKey = isKey;
    }

    public int Index { get; }

    public bool IsKey { get; }
}
