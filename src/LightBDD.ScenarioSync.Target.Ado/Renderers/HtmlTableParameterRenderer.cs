using System.Text;
using System.Text.Encodings.Web;
using LightBDD.ScenarioSync.Core.Entities.Parameters.TableParameter;

namespace LightBDD.ScenarioSync.Target.Ado.Renderers;
//var tableStyle = "text-align:left;font-size:13px;";
//var trStyle = "padding:5px; border-bottom:1px solid #f2f2f2;";
//var s = $"<table style=\"{tableStyle}\"><tr style=\"{trStyle}\"><th>test sadas</th></tr><tr style=\"{trStyle}\"><td>test sadas 2</td></tr></table>";
internal class HtmlTableParameterRenderer
{
    private const string TableStyle = "text-align:left;font-size:13px;";
    private const string TdStyle = "padding:5px 5px 5px 0px; border-bottom:1px solid #ccc;";
    private readonly TextColumn[] _columns;
    private readonly List<TextRow> _rows = new();

    public HtmlTableParameterRenderer(Table table)
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

    public string Render()
    {
        var writer = new StringBuilder();
        writer.Append($"<table style=\"{TableStyle}\">");
        // Write table head
        foreach (var column in _columns)
        {
            column.Render(writer);
        }

        // Write table body
        foreach (TextRow row in _rows)
        {
            writer.Append($"<tr>");
            row.Render(writer, _columns);
            writer.Append($"</tr>");
        }
        writer.Append($"</table>");
        return writer.ToString();
    }

    class TextRow
    {
        private readonly List<TextCell> _cells = new();

        public void Add(TextCell cell)
        {
            _cells.Add(cell);
        }

        public void Render(StringBuilder writer, TextColumn[] columns)
        {
            for (var i = 0; i < _cells.Count; i++)
            {
                writer.Append($"<td style=\"{TdStyle}\">{_cells[i].Text}</td>");
            }
        }
    }

    class TextColumn
    {
        private readonly string _name;

        public TextColumn(string name)
        {
            _name = Escape(name);
        }

        public void Render(StringBuilder writer)
        {
            writer.Append($"<th style=\"{TdStyle}\">{_name}</th>");
        }
    }
    private static string Escape(string text)
    {
        return HtmlEncoder.Default.Encode(text);
    }
}