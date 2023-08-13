using LightBDD.ScenarioSync.Core.Entities.Parameters.TreeParameter;
using System.Text;
using System.Text.Encodings.Web;

namespace LightBDD.ScenarioSync.Target.Ado.Renderers;

internal class HtmlTreeParameterRenderer
{
    private readonly Tree _tree;

    public HtmlTreeParameterRenderer(Tree tree)
    {
        _tree = tree;
    }

    public string Render()
    {
        var sb = new StringBuilder();
        foreach (var node in _tree.Nodes)
        {
            sb.Append($"<div>{Escape($"{node.Path} {node.Value}")}</div>");
        }
        return sb.ToString();
    }

    private static string Escape(string text)
    {
        return HtmlEncoder.Default.Encode(text);
    }
}