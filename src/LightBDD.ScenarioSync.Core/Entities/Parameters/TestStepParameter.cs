using LightBDD.ScenarioSync.Core.Entities.Parameters.TableParameter;
using LightBDD.ScenarioSync.Core.Entities.Parameters.TreeParameter;

namespace LightBDD.ScenarioSync.Core.Entities.Parameters;

public record TestStepParameter
{
    public TestStepParameter(string name, Tree? tree = default, Table? table = default, PrimitiveParameter? value = default)
    {
        Value = value ?? new PrimitiveParameter();
        Name = name ?? string.Empty;
        Tree = tree ?? new Tree(Enumerable.Empty<Node>());
        Table = table ?? new Table(Enumerable.Empty<Column>(), Enumerable.Empty<Row>());
    }

    public string Name { get; }
    public Tree Tree { get; }

    public Table Table { get; }
    
    public PrimitiveParameter Value { get; }
}