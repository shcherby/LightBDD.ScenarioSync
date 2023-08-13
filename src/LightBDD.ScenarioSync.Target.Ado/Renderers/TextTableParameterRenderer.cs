using System.Text;
using LightBDD.ScenarioSync.Core.Entities.Parameters.TableParameter;

namespace LightBDD.ScenarioSync.Target.Ado.Renderers;

internal class TextTableParameterRenderer : IStepParameterRenderer
{
    private readonly TextColumn[] _columns;
    private readonly List<TextRow> _rows = new();

    public TextTableParameterRenderer(Table table)
    {
        _columns = table.Columns.Select(c => new TextColumn(c.Name)).ToArray();
        foreach (var row in table.Rows)
            AddRow(row);
    }

    private void AddRow(Row row)
    {
        var textRow = new TextRow();
        for (var i = 0; i < row.Values.Count; i++)
        {
            var cell = new TextCell(row.Values[i]);
            textRow.Add(cell);
            _columns[i].EnsureFit(cell.Text);
        }
        _rows.Add(textRow);
    }

    private struct TextCell
    {
        public TextCell(RowValue result)
        {
            Text = Escape(result.Value);
        }

        public string Text { get; }
    }

    public string Render(string prefix)
    {
        var builder = new StringBuilder();
        using (var writer = new StringWriter(builder))
            Render(writer, prefix);
        return builder.ToString();
    }

    public void Render(TextWriter writer, string prefix)
    {
        WriteHRule(writer, prefix);
        writer.WriteLine();
        writer.Write(prefix);
        writer.Write("|");

        foreach (var column in _columns)
        {
            column.Render(writer);
            writer.Write('|');
        }
        writer.WriteLine();
        WriteHRule(writer, prefix);
        writer.WriteLine();

        foreach (var row in _rows)
        {
            writer.Write(prefix);
            row.Render(writer, _columns);
        }
        WriteHRule(writer, prefix);
    }

    private void WriteHRule(TextWriter writer, string prefix)
    {
        writer.Write(prefix);
        writer.Write("+");
        foreach (var column in _columns)
        {
            WriteFill(writer, '-', column.Size);
            writer.Write('+');
        }
    }

    private static void WriteFill(TextWriter writer, char c, int repeat)
    {
        while (--repeat >= 0)
            writer.Write(c);
    }

    class TextRow
    {
        private readonly List<TextCell> _cells = new();

        public void Add(TextCell cell)
        {
            _cells.Add(cell);
        }

        public void Render(TextWriter writer, TextColumn[] columns)
        {
            writer.Write('|');
            for (var i = 0; i < _cells.Count; i++)
            {
                WritePadded(writer, _cells[i].Text, columns[i].Size);
                writer.Write('|');
            }
            writer.WriteLine();
        }
    }
    class TextColumn
    {
        private readonly string _name;
        public int Size { get; private set; }

        public TextColumn(string name)
        {
            _name = Escape(name);
            EnsureFit(_name);
        }

        public void EnsureFit(string text)
        {
            Size = Math.Max(Size, text?.Length ?? 0);
        }

        public void Render(TextWriter writer)
        {
            WritePadded(writer, _name, Size);
        }
    }
    private static string Escape(string text)
    {
        return text.Replace("\r", "").Replace('\t', ' ').Replace("\n", " ").Replace("\b", "");
    }

    private static void WritePadded(TextWriter writer, string text, int size)
    {
        writer.Write(text);
        for (var i = size - text.Length; i > 0; --i)
            writer.Write(' ');
    }
}