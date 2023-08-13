using System.Collections.ObjectModel;

namespace LightBDD.ScenarioSync.Core.Entities.Parameters.TreeParameter;

public record Tree
{
    public Tree(IEnumerable<Node> nodes)
    {
        nodes ??= Enumerable.Empty<Node>();
        Nodes = new ReadOnlyCollection<Node>(nodes.ToList());
    }
    public IReadOnlyCollection<Node> Nodes { get; }

}
