using LightBDD.ScenarioSync.Core.Entities.Parameters.TreeParameter;

namespace LightBDD.ScenarioSync.Target.Ado.Renderers;

internal class TextTreeParameterRenderer : IStepParameterRenderer
{
    private readonly Tree _tree;

    public TextTreeParameterRenderer(Tree tree)
    {
        _tree = tree;
    }

    public void Render(TextWriter writer, string stepIntend)
    {
        var first = true;
        foreach (var node in _tree.Nodes)
        {
            if (!first)
                writer.WriteLine();
            writer.Write(stepIntend);
            writer.Write(" ");
            writer.Write(node.Path);

            writer.Write(Escape(node.Value));
            first = false;
        }
    }

    private static string Escape(string text)
    {
        return text.Replace("\r", "").Replace('\t', ' ').Replace("\n", " ").Replace("\b", "");
    }
}