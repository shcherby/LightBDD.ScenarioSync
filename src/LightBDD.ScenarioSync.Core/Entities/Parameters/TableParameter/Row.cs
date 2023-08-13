using System.Collections.ObjectModel;

namespace LightBDD.ScenarioSync.Core.Entities.Parameters.TableParameter;

public record Row
{
    public Row(int index, IEnumerable<RowValue> values)
    {
        Index = index;
        values ??= Enumerable.Empty<RowValue>();
        Values = new ReadOnlyCollection<RowValue>(values.ToList());
    }
    public int Index { get; }

    public IReadOnlyList<RowValue> Values { get; }
}
