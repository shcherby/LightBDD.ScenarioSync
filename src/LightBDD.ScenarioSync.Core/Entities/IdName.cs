namespace LightBDD.ScenarioSync.Core.Entities;

public record IdName
{
    public IdName(int id, string name)
    {
        Id = id;
        Name = name ?? throw new ArgumentNullException(nameof(name));

    }

    public int Id { get; }

    public string Name { get; }
}