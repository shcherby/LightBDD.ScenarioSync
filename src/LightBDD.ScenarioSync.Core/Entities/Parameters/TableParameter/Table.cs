using System.Collections.ObjectModel;

namespace LightBDD.ScenarioSync.Core.Entities.Parameters.TableParameter;

public record Table
{
    public Table(IEnumerable<Column> columns, IEnumerable<Row> rows)
    {
        columns ??= Enumerable.Empty<Column>();
        Columns = new ReadOnlyCollection<Column>(columns.ToList());

        rows ??= Enumerable.Empty<Row>();
        Rows = new ReadOnlyCollection<Row>(rows.ToList());
    }

    public IReadOnlyCollection<Column> Columns { get; }

    public IReadOnlyCollection<Row> Rows { get; }
}
